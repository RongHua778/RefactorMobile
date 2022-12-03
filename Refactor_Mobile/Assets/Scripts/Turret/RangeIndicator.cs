using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : ReusableObject
{
    [SerializeField] SpriteRenderer sprite = default;
    public void ShowSprite(bool show)
    {
        sprite.enabled = show;
    }

}
