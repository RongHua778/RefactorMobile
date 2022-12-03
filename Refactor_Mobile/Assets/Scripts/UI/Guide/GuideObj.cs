using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideObj : MonoBehaviour
{
    [SerializeField] GameObject[] guideObjs = default;
    private void Awake()
    {
        GameEvents.Instance.onGuideObjCollect += CollectThisObj;
    }

    private void CollectThisObj()
    {
        for (int i = 0; i < guideObjs.Length; i++)
        {
            GuideGirlSystem.Instance.AddGuideObj(guideObjs[i]);
        }
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onGuideObjCollect -= CollectThisObj;

    }
}
