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
        GUILayout.Label("��Ǯ");
        moneyGet = EditorGUILayout.TextField("", moneyGet, GUILayout.Width(80));
        if (GUILayout.Button("��ȡ", GUILayout.Width(120)))
        {
            GameManager.Instance.GainMoney(int.Parse(moneyGet));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Ԫ��");
        element = (ElementType)EditorGUILayout.EnumPopup("", element, GUILayout.Width(60));
        GUILayout.Label("Ʒ��");
        quality = (Quality)EditorGUILayout.EnumPopup("", quality, GUILayout.Width(60));
        if (GUILayout.Button("��ȡ", GUILayout.Width(120)))
        {
            ConstructHelper.GetElementTurretByQualityAndElement(element, (int)quality + 1);
        }
        GUILayout.EndHorizontal();

        turretName = EditorGUILayout.TextField("�ϳ���", turretName);
        GUILayout.BeginHorizontal();
        //GUILayout.Label("Ԫ��1");
        //e1 = EditorGUILayout.TextField("", e1);

        e1 = (ElementType)EditorGUILayout.EnumPopup("", e1, GUILayout.Width(60));
        //GUILayout.Label("Ԫ��2");
        //e2 = EditorGUILayout.TextField("", e2);

        e2 = (ElementType)EditorGUILayout.EnumPopup("", e2, GUILayout.Width(60));
        //GUILayout.Label("Ԫ��3");
        //e3 = EditorGUILayout.TextField("", e3);

        e3 = (ElementType)EditorGUILayout.EnumPopup("", e3, GUILayout.Width(60));
        if (GUILayout.Button("��ȡ", GUILayout.Width(120)))
        {
            ConstructHelper.GetRefactorTurretByNameAndElement(turretName, (int)e1, (int)e2, (int)e3);
           // ConstructHelper.GetRefactorTurretByNameAndElement(turretName,  int.Parse(e1), int.Parse(e2), int.Parse(e3));

        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        trapName = EditorGUILayout.TextField("����", trapName);
        if (GUILayout.Button("��ȡ", GUILayout.Width(120)))
        {
            ConstructHelper.GetTrapShapeByName(trapName);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        buildingName = EditorGUILayout.TextField("����", buildingName);
        if (GUILayout.Button("��ȡ", GUILayout.Width(120)))
        {
            //ConstructHelper.GetRefactorTurretByNameAndElement(buildingName, int.Parse(e1), int.Parse(e2), int.Parse(e3));

            ConstructHelper.GetRefactorTurretByNameAndElement(buildingName, (int)e1, (int)e2, (int)e3);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("����Ƽ�", GUILayout.Width(120)))
        {
            GameRes.AbnormalRate = 1f;

            GameManager.Instance.ShowTechSelect(true);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        techName = EditorGUILayout.TextField("�Ƽ���", techName);
        GUILayout.Label("�쳣");
        abnormal = (Abnormal)EditorGUILayout.EnumPopup("", abnormal, GUILayout.Width(60));
        if (GUILayout.Button("��ȡ�Ƽ�", GUILayout.Width(120)))
        {
            Technology tech = TechnologyFactory.GetBattleTech((int)Enum.Parse(typeof(TechnologyName),techName));
            tech.IsAbnormal = abnormal == Abnormal.True;
            GameManager.Instance.GetTech(tech);

        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("��ȡ���ܲ�оƬ", GUILayout.Width(120)))
        {
            GameRes.SkillChip++;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        achName = EditorGUILayout.TextField("�ɾ���", achName);
        GUILayout.Label("��ȡ���");
        abnormal = (Abnormal)EditorGUILayout.EnumPopup("", abnormal, GUILayout.Width(60));
        if (GUILayout.Button("���óɾ�", GUILayout.Width(120)))
        {
            AchievementManager.Instance.GetAchievement(achName, abnormal == Abnormal.True);
        }
        GUILayout.EndHorizontal();
    }
}
