using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangStrategy : RefactorStrategy
{
    public BoomerangStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
    {
    }

    public float DmgBonusWhileReturn { get; set; }

}
