using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_ChoiceElements : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI elementSkillName = default;
    [SerializeField] Image[] elements = default;

    public void SetElements(ElementSkill skill)
    {
        elementSkillName.text = skill.SkillName;

        for (int i = 0; i < skill.Elements.Count; i++)
        {
            elements[i].sprite = StaticData.Instance.ElementSprites[skill.Elements[i] % 10];
        }
    }

}
