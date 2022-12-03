using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySequence
{
    public EnemyType EnemyType;
    public float Intensify;
    public int Amount;
    public float CoolDown;
    public bool IsBoss;
    public int Wave;
    private float SpawnTimer;
    private int count;

    public float DmgResist;
    public EnemySequence()
    {
    }

    public EnemySequence(int wave,EnemyType type, int amount, float cooldown, float intensify, bool isBoss,float dmgResit)
    {
        this.Wave = wave;
        EnemyType = type;
        Amount = amount;
        CoolDown = cooldown;
        Intensify = intensify;
        SpawnTimer = 0;
        this.IsBoss = isBoss;
        count = Amount;
        this.DmgResist = dmgResit;
    }



    public bool first = true;
    float coolDown;

    public void Initialize()
    {
        count = this.Amount;
    }
    public bool Progress()
    {
        SpawnTimer += Time.deltaTime;
        if (count > 0)
        {
            if (first)//第一只出怪冷却为0
            {
                first = false;
                coolDown = 0;
            }
            else
            {
                coolDown = CoolDown;

            }
            if (SpawnTimer >= coolDown)
            {
                SpawnTimer = 0;
                count--;
                GameManager.Instance.SpawnEnemy(EnemyType, 0, Intensify, DmgResist, BoardSystem.shortestPoints);
            }
        }
        else
        {
            first = true;
            return false;
        }
        return true;


    }
}
