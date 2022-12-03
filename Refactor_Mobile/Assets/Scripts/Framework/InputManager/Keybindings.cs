using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="Keybingdings",menuName ="Keybindings")]
public class Keybindings : ScriptableObject
{
    [Serializable]
    public class KeybingdingCheck
    {
        public KeyBindingActions keybingdingAction;
        public KeyCode keyCode;
        public KeyCode defaultKeycode;
    }

    public KeybingdingCheck[] keybindingChecks;


}
