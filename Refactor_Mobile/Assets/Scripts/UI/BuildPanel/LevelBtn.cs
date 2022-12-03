using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] LevelInfoPanel infoPanel = default;
    [SerializeField] ParticleSystem LevelUpPartical = default;
    //[SerializeField] ReusableObject perfectAnim = default;
    private void Start()
    {
        infoPanel.Initialize();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.Hide();
    }

    public void LevelBtnClick()
    {
        if (GameRes.SystemLevel < StaticData.Instance.SystemMaxLevel)
        {
            if (GameManager.Instance.ConsumeMoney(GameRes.SystemUpgradeCost))
            {
                GameRes.SystemLevel++;
                if (GameRes.SystemLevel == 2 || GameRes.SystemLevel == 4 || GameRes.SystemLevel == 6)//2，4,6级增加一个商店容量
                {
                    GameRes.ShopCapacity++;
                    //GameManager.Instance.GainPerfectElement(1);
                    //ReusableObject anim = ObjectPool.Instance.Spawn(perfectAnim);
                    //anim.transform.parent = LevelUpPartical.transform;
                    //anim.transform.position = LevelUpPartical.transform.position + Vector3.up*0.1f;
                }
                LevelUpPartical.Play();
                Sound.Instance.PlayUISound("LevelUp");
                infoPanel.SetInfo();
                GameEvents.Instance.TutorialTrigger(TutorialType.SystemBtnClick);
            }
        }
    }
}
