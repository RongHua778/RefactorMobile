using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeSystem : IGameSystem
{
    private Queue<ChallengeChoice> challengeChoices;

    public int MaxChoice;
    public int ChoiceRemained => challengeChoices.Count;
    public override void Initialize()
    {
        base.Initialize();
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge)
        {
            challengeChoices = new Queue<ChallengeChoice>();
            for (int i = 0; i < LevelManager.Instance.CurrentLevel.Choices.Count; i++)
            {
                challengeChoices.Enqueue(LevelManager.Instance.CurrentLevel.Choices[i]);
            }
            MaxChoice = challengeChoices.Count;
        }

    }

    public void LoadSaveGame()
    {
        int forbidCount = LevelManager.Instance.LastGameSave.ChallengeChoicePicked ? GameRes.CurrentWave : GameRes.CurrentWave - 1;
        for (int i = 0; i < forbidCount; i++)
        {
            challengeChoices.Dequeue();
        }
    }

    public ChallengeChoice GetCurrentChoice()
    {
        if (challengeChoices.Count > 0)
        {
            return challengeChoices.Dequeue();
        }
        else
        {
            return null;
        }

    }
}
