using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShortCutsSetting : MonoBehaviour
{
    [SerializeField] KeyBingdingSetter[] m_Setters = default;

    public void Initialize()
    {
        SetAllContents();
    }
    private void SetAllContents()
    {
        foreach (var setter in m_Setters)
        {
            setter.SetContent();
        }
    }

    public void ResetAllKey()
    {
        InputManager.Instance.ResetAllKeys();
        SetAllContents();
    }



}
