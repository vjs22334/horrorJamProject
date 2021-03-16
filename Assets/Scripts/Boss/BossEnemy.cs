using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public List<BossAttack> bossAttacks;

    public Gun gun;
    public Animator animator;

    public GameObject laser;

    BossAttack currAttack;

    bool initialized = false;

    void Awake()
    {
        foreach (BossAttack atatck in bossAttacks)
        {
            atatck.boss = this;
        }
    }


    public void Initialize(){
        ChooseAttack();
        initialized = true;
    }


    public void ChooseAttack(){
        List<BossAttack> possibleAttacks = new List<BossAttack>();
        foreach (BossAttack attack in bossAttacks)
        {
            if(vectorToTarget.magnitude <= attack.MaxPlayerRange && vectorToTarget.magnitude >= attack.minPlayerRange){
                possibleAttacks.Add(attack);
            }
        }

        int i = Random.Range(0,possibleAttacks.Count);

        if(currAttack!=null){
            currAttack.OnExit();
        }
        currAttack = possibleAttacks[i];

        currAttack.OnEnter();

    }

    public override void Update()
    {
        if(Target == null){
            return;
        }
        if(!initialized){
            return;
        }
        
        vectorToTarget = Target.position - transform.position;

        if(currAttack.followPlayer && vectorToTarget.magnitude > currAttack.stoppingDistance){
            navMeshAgent.SetDestination(Target.position);
        }
        else if(currAttack.followPlayer && vectorToTarget.magnitude < currAttack.stoppingDistance){
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }
        
        enemyUpdate();

        currAttack.Tick(vectorToTarget);
       
    }

    public void LaserAttack(){
        laser.GetComponent<Laser>().LaserFire();
    }



}
