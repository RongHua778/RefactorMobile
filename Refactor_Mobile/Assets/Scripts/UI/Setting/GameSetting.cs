using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    [SerializeField] Toggle showDmgTog = default;
    [SerializeField] Toggle showIntensifyTog = default;

    public void ShowSetting()
    {
        showDmgTog.isOn = StaticData.ShowDamage;
        showIntensifyTog.isOn = StaticData.ShowIntensify;
    }
    public void ShowJumpDamage(bool value)
    {
        StaticData.ShowDamage = value;
    }

    public void ShowIntensify(bool value)
    {
        StaticData.ShowIntensify = value;
    }
}
