using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MySingleton<InputManager>
{
    [SerializeField] Keybindings KeyBindings = default;
    public KeyCode GetKeyForAction(KeyBindingActions keybindingAction)
    {
        foreach (var keybindingCheck in KeyBindings.keybindingChecks)
        {
            if (keybindingCheck.keybingdingAction == keybindingAction)
            {
                return keybindingCheck.keyCode;
            }
        }
        return KeyCode.None;
    }

    public bool GetKeyDown(KeyBindingActions key)
    {
        foreach (var keybindingCheck in KeyBindings.keybindingChecks)
        {
            if (keybindingCheck.keybingdingAction == key)
            {
                return Input.GetKeyDown(keybindingCheck.keyCode);
            }
        }
        return false;
    }

    public bool GetKeyUp(KeyBindingActions key)
    {
        foreach (var keybindingCheck in KeyBindings.keybindingChecks)
        {
            if (keybindingCheck.keybingdingAction == key)
            {
                return Input.GetKeyUp(keybindingCheck.keyCode);
            }
        }
        return false;
    }

    public bool GetKey(KeyBindingActions key)
    {
        foreach (var keybindingCheck in KeyBindings.keybindingChecks)
        {
            if (keybindingCheck.keybingdingAction == key)
            {
                return Input.GetKey(keybindingCheck.keyCode);
            }
        }
        return false;
    }

    public bool SetKeyBinding(KeyBindingActions action, KeyCode keycode)
    {
        foreach (var item in KeyBindings.keybindingChecks)
        {
            if (item.keyCode == keycode)
            {
                TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("SAMEKEY"));
                return false;
            }
        }
        foreach (var item in KeyBindings.keybindingChecks)
        {
            if (item.keybingdingAction == action)
            {
                item.keyCode = keycode;
                return true;
            }
        }
        return true;
    }

    public void ResetAllKeys()
    {
        foreach (var item in KeyBindings.keybindingChecks)
        {
            item.keyCode = item.defaultKeycode;
        }
    }

}
