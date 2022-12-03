using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirerStrategy : RefactorStrategy
{
    public override int FinalRange => InitRange;
    public FirerStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
    {
    }
}
