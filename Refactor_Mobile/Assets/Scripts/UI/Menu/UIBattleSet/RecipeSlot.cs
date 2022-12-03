using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class RecipeSlot : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    Toggle m_Toggle;
    Animator m_Anim;
    [SerializeField] Color disableColor = default;

    [SerializeField] Image turretIcon = default;
    [SerializeField] Text turretName = default;
    [SerializeField] Image lockIcon = default;

    public bool IsSelected => m_Toggle.isOn;
    public TurretAttribute m_Att;
    public void Initialize(TurretAttribute att, ToggleGroup group)
    {
        m_Anim = this.GetComponent<Animator>();
        m_Att = att;
        m_Toggle = this.GetComponent<Toggle>();
        //m_Toggle.group = group;

        //OnSelect(isOn);
        SetAtt(att);
        m_Toggle.interactable = !m_Att.isLock;//未解锁的无法选择

    }


    private void SetAtt(TurretAttribute att)
    {
        lockIcon.gameObject.SetActive(att.isLock);
        turretName.gameObject.SetActive(!att.isLock);
        turretIcon.sprite = att.Icon;
        turretName.text = GameMultiLang.GetTraduction(att.Name);
    }

    public void OnSelect(bool value)
    {
        m_Toggle.isOn = value;
        turretIcon.color = value ? Color.white : disableColor;
        m_Anim.SetBool("Selected", value);
        //Sound.Instance.PlayUISound("Sound_Click");
        //if (value)
        //    Sound.Instance.PlayUISound("Sound_Click");
    }




}
