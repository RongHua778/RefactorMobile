using System;
using UnityEditor;
using UnityEngine;


public enum Quality
{
    level1, Level2, level3, Level4, level5
}

enum Abnormal
{
    True,
    False
}

public class TestWindow : EditorWindow
{
    string moneyGet = "10000";
    //string element = "0";
    Quality quality = Quality.level1;

    public ElementType element = ElementType.GOLD;

    string turretName = "CONSTRUCTOR";
    ElementType e1 = ElementType.GOLD;
    ElementType e2 = ElementType.GOLD;
    ElementType e3 = ElementType.GOLD;
    //string e1 = "";
    //string e2 = "";
    //string e3 = "";



    Abnormal abnormal = Abnormal.False;

    string trapName = "BLINKTRAP";
    string buildingName = "VAULT";
    string techName = "";


    string achName = "";

    [MenuItem("Window/TestWindow")]
    public static void ShowWindow()
    {
        GetWindow<TestWindow>("TestWindow");
    }

    private void OnGUI()
    {

        GUILayout.BeginHorizontal();
        GUILayout.Label("金钱");
        moneyGet = EditorGUILayout.TextField("", moneyGet, GUILayout.Width(80));
        if (GUILayout.Button("获取", GUILayout.Width(120)))
        {
            GameManager.Instance.GainMoney(int.Parse(moneyGet));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("元素");
        element = (ElementType)EditorGUILayout.EnumPopup("", element, GUILayout.Width(60));
        GUILayout.Label("品质");
        quality = (Quality)EditorGUILayout.EnumPopup("", quality, GUILayout.Width(60));
        if (GUILayout.Button("获取", GUILayout.Width(120)))
        {
            ConstructHelper.GetElementTurretByQualityAndElement(element, (int)quality + 1);
        }
        GUILayout.EndHorizontal();

        turretName = EditorGUILayout.TextField("合成塔", turretName);
        GUILayout.BeginHorizontal();
        //GUILayout.Label("元素1");
        //e1 = EditorGUILayout.TextField("", e1);

        e1 = (ElementType)EditorGUILayout.EnumPopup("", e1, GUILayout.Width(60));
        //GUILayout.Label("元素2");
        //e2 = EditorGUILayout.TextField("", e2);

        e2 = (ElementType)EditorGUILayout.EnumPopup("", e2, GUILayout.Width(60));
        //GUILayout.Label("元素3");
        //e3 = EditorGUILayout.TextField("", e3);

        e3 = (ElementType)EditorGUILayout.EnumPopup("", e3, GUILayout.Width(60));
        if (GUILayout.Button("获取", GUILayout.Width(120)))
        {
            ConstructHelper.GetRefactorTurretByNameAndElement(turretName, (int)e1, (int)e2, (int)e3);
           // ConstructHelper.GetRefactorTurretByNameAndElement(turretName,  int.Parse(e1), int.Parse(e2), int.Parse(e3));

        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        trapName = EditorGUILayout.TextField("陷阱", trapName);
        if (GUILayout.Button("获取", GUILayout.Width(120)))
        {
            ConstructHelper.GetTrapShapeByName(trapName);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        buildingName = EditorGUILayout.TextField("建筑", buildingName);
        if (GUILayout.Button("获取", GUILayout.Width(120)))
        {
            //ConstructHelper.GetRefactorTurretByNameAndElement(buildingName, int.Parse(e1), int.Parse(e2), int.Parse(e3));

            ConstructHelper.GetRefactorTurretByNameAndElement(buildingName, (int)e1, (int)e2, (int)e3);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("随机科技", GUILayout.Width(120)))
        {
            GameRes.AbnormalRate = 1f;

            GameManager.Instance.ShowTechSelect(true);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        techName = EditorGUILayout.TextField("科技名", techName);
        GUILayout.Label("异常");
        abnormal = (Abnormal)EditorGUILayout.EnumPopup("", abnormal, GUILayout.Width(60));
        if (GUILayout.Button("获取科技", GUILayout.Width(120)))
        {
            Technology tech = TechnologyFactory.GetBattleTech((int)Enum.Parse(typeof(TechnologyName),techName));
            tech.IsAbnormal = abnormal == Abnormal.True;
            GameManager.Instance.GetTech(tech);

        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("获取技能槽芯片", GUILayout.Width(120)))
        {
            GameRes.SkillChip++;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        achName = EditorGUILayout.TextField("成就名", achName);
        GUILayout.Label("获取与否");
        abnormal = (Abnormal)EditorGUILayout.EnumPopup("", abnormal, GUILayout.Width(60));
        if (GUILayout.Button("设置成就", GUILayout.Width(120)))
        {
            AchievementManager.Instance.GetAchievement(achName, abnormal == Abnormal.True);
        }
        GUILayout.EndHorizontal();
    }
}
