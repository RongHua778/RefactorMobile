using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
//using DG.Tweening;


[System.Serializable]
public struct TextSet
{
    public float size;
    public float time;
    public Color color;
}
public class JumpDamage : ReusableObject
{
    [SerializeField] TextMeshPro Text = default;

    private Vector2 randomOffset;
    float offset = 0.1f;
    //float size;
    //float time;
    //[SerializeField] Color normalColor = default;
    //[SerializeField] Color critColor = default;

    [SerializeField] TextSet normal = default;
    [SerializeField] TextSet crit = default;
    [SerializeField] private float jumpSpeed;
    private Vector2 startPos;
    private Vector2 endPos;
    float progress;

    private void Update()
    {
        progress += jumpSpeed * Time.deltaTime;
        if (progress <= 1)
            transform.position = Vector2.Lerp(startPos, endPos, progress);
        else if (progress >= 2f)
            RecycleObject();
    }

    private void SetValue(long amount, Vector2 pos, bool isCritical)
    {
        progress = 0;
        TextSet set = isCritical ? crit : normal;
        Text.color = set.color;
        transform.localScale = Vector2.one * Mathf.Clamp(set.size * (Mathf.Log10(amount) + 1), 0.1f, 5f);
        randomOffset = new Vector2(Random.Range(-offset, offset), Random.Range(-offset, offset));
        startPos = pos + randomOffset;
        endPos = startPos + Vector2.up * 0.5f;
        transform.position = startPos;
        Text.text = amount.ToString();
    }


    public void Jump(long amount, Vector2 pos, bool isCritical)
    {
        SetValue(amount, pos, isCritical);
        //StartCoroutine(JumpCor(amount, pos, isCritical));
    }

    //IEnumerator JumpCor(long amount, Vector2 pos, bool isCritical)
    //{
    //    TextSet set = isCritical ? crit : normal;
    //    Text.color = set.color;
    //    transform.localScale = Vector2.one * Mathf.Clamp(set.size * (Mathf.Log10(amount) + 1), 0.1f, 5f);
    //    randomOffset = new Vector2(Random.Range(-offset, offset), Random.Range(-offset, offset));
    //    transform.position = pos + randomOffset;
    //    Text.text = amount.ToString();
    //    transform.DOMove((Vector2)transform.position + Vector2.up * 0.5f, 0.25f);
    //    yield return new WaitForSeconds(set.time);
    //    RecycleObject();
    //}

    public void RecycleObject()
    {
        ObjectPool.Instance.UnSpawn(this);
    }

}
