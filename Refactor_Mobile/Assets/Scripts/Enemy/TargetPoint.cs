using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Vector2 Position => transform.position;

    public IDamage Enemy { get; set; }

    private void Awake()
    {
        SetEnemy();
    }

    protected virtual void SetEnemy()
    {
        Enemy = transform.root.GetComponent<IDamage>();

    }

}
