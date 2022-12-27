using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AchievementUnlockTips : MonoBehaviour
{
    [SerializeField] private AchievementGrid achievementGrid = default;
    [SerializeField] private Transform gridParent = default;
    public void UnlockAchievement(Achievement ach)
    {
        StartCoroutine(UnlockAch(ach));
    }

    IEnumerator UnlockAch(Achievement ach)
    {
        AchievementGrid grid = Instantiate(achievementGrid, gridParent);
        grid.SetAch(ach);
        grid.transform.localScale = new Vector3(1, 0, 1);
        grid.transform.DOScaleY(1f, 0.5f);
        yield return new WaitForSeconds(4f);
        grid.transform.DOScaleY(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(grid.gameObject);
    }
}
