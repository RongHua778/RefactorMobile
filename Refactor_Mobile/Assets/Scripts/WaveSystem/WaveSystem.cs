using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Sirenix.OdinInspector;

public class WaveSystem : IGameSystem
{
    [Header("测试用")]
    [SerializeField] float testIntensify = default;
    [SerializeField] EnemyType TestType = EnemyType.Knight;
    [SerializeField] int TestEnemyCount = default;
    [SerializeField] float TestCoolDown = default;

    [Button]
    private void GenerateWave()
    {
        RunningSequence = TestSequenceSet();
    }

    [Space]
    public bool RunningSpawn = false;//是否正在生产敌人

    [ShowInInspector]
    public static List<List<EnemySequence>> LevelSequence = new List<List<EnemySequence>>();
    LevelAttribute LevelAttribute;


    private List<EnemySequence> runningSequence;
    public List<EnemySequence> RunningSequence { get => runningSequence; set => runningSequence = value; }

    [SerializeField] BossComeAnim bossWarningUIAnim = default;
    [SerializeField] GoldKeeperAnim goldKeeperUIAnim = default;

    public EnemyType NextBoss;
    public int NextBossWave;

    public override void Initialize()
    {
        LevelAttribute = LevelManager.Instance.CurrentLevel;
        GameEvents.Instance.onEnemyReach += EnemyReach;
        GameEvents.Instance.onEnemyDie += EnemyDie;
        bossWarningUIAnim.Initialize();
        goldKeeperUIAnim.Initialize();
    }

    public override void Release()
    {
        base.Release();
        GameEvents.Instance.onEnemyReach -= EnemyReach;
        GameEvents.Instance.onEnemyDie -= EnemyDie;
    }

    private void EnemyReach(Enemy enemy)
    {
        GameRes.LostLifeBattleTurn += Mathf.Min(GameRes.Life - 1, enemy.ReachDamage);
        GameRes.Life -= enemy.ReachDamage;//必须先减少LIFE判失败
        GameRes.EnemyRemain--;
    }

    private void EnemyDie(Enemy enemy)
    {
        GameRes.EnemyRemain--;
    }

    public override void GameUpdate()
    {
        if (RunningSpawn)
        {
            for (int i = 0; i < RunningSequence.Count; i++)
            {
                if (RunningSequence[i].Progress())
                {
                    continue;
                }
                RunningSpawn = false;
                GameRes.EnemyRemain = GameRes.EnemyRemain;
            }
        }
    }

    public void LoadSaveWave()
    {
        LevelSequence.Clear();
        foreach (var saveStruct in LevelManager.Instance.LastGameSave.SaveSequences)
        {
            LevelSequence.Add(saveStruct.SequencesList);
        }
    }

    public void LoadChallengeWave()
    {
        LevelSequence.Clear();
        foreach (var saveStruct in LevelManager.Instance.CurrentLevel.SaveSequences)
        {
            LevelSequence.Add(saveStruct.SequencesList);
        }
    }

    public void LevelInitialize()
    {
        LevelSequence.Clear();
        float stage = 1;
        int index = 0;
        float dmgResist = 0f;
        int overWave = 0;
        float num = 0f;
        List<EnemySequence> sequences = null;
        for (int i = 0; i < LevelAttribute.Wave; i++)
        {
            for (int g = 0; g < LevelAttribute.WaveSets.Length; g++)
            {
                if (i < LevelAttribute.WaveSets[g].waveIndex)
                {
                    index = g;
                    break;
                }
            }

            if (i <= LevelAttribute.WaveSets[LevelAttribute.WaveSets.Length - 1].waveIndex)
            {
                stage += LevelAttribute.LevelIntensify *
                (LevelAttribute.WaveSets[index].baseNum * Mathf.Pow(i, LevelAttribute.WaveSets[index].powerNum) + 1);
            }
            else
            {
                overWave = i - LevelAttribute.WaveSets[LevelAttribute.WaveSets.Length - 1].waveIndex;
                num = Mathf.Pow(overWave, 1f + overWave / 25f);
                dmgResist = num / (num + 4f);
            }

            //if (LevelAttribute.ModeType == ModeType.Challenge)//挑战模式
            //{
            //    sequences = GenerateRandomSequence(LevelAttribute.Boss1, 1, stage, i, dmgResist);
            //}
            //else
            //{
            if (i < 3)
            {
                stage = (i + 1) * 0.5f;

                sequences = GenerateRandomSequence(LevelAttribute.NormalEnemies, 1, stage, i, dmgResist);
            }
            else if (i == 9)
            {
                sequences = GenerateRandomSequence(LevelAttribute.Boss1, 1, stage, i, dmgResist);
            }
            else if (i == 19)
            {
                sequences = GenerateRandomSequence(LevelAttribute.Boss2, 1, stage, i, dmgResist);
            }
            else if (i == 29)
            {
                sequences = GenerateRandomSequence(LevelAttribute.Boss3, 1, stage, i, dmgResist);
            }
            else if (i > 29 && i < 49 && (i + 1) % 5 == 0)//无尽模式30波后每5波一个BOSS
            {
                sequences = GenerateRandomSequence(LevelAttribute.Boss3, 1, stage, i, dmgResist);
            }
            else if (i >= 49 && i < LevelAttribute.WaveSets[LevelAttribute.WaveSets.Length - 1].waveIndex && (i + 1) % 5 == 0)//无尽模式49波后每5波一个超级BOSS
            {
                sequences = GenerateRandomSequence(LevelAttribute.Boss4, 1, stage, i, dmgResist);
            }
            else if (i >= LevelAttribute.WaveSets[LevelAttribute.WaveSets.Length - 1].waveIndex)//减伤波开始后，每波都刷高级BOSS
            {
                sequences = GenerateRandomSequence(LevelAttribute.Boss4, 1, stage, i, dmgResist);
            }
            else if ((i + 4) % 10 == 0 && i < 99)//每7，17，27波
            {
                sequences = GenerateSpecificSequence(EnemyType.GoldKeeper, stage, i, false, dmgResist);
            }
            else if (((i + 10) % 10 == 0 || (i + 9) % 10 == 0) && i > 9)//每11，16波二混
            {
                sequences = GenerateRandomSequence(i > LevelAttribute.EliteWave ? LevelAttribute.EliteEnemies : LevelAttribute.NormalEnemies, 2, stage, i, dmgResist);
            }
            else if (((i + 8) % 10 == 0 || (i + 3) % 10 == 0) && i > 9)//每13，18波三混
            {
                sequences = GenerateRandomSequence(i > LevelAttribute.EliteWave ? LevelAttribute.EliteEnemies : LevelAttribute.NormalEnemies, 3, stage, i, dmgResist);
            }
            else
            {
                sequences = GenerateRandomSequence(i > LevelAttribute.EliteWave ? LevelAttribute.EliteEnemies : LevelAttribute.NormalEnemies, 1, stage, i, dmgResist);
            }
            //}
            LevelSequence.Add(sequences);
        }
    }



    private List<EnemySequence> GenerateRandomSequence(List<EnemyAttribute> enemyList, int genres, float stage, int wave, float dmgResist)
    {
        List<EnemySequence> sequencesToReturn = new List<EnemySequence>();
        List<int> indexs = StaticData.SelectNoRepeat(enemyList.Count, genres);
        foreach (var index in indexs)
        {
            EnemySequence sequence = SequenceInfoSet(genres, stage, wave, enemyList[index].EnemyType, enemyList[index].IsBoss, dmgResist);
            sequencesToReturn.Add(sequence);
        }
        return sequencesToReturn;
    }

    private List<EnemySequence> GenerateSpecificSequence(EnemyType type, float stage, int wave, bool isBoss, float dmgResist)
    {
        List<EnemySequence> sequencesToReturn = new List<EnemySequence>();
        EnemySequence sequence = SequenceInfoSet(1, stage, wave, type, isBoss, dmgResist);
        sequencesToReturn.Add(sequence);
        return sequencesToReturn;
    }

    private EnemySequence SequenceInfoSet(int genres, float intensify, int wave, EnemyType type, bool isBoss, float dmgResist)
    {
        EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(type);
        int amount = Mathf.CeilToInt(Mathf.Min(attribute.MaxAmount / genres, Mathf.RoundToInt(attribute.InitCount + ((float)wave / 5) * attribute.CountIncrease / genres)) * GameRes.EnemyAmoundAdjust);
        float coolDown = attribute.CoolDown * genres;
        intensify = intensify * GameRes.EnemyIntensifyAdjust;
        EnemySequence sequence = new EnemySequence(wave, type, amount, coolDown, intensify, isBoss, dmgResist);
        return sequence;
    }

    private List<EnemySequence> TestSequenceSet()
    {
        List<EnemySequence> sequencesToReturn = new List<EnemySequence>();

        EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(TestType);
        EnemySequence sequence = new EnemySequence(1, TestType, TestEnemyCount, TestCoolDown, testIntensify, attribute.IsBoss, 0);
        sequencesToReturn.Add(sequence);
        return sequencesToReturn;
    }

    public void ManualSetSequence(EnemyType type, float stage, int wave)
    {
        LevelSequence[wave] = GenerateSpecificSequence(type, stage, wave, false, 0);
        PrepareNextWave();
    }

    public void PrepareNextWave()
    {
        RunningSequence = LevelSequence[GameRes.CurrentWave - 1];
        foreach (var sequence in RunningSequence)
        {
            sequence.Initialize();
        }
        if (RunningSequence[0].IsBoss)
        {
            //EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(RunningSequence[0].EnemyType);
            //attribute.isLock = false;//见到就解锁BOSS
            bossWarningUIAnim.Show();
        }
        else if (RunningSequence[0].EnemyType == EnemyType.GoldKeeper)
        {
            goldKeeperUIAnim.Show();
        }
        //下个BOSS预告
        for (int i = GameRes.CurrentWave - 1; i < LevelSequence.Count; i++)
        {
            if (LevelSequence[i][0].IsBoss)
            {
                NextBoss = LevelSequence[i][0].EnemyType;
                NextBossWave = i - GameRes.CurrentWave + 1;
                break;
            }
        }
    }


    public Enemy SpawnEnemy(EnemyType eType, int pathIndex, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        EnemyAttribute att = StaticData.Instance.EnemyFactory.Get(eType);
        GameRes.EnemyRemain++;
        Enemy enemy = ObjectPool.Instance.Spawn(att.Prefab) as Enemy;
        enemy.Initialize(pathIndex, att, UnityEngine.Random.Range(-0.19f, 0.19f), intensify, dmgResist, pathPoints);
        GameManager.Instance.enemies.Add(enemy);
        return enemy;
    }


}
