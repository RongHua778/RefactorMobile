using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockBonusTips : IUserInterface
{
    private Animator anim;
    [SerializeField] ToggleGroup m_ToggleGroup = default;
    [SerializeField] Transform parentObj = default;
    [SerializeField] TurretItemSlot turretSlotPrefab = default;
    [SerializeField] TrapItemSlot trapSlotPrefab = default;
    private List<ItemSlot> m_SlotList = new List<ItemSlot>();
    public override void Initialize()
    {
        base.Initialize();
        anim = this.GetComponent<Animator>();
    }

    public override void Show()
    {
        base.Show();
        anim.SetBool("isOpen", true);
    }

    public void SetBouns(GameLevelInfo info)
    {
        foreach (var item in m_SlotList)
        {
            Destroy(item.gameObject);
        }
        m_SlotList.Clear();
        for (int i = 0; i < 3; i++)
        {

            if (i < info.UnlockItems.Length)
            {
                ItemSlot slot = null;
                switch (info.UnlockItems[i].AttType)
                {
                    case AttType.Turret:
                        slot = Instantiate(turretSlotPrefab, parentObj);
                        slot.SetContent(info.UnlockItems[i], m_ToggleGroup);
                        break;
                    case AttType.Mark:
                        slot = Instantiate(trapSlotPrefab, parentObj);
                        slot.SetContent(info.UnlockItems[i], m_ToggleGroup);
                        break;
                }
                m_SlotList.Add(slot);
            }
        }
    }

    public void CloseTips()
    {
        anim.SetBool("isOpen", false);
    }

}
