using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EndlessParam
{
    public string Tips;
    public int Version;
    public List<RuleName> RuleNames;
    [AssetList(Path = "Resources/SO/RefactorAttribute/")]
    public List<TurretAttribute> Recipes;
}

[CreateAssetMenu(menuName = "Param/WeeklyParam_Endless", fileName = "WeeklyParam_Endless")]
public class WeeklyParam_Endless : ScriptableObject
{
    [ListDrawerSettings(NumberOfItemsPerPage = 5)]
    public List<EndlessParam> EndlessParams;


}
