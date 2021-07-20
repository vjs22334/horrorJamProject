using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu]
public class BossPhase : ScriptableObject
{
    public List<BossAttack> phaseAttacks;

    public BossPhase nextPhase;

    public int phaseChangePercent;

    public void OnEnter(BossEnemy boss)
    {
        foreach (BossAttack attack in phaseAttacks)
        {
            attack.boss = boss;
        }
    }

    public void OnEnd()
    {
        if (phaseChangePercent < 0)
        {
            Debug.Log("Boss ded!");
        }
    }
}
