using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Attribute/ChallengeModeAttribute", fileName = "ChallengeModeAttribute")]
public class ChallengeModeAttribute : ScriptableObject
{
    public int[] WaveRequired;
    public List<Vector2> Paths;
    public List<ChallengeChoice> Choices;

    [Button("SaveCurrentPath")]
    private void SaveCurrentPath()
    {
        Paths.Clear();
        foreach (var pathPoint in BoardSystem.shortestPoints)
        {
            Paths.Add(pathPoint.PathPos);
        }
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

                    //ÌÞ³ý»Ã»¯¹¹Ôì
                    while (choice.Elements == null ||
                        (choice.Elements[0] == 0 && choice.Elements[1] == 2 && choice.Elements[2] == 4)
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
                    //ÌÞ³ýÖý±ÒÏÝÚå
                    challengeChoice.Choices.Add(choice);
                }
                break;
        }
        Choices.Add(challengeChoice);

    }
}
