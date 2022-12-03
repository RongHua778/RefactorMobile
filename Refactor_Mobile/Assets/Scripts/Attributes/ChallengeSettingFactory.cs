using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChallengeChoiceType
{
    Turret,
    Trap,
    Technology
}

[System.Serializable]
public class ChallengeChoice
{
    public List<Choice> Choices;
}
[System.Serializable]
public struct Choice
{
    public ChallengeChoiceType ChoiceType;
    public string Value1;
    public List<int> Elements;
}

[CreateAssetMenu(menuName = "Attribute/ChallengeSetting", fileName = "ChallengeSetting")]
public class ChallengeSettingFactory : ScriptableObject
{
    public List<ChallengeChoice> WhiteChoices;//level=0
    public List<ChallengeChoice> BlueChoices;
    public List<ChallengeChoice> PurpleChoices;
    public List<ChallengeChoice> GoldChoices;//level=3

    public ChallengeChoice GetRandomChoice(int level)
    {
        switch (level)
        {
            case 0:
                return WhiteChoices[Random.Range(0, WhiteChoices.Count)];
            case 1:
                return BlueChoices[Random.Range(0, BlueChoices.Count)];
            case 2:
                return PurpleChoices[Random.Range(0, PurpleChoices.Count)];
            case 3:
                return GoldChoices[Random.Range(0, GoldChoices.Count)];
        }
        return default;
    }

    public List<ChallengeChoice> GetRandomChoices(int level, int amount)
    {
        List<ChallengeChoice> returnChoices = new List<ChallengeChoice>();

        List<ChallengeChoice> targetChoices = null;
        int maxAmount = 0;
        switch (level)
        {
            case 0:
                targetChoices = WhiteChoices;
                maxAmount = WhiteChoices.Count;
                break;
            case 1:
                targetChoices = BlueChoices;
                maxAmount = BlueChoices.Count;
                break;
            case 2:
                targetChoices = PurpleChoices;
                maxAmount = PurpleChoices.Count;
                break;
            case 3:
                targetChoices = GoldChoices;
                maxAmount = GoldChoices.Count;
                break;
        }
        List<int> selectedID = StaticData.SelectNoRepeat(maxAmount, amount);
        for (int i = 0; i < selectedID.Count; i++)
        {
            returnChoices.Add(targetChoices[selectedID[i]]);
        }
        return returnChoices;
    }

}
