using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorHolder : MonoBehaviour
{
    [SerializeField] Armor[] m_Armors = default;
    private int armorRemain = 0;
    private Enemy enemyParent;
    public void Initialize(Enemy enemy, float maxHealth)
    {
        this.enemyParent = enemy;
        for (int i = 0; i < m_Armors.Length; i++)
        {
            m_Armors[i].Initialize(enemy, maxHealth, this);
            armorRemain++;
        }
    }

    public void RemoveArmor(int value)
    {
        armorRemain -= value;
        //if (armorRemain <= 0)
        //{
        //    Destroy(this.gameObject);
        //}
    }

}
