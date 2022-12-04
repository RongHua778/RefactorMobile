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
        if (LevelManager.Instance.PassDiifcutly < 6)
        {
            endlessUnlockText.gameObject.SetActive(true);
            mainArea.SetActive(false);
        }
        else
        {
            endlessUnlockText.gameObject.SetActive(false);
            mainArea.SetActive(true);

            endlessCustom.Initialize();
            endlessWeekly.Initialize();

            endlessCustom.gameObject.SetActive(true);
            endlessWeekly.gameObject.SetActive(false);
        }


    }




}
