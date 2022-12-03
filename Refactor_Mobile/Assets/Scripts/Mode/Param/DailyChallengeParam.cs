using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Param/DaylyChallengeParam", fileName = "DaylyChallengeParam")]
public class DailyChallengeParam : ScriptableObject
{
    public List<LevelAttribute> ChallengeLevels;
}
