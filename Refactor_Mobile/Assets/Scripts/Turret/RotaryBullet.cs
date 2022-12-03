using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaryBullet : GroundBullet
{
    protected override float SplashBaseValue => turretParent.Strategy.FinalRange;

}
