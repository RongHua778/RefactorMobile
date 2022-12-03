using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteContent : GameTileContent, IGameBehavior
{
    public override bool IsWalkable => false;
    public StrategyBase Strategy;
    public bool Dropped { get; set; }

    public bool Activated;
    private RangeHolder RangeHolder;
    private float frostTime = 0;
    private FrostEffect frostEffect;
    private bool isShowingRange;

    protected Transform rotTrans;
    protected Transform shootPoint;
    protected SpriteRenderer CannonSprite;

    public List<TargetPoint> targetList = new List<TargetPoint>();
    private List<TargetPoint> target = new List<TargetPoint>();
    public List<TargetPoint> Target { get => target; set => target = value; }

    public BuffableTurret Buffable { get; set; }

    public bool IsAttacking => targetList.Count > 0 && Activated;
    protected virtual void Awake()
    {
        RangeHolder = Instantiate(StaticData.Instance.RangeHolder, transform);
        rotTrans = transform.Find("RotPoint");
        shootPoint = rotTrans.Find("ShootPoint");
        CannonSprite = rotTrans.Find("Cannon").GetComponent<SpriteRenderer>();
        Buffable = this.GetComponent<BuffableTurret>();
    }
    public virtual bool GameUpdate()
    {
        OnActivating();
        SkillUpdate();
        Buffable.TimeTick();
        return true;
    }

    protected virtual void SkillUpdate()
    {
        foreach (var skill in Strategy.TurretSkills)
        {
            if (!skill.IsFinish)
                skill.Tick(Time.deltaTime);
        }
        foreach (var skill in Strategy.GlobalSkills)
        {
            if (!skill.IsFinish)
                skill.Tick(Time.deltaTime);
        }
    }

    public void ShowRange(bool show)
    {
        isShowingRange = show;
        GameManager.Instance.ShowConcreteRange(this, show);
    }
    public virtual void GenerateRange()
    {
        RangeHolder.SetRange();
        if (RangeContainer.ShowingConcrete == this)
            ShowRange(isShowingRange);
    }

    protected virtual void OnActivating()
    {
        if (frostTime > 0)
        {
            frostTime -= Time.deltaTime;
            if (frostTime <= 0)
            {
                UnFrost();
            }
        }
    }

    public void UnFrost()
    {
        Activated = true;
        if (frostEffect != null)
        {
            frostEffect.Broke();
            frostEffect = null;
        }
        frostTime = 0;
    }

    public virtual void Frost(float time, FrostEffect effect = null)
    {
        if (Strategy == null)
        {
            Debug.LogWarning("Strategy=null问题出现");
            return;
        }
        Activated = false;
        frostTime = Mathf.Max(0.2f, time * (1 - Strategy.FinalFrostResist - GameRes.TurretFrostResist));
        if (frostEffect != null)
            frostEffect.Broke();
        frostEffect = effect;

        foreach (var skill in Strategy.TurretSkills)//冻结时触发效果
        {
            skill.Frost();
        }
        foreach (var skill in Strategy.GlobalSkills)//冻结时触发效果
        {
            skill.Frost();
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (Dropped)
            StaticData.SetNodeWalkable(m_GameTile, false, true);
        Target.Clear();
        targetList.Clear();
        Dropped = false;
        Strategy = null;
        frostTime = 0;
        if (frostEffect != null)
        {
            frostEffect.Broke();
            frostEffect = null;
        }

        //ShowRange(false);
    }

    public virtual void TurnClear()
    {
        if (frostEffect != null)
        {
            frostEffect.Broke();
            frostTime = 0;
            frostEffect = null;
            Activated = true;
        }
        Buffable.ClearBuffs();
        Target.Clear();
        targetList.Clear();
    }

    public override void OnContentSelected(bool value)
    {
        base.OnContentSelected(value);
        ShowRange(value);
    }

    public override void ContentLanded()
    {
        base.ContentLanded();
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);

        Dropped = true;
        StaticData.SetNodeWalkable(m_GameTile, false, false);
        GameManager.Instance.TriggerDetectSkills();//任何一个塔放下来，都要所有防御塔检测一次侦测效果
        //GameManager.Instance.CheckDetectSkill();//任何一个塔放下来，都要所有防御塔检测一次侦测效果
    }

    protected override void ContentLandedCheck(Collider2D col)
    {
        base.ContentLandedCheck(col);
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            ObjectPool.Instance.UnSpawn(tile);
        }
    }



    public virtual void AddTarget(TargetPoint target)
    {
        if (target.gameObject.activeInHierarchy)
            targetList.Add(target);
        Strategy.EnterSkill(target.Enemy);
    }

    public virtual void RemoveTarget(TargetPoint target)
    {
        if (targetList.Contains(target))
        {
            if (this.Target.Contains(target))
            {
                this.Target.Remove(target);
            }
            targetList.Remove(target);
            Strategy.ExitSkill(target.Enemy);
        }
    }

    public virtual void SetGraphic()
    {

    }


}
