using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firer : RecipeRefactor
{
    [SerializeField] private FirerArea fireArea;
    [SerializeField] private ParticleSystem fireEffect;

    protected override void Shoot()
    {
        fireArea.Initialize(this,Target[0]);
    }

    public override void AddTarget(TargetPoint target)
    {
        base.AddTarget(target);
        fireEffect.Play();
    }

    public override void RemoveTarget(TargetPoint target)
    {
        base.RemoveTarget(target);
        if (targetList.Count <= 0)
            fireEffect.Stop();
    }

    public void SetFireAngle(float value)
    {
        fireArea.SetAngle(value);
    }

}
