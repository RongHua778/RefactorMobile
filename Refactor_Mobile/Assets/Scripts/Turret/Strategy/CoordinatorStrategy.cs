using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinatorStrategy : RefactorStrategy
{
    public CoordinatorStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
    {
    }
}
