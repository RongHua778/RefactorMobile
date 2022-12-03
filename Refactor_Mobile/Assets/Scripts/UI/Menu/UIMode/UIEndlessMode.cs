using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;

public class UIEndlessMode : MonoBehaviour
{
    [SerializeField] Text endlessUnlockText = default;
    [SerializeField] GameObject mainArea = default;

    [SerializeField] EndlessCustom endlessCustom = default;
    [SerializeField] EndlessWeekly endlessWeekly = default;



    public void Initialize()
    {
        endlessUnlockText.gameObject.SetActive(LevelManager.Instance.PassDiifcutly <= 4);
        mainArea.SetActive(LevelManager.Instance.PassDiifcutly > 4);

        endlessCustom.Initialize();
        endlessWeekly.Initialize();

        endlessCustom.gameObject.SetActive(true);
        endlessWeekly.gameObject.SetActive(false);

    }




}
