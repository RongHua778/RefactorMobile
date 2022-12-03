using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Destination
{
    boss, target, Random
}

public abstract class Aircraft : ReusableObject, IDamage, IGameBehavior
{
    public string ExplosionSound => "Sound_EnemyExplosion";
    public string ExplosionEffect => "EnemyExplosionBlue";

    [SerializeField] private ReusableObject ExplosionPrefab = default;

    [SerializeField] private HealthBar_Sprie m_HealthBar = default;
    [SerializeField] private SpriteRenderer aircraftSprite = default;
    [SerializeField] private Collider2D aircraftCol = default;


    public HealthBar_Sprie HealthBar { get => m_HealthBar; set => m_HealthBar = value; }
    public DamageStrategy DamageStrategy { get; set; }
    public SpriteRenderer gfxSprite { get => aircraftSprite; set => aircraftSprite = value; }
    public Collider2D TargetCollider { get => aircraftCol; set => aircraftCol = value; }


    [HideInInspector] public AircraftCarrier boss;
    [HideInInspector] public ConcreteContent targetTurret;

    Quaternion look_Rotation;
    protected float exploreRange = 10f;
    protected Collider2D[] attachedResult = new Collider2D[10];
    List<ConcreteContent> turrets = new List<ConcreteContent>();
    public readonly float minDistanceToLure = .1f;
    public readonly float minDistanceToDealDamage = 0.75f;
    [SerializeField] float maxDistanceToReturnToBoss = 5f;
    protected float movingSpeed = 3.5f;
    protected float rotatingSpeed = 2f;
    protected float originalMovingSpeed = 3.5f;
    protected float originalRotatingSpeed = 2f;

    protected Vector3 movingDirection;

    protected FSMSystem fsm;


    public virtual void Initiate(AircraftCarrier boss, float maxHealth, float dmgIntenWhenDie, float dmgResist)
    {
        DamageStrategy = new AircraftStrategy(this, dmgIntenWhenDie, dmgResist);
        DamageStrategy.MaxHealth = maxHealth;
        DamageStrategy.IsDie = false;
        GameManager.Instance.nonEnemies.Add(this);
        boss.AddAircraft(this);
    }


    public virtual bool GameUpdate()
    {
        if (DamageStrategy.IsDie)
        {
            OnDie();
            ObjectPool.Instance.UnSpawn(this);
            return false;
        }
        return true;
    }
    private void OnDie()
    {
        ReusableObject explosion = ObjectPool.Instance.Spawn(ExplosionPrefab);
        explosion.transform.position = transform.position;
        Sound.Instance.PlayEffect(ExplosionSound);
    }


    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        fsm = null;
    }

    public void PickRandomDes()
    {
        float randomX = Random.Range(boss.transform.position.x - maxDistanceToReturnToBoss,
            boss.transform.position.x + maxDistanceToReturnToBoss);
        float randomY = Random.Range(boss.transform.position.y - maxDistanceToReturnToBoss,
            boss.transform.position.y + maxDistanceToReturnToBoss);
        movingDirection = new Vector3(randomX, randomY) - transform.position;
    }

    public void MovingToTarget(Destination des)
    {
        switch (des)
        {
            case Destination.boss:
                movingDirection = boss.model.transform.position - transform.position;
                break;
            case Destination.target:
                movingDirection = targetTurret.transform.position - transform.position;
                break;
            case Destination.Random:
                break;
            default:
                Debug.LogAssertion("飞行目的地错误！");
                break;
        }

        transform.Translate(Vector3.up * Time.deltaTime * movingSpeed);
        RotateTowards();
    }

    private void RotateTowards()
    {
        var angle = Mathf.Atan2(movingDirection.y, movingDirection.x) * Mathf.Rad2Deg - 90f;
        look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, look_Rotation,
            rotatingSpeed * Time.deltaTime);
    }



    public void Lure()
    {
        movingSpeed = originalMovingSpeed;
        rotatingSpeed = originalRotatingSpeed;
        float distanceToTarget = ((Vector2)transform.position - (Vector2)targetTurret.transform.position).magnitude;
        if (distanceToTarget < minDistanceToLure)
        {
            movingDirection = targetTurret.transform.position - transform.position + new Vector3(0.5f, 0.5f);
            MovingToTarget(Destination.Random);
        }
        else
        {
            movingDirection = targetTurret.transform.position - transform.position;
            MovingToTarget(Destination.Random);
        }

    }

    public virtual void Attack()
    {

    }

    public void ProtectMe()
    {
        fsm.PerformTransition(Transition.ProtectBoss);
    }

    public void Protect()
    {
        movingSpeed = originalMovingSpeed;
        rotatingSpeed = originalRotatingSpeed;
        float distanceToTarget = ((Vector2)transform.position - (Vector2)boss.transform.position).magnitude;
        if (distanceToTarget < minDistanceToLure)
        {
            movingDirection = boss.model.transform.position - transform.position + new Vector3(0.5f, 0.5f);
        }
        else
        {
            movingDirection = boss.model.transform.position - transform.position;
        }
        MovingToTarget(Destination.Random);

    }

    public void SearchTarget()
    {
        int hits = Physics2D.OverlapCircleNonAlloc(transform.position,
     exploreRange, attachedResult, LayerMask.GetMask(StaticData.TurretMask));
        if (hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                if (attachedResult[i].GetComponent<ConcreteContent>().Activated)
                {
                    turrets.Add(attachedResult[i].GetComponent<ConcreteContent>());
                }
            }

            if (turrets.Count > 0)
            {
                int temp = Random.Range(0, turrets.Count);
                targetTurret = turrets[temp];
                turrets.Clear();
            }
        }
    }


}
