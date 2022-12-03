using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour
{
    [SerializeField] protected ContentAttribute contenAtt = default;
    [SerializeField] Image icon = default;
    [SerializeField] Text nameTxt = default;
    [SerializeField] Color normalColor = default;
    [SerializeField] Color lockColor = default;
    [SerializeField] GameObject lockIcon = default;

    protected bool isLock = false;
    private Toggle m_Toggle;

    public virtual void SetContent(ContentAttribute attribute,ToggleGroup group)
    {
        m_Toggle = this.GetComponent<Toggle>();
        m_Toggle.group = group;

        contenAtt = attribute;

        isLock = contenAtt.isLock;
        lockIcon.SetActive(isLock);
        icon.sprite = contenAtt.Icon;
        nameTxt.text = isLock ? "" : GameMultiLang.GetTraduction(contenAtt.Name);
        icon.color = isLock ? lockColor : Color.white;
        nameTxt.color = isLock ? Color.gray : normalColor;

    }

    public virtual void OnItemSelect(bool value)
    {
        Sound.Instance.PlayEffect("Sound_Click");
    }
}
