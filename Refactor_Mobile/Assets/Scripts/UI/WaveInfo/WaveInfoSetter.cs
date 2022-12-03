
using System.Collections.Generic;
using UnityEngine;


public class WaveInfoSetter : MonoBehaviour
{
    [SerializeField] WaveInfoHolder waveHolder = default;
    [SerializeField] BossInfoHolder bossHolder = default;

    public void SetWaveInfo(List<EnemySequence> sequences,EnemyType nextBoss,int nextBossWave)
    {
        waveHolder.SetWaveInfo(sequences);
        bossHolder.SetBossInfo(nextBoss,nextBossWave);
    }


}
