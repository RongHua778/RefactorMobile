using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Armor,
    Frost,
    Restore
}

public abstract class Weapon : ReusableObject, IDamage, IGameBehavior
{
    // public abstract WeaponType WeaponType { get; set; }
    public string ExplosionSound => "Sound_EnemyExplosion";

    private const float speedUpDis = 1.5f;
    [SerializeField] private ReusableObject m_ExplosionPrefab = default;
    [SerializeField] private HealthBar_Sprie m_HealthBar = default;
    [SerializeField] private SpriteRenderer m_SpriteRenderer = default;
    [SerializeField] private Collider2D weaponCol = default;

    [SerializeField] private float healthPercent = default;
    [SerializeField] private float frost = default;
    [SerializeField] private float speed = default;


    public DamageStrategy DamageStrategy { get; set; }
    public Collider2D TargetCollider { get => weaponCol; set => weaponCol = value; }
    public SpriteRenderer gfxSprite { get => m_SpriteRenderer; set => m_SpriteRenderer = value; }
    public HealthBar_Sprie HealthBar { get => m_HealthBar; set => m_HealthBar = value; }

    private Knight m_Knight;
    public Knight Knight { get => m_Knight; }
    public float MoveSpeed => DamageStrategy.IsFrost ? 0f : speed;

    public float SpeedModify = 1;

    public virtual void Initiate(Knight knight)
    {
        m_Knight = knight;
        DamageStrategy = new WeaponStrategy(this, Knight.DmgResist, this.frost);
        DamageStrategy.MaxHealth = knight.DamageStrategy.MaxHealth * healthPercent;
        HealthBar.FrostAmount = 0;
        GameManager.Instance.nonEnemies.Add(this);
    }

    public bool GameUpdate()
    {
        if (DamageStrategy.IsDie || Knight.DamageStrategy.IsDie)
        {
            OnDie();
            return false;
        }
        DamageStrategy.StrategyUpdate();
        MoveToKnight();
        return true;
    }
    private void MoveToKnight()
    {
        float distance = Vector2.Distance(transform.position, m_Knight.gfxSprite.transform.position);
        float speedUp = 0f;

        if (distance < 0.1f)
        {
            TriggerWeapon();
            ObjectPool.Instance.UnSpawn(this);
            GameManager.Instance.nonEnemies.Remove(this);
            return;
        }
        else if (distance < speedUpDis)//½ü¾àÀë¼ÓËÙ
        {
            speedUp = 1f;
        }
        transform.position = Vector2.MoveTowards(transform.position,
            m_Knight.gfxSprite.transform.position, SpeedModify * (speedUp + MoveSpeed) * Time.deltaTime);

    }

    protected virtual void TriggerWeapon()
    {
        Knight.weaponInScene--;
    }

    private void OnDie()
    {
        Knight.weaponInScene--;
        ReusableObject explosion = ObjectPool.Instance.Spawn(m_ExplosionPrefab);
        explosion.transform.position = transform.position;
        Sound.Instance.PlayEffect(ExplosionSound);
        ObjectPool.Instance.UnSpawn(this);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        DamageStrategy.UnFrost();

    }


}
