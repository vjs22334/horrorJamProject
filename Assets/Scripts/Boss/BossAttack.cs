using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossAttack : ScriptableObject
{
    [HideInInspector]
    public BossEnemy boss;
    public virtual void OnEnter()
    {
        boss.animator.ResetTrigger("idle");
    }

    public virtual void OnExit(){}

    public virtual void Tick(Vector3 vectorToTarget){}

    public float minPlayerRange;

    public float MaxPlayerRange;

    public bool followPlayer;

    public float stoppingDistance;

}
