using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public abstract class Enemy : PathFollower, IDamage
{
    protected override bool IsPathfollower => false;
    public virtual string ExplosionSound => "Sound_EnemyExplosion";
    [SerializeField] protected ReusableObject ExplosionPrefab;
    [Header("基本配置")]
    protected Animator anim;
    public HealthBar_Sprie HealthBar { get; set; }
    protected SpriteRenderer enemySprite;
    protected Collider2D enemyCol;
    protected EnemyAttribute m_Attribute;
    public DamageStrategy DamageStrategy { get; set; }
    public SpriteRenderer gfxSprite { get => enemySprite; set => enemySprite = value; }
    public Collider2D TargetCollider { get => enemyCol; set => enemyCol = value; }

    //寻路及陷阱触发
    protected List<BasicTile> pathTiles;
    private BasicTile currentTile;
    public BasicTile CurrentTile
    {
        get => currentTile;
        set
        {
            currentTile = value;
            currentTile.OnTilePass(this);
            Buffable.TileTick();
        }
    }

    //状态配置
    [ReadOnly]
    public float Intensify;
    [ReadOnly]
    public float DmgResist;
    protected bool isOutTroing = false;//正在消失
    protected bool trapTriggered = false;
    public bool IsEnemy { get => true; }
    public virtual EnemyType EnemyType { get; }

    //经过的陷阱列表
    private List<TrapContent> passedTraps = new List<TrapContent>();
    public List<TrapContent> PassedTraps { get => passedTraps; set => passedTraps = value; }

    public int ReachDamage { get; set; }


    private int affectHealerCount = 0;
    public int AffectHealerCount { get => affectHealerCount; set => affectHealerCount = value; }
    float speedIntensify = 0;
    public virtual float SpeedIntensify
    {
        get => speedIntensify + (AffectHealerCount > 0 ? 0.6f : 0);
        set
        {
            speedIntensify = value;
            ProgressFactor = Speed * Adjust;
        }
    }
    public override float Speed { get => (speed + SpeedIntensify) * GameRes.TurnSpeedAdjust * SpeedAdjust; }

    private float speedAdjust = 1;
    public float SpeedAdjust
    {
        get => speedAdjust;
        set
        {
            speedAdjust = value;
            ProgressFactor = Speed * Adjust;//子弹减速即时更新速度
        }

    }
    //public override float Speed { get => (Mathf.Max(0.3f, ((speed + SpeedIntensify) * (1 - SlowRate / (SlowRate + SlowResist))) * GameRes.EnemySpeedAdjust)); }
    public override float ProgressFactor { get => DamageStrategy.IsControlled ? 0 : base.ProgressFactor; set => base.ProgressFactor = value; }



    public BuffableEnemy Buffable { get; set; }

    protected EnemyAttribute m_Att;

    public virtual int MaxAmount => 200;//同屏最大该类敌人上限

    public ArmorHolder HoldingArmor { get; set; }
    public virtual void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        m_Att = attribute;
        DmgResist = dmgResist;
        this.pathTiles = BoardSystem.shortestPath;
        this.PathOffset = pathOffset;
        this.Intensify = intensify;
        this.DamageStrategy.ResetStrategy(attribute, intensify, dmgResist);//清除加成
        this.speed = attribute.Speed;
        this.ReachDamage = attribute.ReachDamage;

        foreach (var buffInfo in EnemyBuffFactory.GlobalBuffs)//施加全局BUFF
        {
            this.DamageStrategy.ApplyBuff(buffInfo);
        }

        SpawnOn(pathIndex, pathPoints);

    }

    protected override void Awake()
    {
        base.Awake();
        SetStrategy();
        HealthBar = model.GetComponentInChildren<HealthBar_Sprie>();
        enemySprite = model.Find("GFX").GetComponent<SpriteRenderer>();

        Buffable = this.GetComponent<BuffableEnemy>();
        enemyCol = enemySprite.GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

    }

    protected virtual void SetStrategy()
    {
        DamageStrategy = new BasicEnemyStrategy(this);
    }
    public override bool GameUpdate()
    {
        if (isOutTroing)
        {
            return true;
        }
        OnEnemyUpdate();
        Progress += Time.deltaTime * ProgressFactor;

        if (Progress >= 0.5f && !trapTriggered)
        {
            trapTriggered = true;
            CurrentTile = pathTiles[PointIndex];
        }

        if (Progress >= 1f)
        {

            if (PointIndex == PathPoints.Count - 1)
            {
                isOutTroing = true;
                anim.SetTrigger("Exit");
                DamageStrategy.GFXFade(false);

                return true;
            }
            trapTriggered = false;
            Progress = 0;
            PrepareNextState();
        }


        if (DirectionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, Progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, Progress);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        return true;
    }

    protected virtual void OnEnemyUpdate()
    {
        DamageStrategy.StrategyUpdate();
        Buffable.TimeTick();
    }
    public virtual void OnDie()
    {
        ReusableObject explosion = ObjectPool.Instance.Spawn(ExplosionPrefab);
        explosion.transform.position = model.position;
        Sound.Instance.PlayEffect(ExplosionSound);
        GameEvents.Instance.EnemyDie(this);
        ObjectPool.Instance.UnSpawn(this);
    }


    public virtual void EnemyExit()
    {
        if (!DamageStrategy.IsDie)
        {
            ((BasicEnemyStrategy)DamageStrategy).UnFrost();
            GameEvents.Instance.EnemyReach(this);
            ObjectPool.Instance.UnSpawn(this);
        }
    }

    protected override void PrepareIntro()
    {
        base.PrepareIntro();
        CurrentTile = pathTiles[PointIndex];
        anim.Play("Default");
        anim.SetTrigger("Enter");

        gfxSprite.color = new Color(1, 1, 1, 0);
        DamageStrategy.GFXFade(true);
    }


    public void Flash(int distance)
    {
        PointIndex -= distance;
        if (PointIndex < 0)
        {
            PointIndex = 0;
            Progress = 0f;
        }
        else if (PointIndex >= PathPoints.Count - 1)
        {
            PointIndex = PathPoints.Count - 1;
            Progress = 1f;
        }
        CurrentPoint = PathPoints[PointIndex];
        CurrentTile = pathTiles[PointIndex];
        trapTriggered = true;

        transform.localPosition = PathPoints[PointIndex].PathPos;
        PositionFrom = CurrentPoint.PathPos;
        PositionTo = CurrentPoint.ExitPoint;
        Direction = CurrentPoint.PathDirection;
        DirectionChange = DirectionChange.None;
        model.localPosition = new Vector3(PathOffset, 0);
        DirectionAngleFrom = DirectionAngleTo = Direction.GetAngle();
        transform.localRotation = CurrentPoint.PathDirection.GetRotation();

        Adjust = 2f;
        ProgressFactor = Adjust * Speed;


        anim.Play("Default");
        anim.SetTrigger("Enter");

        gfxSprite.color = new Color(1, 1, 1, 0);
        DamageStrategy.GFXFade(true);

    }

    public void Blink(int blinkDistance)
    {
        StartCoroutine(BlinkCor(blinkDistance));
    }
    private IEnumerator BlinkCor(int blinkDistance)
    {
        DamageStrategy.StunTime += 0.5f;
        model.DOScale(Vector3.zero, 0.25f);
        SpawnHoleOnPos(model.position);
        yield return new WaitForSeconds(0.25f);
        model.DOScale(Vector3.one, 0.25f);
        Vector3 targetPos;
        targetPos = PathPoints[Mathf.Min(PointIndex + blinkDistance, PathPoints.Count - 1)].PathPos;
        SpawnHoleOnPos(targetPos);
        Flash(-blinkDistance);
        if (((BasicEnemyStrategy)DamageStrategy).m_FrostEffect != null)
        {
            ((BasicEnemyStrategy)DamageStrategy).m_FrostEffect.transform.position = model.position;
        }
        yield return new WaitForSeconds(0.25f);
    }

    private void SpawnHoleOnPos(Vector3 pos)
    {
        ReusableObject hole = ObjectPool.Instance.Spawn(StaticData.Instance.BlinkHolePrefab);
        hole.transform.position = pos;
        hole.transform.localScale = Vector3.one * 0.1f;
        hole.transform.DOScale(Vector3.one * 0.4f, 0.5f);
        hole.UnspawnAfterTime(0.5f);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();

        model.localScale = Vector3.one;
        if (HoldingArmor != null)
            Destroy(HoldingArmor.gameObject);

        PassedTraps.Clear();
        isOutTroing = false;
        SpeedIntensify = 0;
        AffectHealerCount = 0;
        SpeedAdjust = 1;
        Buffable.RemoveAllBuffs();
        GameManager.Instance.enemies.Remove(this);
    }

}
