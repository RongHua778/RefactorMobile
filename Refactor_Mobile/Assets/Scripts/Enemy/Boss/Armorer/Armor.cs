using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour, IDamage
{
    public string ExplosionSound => "Sound_EnemyExplosion";
    [SerializeField] private ReusableObject ExplosionPrefab = default;
    [SerializeField] private ParticalControl SputteringEffect = default;
    [SerializeField] private SpriteRenderer ArmorSprite = default;
    [SerializeField] private Collider2D ArmorCol = default;
    [SerializeField] private HealthBar_Sprie m_HealthBar = default;

    public HealthBar_Sprie HealthBar { get => m_HealthBar; set => m_HealthBar = value; }
    public DamageStrategy DamageStrategy { get; set; }
    public SpriteRenderer gfxSprite { get => ArmorSprite; set => ArmorSprite = value; }
    public Collider2D TargetCollider { get => ArmorCol; set => ArmorCol = value; }

    public Enemy EnemyParent { get; set; }
    private ArmorHolder armorHolder;

    public void Initialize(Enemy enemyParent, float maxHealth, ArmorHolder arHolder)
    {
        this.EnemyParent = enemyParent;
        this.armorHolder = arHolder;
        DamageStrategy = new ArmourStrategy(this, EnemyParent.DmgResist);
        DamageStrategy.MaxHealth = maxHealth;
        DamageStrategy.IsDie = false;
    }


    public virtual void DisArmor()
    {
        transform.localScale = Vector3.zero;
        ReusableObject explosion = ObjectPool.Instance.Spawn(ExplosionPrefab);
        explosion.transform.position = transform.position;
        Sound.Instance.PlayEffect(ExplosionSound);

        armorHolder.RemoveArmor(1);
    }

    public virtual void ReArmor()
    {
        transform.localScale = Vector3.one;
        DamageStrategy.CurrentHealth = DamageStrategy.MaxHealth;
    }

    Bullet bullet;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        bullet = collision.GetComponent<Bullet>();
        bullet.DealRealDamage(bullet.FinalDamage, this, transform.position, true);
        GameManager.Instance.nonEnemies.Remove(bullet);
        bullet.ReclaimBullet();

        ParticalControl effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
        effect.transform.position = transform.position;
        effect.transform.localScale = Vector3.one * 0.3f;
        effect.PlayEffect();

    }


}
