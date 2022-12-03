using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoTips : TileTips
{
    [SerializeField] List<EnemyGrid> grids = default;

    public void ReadEnemyAtt(List<EnemyAttribute> atts)
    {
        for (int i = 0; i < grids.Count; i++)
        {
            if (atts.Count > i)
            {
                grids[i].SetEnemyInfo(atts[i]);
                grids[i].gameObject.SetActive(true);
            }
            else
            {
                grids[i].gameObject.SetActive(false);
            }

        }
    }
}
