using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
//using UnityEditor;

[System.Serializable]
public class WaveSet
{
    public int waveIndex;
    public float baseNum;
    public float powerNum;
}
[System.Serializable]
public class TileInfo
{
    public Vector2 PathPoint;
    public ContentStruct ContentStruct;
}

public enum ModeType
{
    Standard,
    Endless,
    Challenge
}

[CreateAssetMenu(menuName = "Attribute/LevelAttribute", fileName = "LevelAttribute")]
public class LevelAttribute : ScriptableObject
{
    public ModeType ModeType;
    public int ModeID;
    public int Level;
    //public ChallengeModeAttribute ChallengeAtt;

    public int StartCoin;
    public int PlayerHealth;
    public int Wave;
    public float LevelIntensify;
    public WaveSet[] WaveSets;

    public List<EnemyAttribute> NormalEnemies;
    public List<EnemyAttribute> EliteEnemies;
    public List<EnemyAttribute> Boss1;
    public List<EnemyAttribute> Boss2;
    public List<EnemyAttribute> Boss3;
    public List<EnemyAttribute> Boss4;

    public string LevelInfo;
    public int TrapCount;
    public int EliteWave;
    public float ExpIntensify;

    [Title("预先生成")]
    public List<ContentStruct> PreTiles;
    [Title("挑战模式配置")]
    public List<ChallengeChoice> Choices;
    public List<EnemySequenceStruct> SaveSequences;

    [Title("教程配置")]
    public DialogueData[] GuideDialogues = default;
    public bool CanSaveGame;

    [Title("小姐姐对白")]
    public DialogueData WinDialogue;
    public DialogueData LostDialogue;
    public DialogueData[] WaveDialogue;
    public EnemyAttribute GetRandomBoss(int level)
    {
        switch (level)
        {
            case 1:
                return Boss1[Random.Range(0, Boss1.Count)];
            case 2:
                return Boss2[Random.Range(0, Boss2.Count)];
            case 3:
                return Boss3[Random.Range(0, Boss3.Count)];
            case 4:
                return Boss4[Random.Range(0, Boss4.Count)];
        }
        Debug.LogWarning("没有可以返回的BOSS类型");
        return null;
    }

    [Button("GenerateSequence")]
    private void GeneateSequence()
    {

        SaveSequences = new List<EnemySequenceStruct>();
        foreach (var sequences in WaveSystem.LevelSequence)
        {
            EnemySequenceStruct eStruct = new EnemySequenceStruct();
            eStruct.SequencesList = sequences;
            SaveSequences.Add(eStruct);
        }
    }
    [Button("SaveCurrentContent")]
    private void SaveCurrentPath()
    {
        PreTiles.Clear();
        PreTiles = LevelManager.Instance.SaveContens();
    }

    [Button("GenerateRandomChoice")]
    private void GenerateRandomChoice()
    {
        ChallengeChoice challengeChoice = new ChallengeChoice();
        challengeChoice.Choices = new List<Choice>();
        ChallengeChoiceType choiceType = Random.value > 0.75f ? ChallengeChoiceType.Trap : ChallengeChoiceType.Turret;

        switch (choiceType)
        {
            case ChallengeChoiceType.Turret:
                List<int> turretNameIndexs = StaticData.SelectNoRepeat(StaticData.Instance.ContentFactory.RefactorTurretNames.Count, 3);
                for (int i = 0; i < 3; i++)
                {
                    Choice choice = new Choice();
                    choice.ChoiceType = choiceType;
                    choice.Value1 = StaticData.Instance.ContentFactory.RefactorTurretNames[turretNameIndexs[i]];
                    choice.Elements = null;

                    //剔除幻化构造
                    while (choice.Elements == null ||
                        (choice.Elements.Contains(0) && choice.Elements.Contains(2) && choice.Elements.Contains(4))
                        )
                    {
                        choice.Elements = new List<int>();
                        for (int m = 0; m < 3; m++)
                        {
                            int element = (int)StaticData.Instance.ContentFactory.GetRandomElementAttribute().element;
                            choice.Elements.Add(element);
                        }
                    }
                    challengeChoice.Choices.Add(choice);
                }
                break;

            case ChallengeChoiceType.Trap:

                List<int> trapNameIndexs = StaticData.SelectNoRepeat(StaticData.Instance.ContentFactory.TrapNames.Count, 3);

                for (int i = 0; i < 3; i++)
                {
                    Choice choice = new Choice();
                    choice.ChoiceType = choiceType;
                    choice.Value1 = StaticData.Instance.ContentFactory.TrapNames[trapNameIndexs[i]];
                    //剔除铸币陷阱
                    challengeChoice.Choices.Add(choice);
                }
                break;
        }
        Choices.Add(challengeChoice);

    }
}
