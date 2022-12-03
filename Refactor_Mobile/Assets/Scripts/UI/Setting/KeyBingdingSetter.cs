using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;


public class KeyBingdingSetter : MonoBehaviour
{
    [SerializeField] KeyBindingActions m_KeyAction = default;
    [SerializeField] Text keyTxt = default;
    [SerializeField] Toggle m_Toggle = default;

    bool isCheckingInput = false;

    private readonly KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
                                             .Cast<KeyCode>()
                                             .Where(k => ((int)k < (int)KeyCode.Mouse0))
                                             .ToArray();

    private void GetCurrentKeyDown()
    {
        if (!Input.anyKeyDown)
        {
            return;
        }

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                if (InputManager.Instance.SetKeyBinding(m_KeyAction, keyCodes[i]))
                {
                    SetContent();
                }
                m_Toggle.isOn = false;
                break;
            }
        }

    }



    public void SetContent()
    {
        keyTxt.text = InputManager.Instance.GetKeyForAction(m_KeyAction).ToString();
    }

    public void ClickSetToggle(bool value)
    {
        isCheckingInput = value;
    }


    private void Update()
    {
        if (isCheckingInput)
            GetCurrentKeyDown();
    }

}
