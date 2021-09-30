using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    //callback based on the triggered anim event
    public static event Action<string> OnAnimationEvent = delegate { };
    
    //list of phases for boss
    //public List<BossPhase> bossPhases;

    public List<BossAttack> bossAttacks;

    //TODO: put launcher into the weapons array and make it work from there
    public Gun gun;
    public Gun launcher;
    public Animator animator;

    public GameObject[] weapons;

    //current phase of boss
    public BossPhase currPhase;

    //assigned in inspector
    public BossHealthSystem bossHealthSystem;

    bool isCurrentPhaseOver = false;

    BossAttack currAttack;

    bool initialized = false;

    void Awake()
    {
        //assign this component to each boss of each attack of each phase
        //foreach (BossPhase phase in bossPhases){
        //    foreach (BossAttack attack in phase.phaseAttacks){
        //        attack.boss = this;
        //    }
        //}
        
        //set current phase to the first phase in list
        //currPhase = bossPhases[0];

        //set list of boss attacks to the current phase
        SetAttacksListFromPhase(currPhase);

        //sub to BHS's onHealthChanged to trigger CheckForPhaseChange method
        bossHealthSystem.onBossHealthChanged += CheckForPhaseChange;

    }

    public void CheckForPhaseChange(int currentBosshealth)
    {
        //if boss's health is <= phaseChangePercent% of maxHealth
        if (currentBosshealth <= bossHealthSystem.MaxHealth * currPhase.phaseChangePercent / 100)
        {
            //ChoosePhase();
            isCurrentPhaseOver = true;
        }
    }

    public GameObject SearchForWeapon(string weaponName)
    {
        foreach (GameObject weapon in weapons)
        {
            if (weapon.name == weaponName)
            {
                return weapon;
            }
        }
        return null;
    }

    public void ActivateWeapons(bool b)
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(b);
        }
    }

    public void Initialize()
    {
        currPhase.OnEnter(this);
        ChooseAttack();
        initialized = true;
    }

    public void ChoosePhase()
    {
        if (currPhase.nextPhase != null)
        {
            if (currPhase.hasPhaseChangeAttack)
            {
                currAttack.OnExit();
                currAttack = currPhase.phaseChangeAttack;
                currPhase.phaseChangeAttack.OnEnter();
            }
            else
            {
                currPhase.OnEnd();
            }

            //is there some list based better alt for this?
            currPhase = currPhase.nextPhase;
            SetAttacksListFromPhase(currPhase);

            currPhase.OnEnter(this);
        }
        //else if currPhase is final phase
        //code goes here

        //else
        //{
        //    currPhase.OnEnd();
        //    bossHealthSystem.onBossHealthChanged -= CheckForPhaseChange;
        //}
    }

    public void ChooseAttack()
    {
        List<BossAttack> possibleAttacks = new List<BossAttack>();
        foreach (BossAttack attack in bossAttacks){
            if(vectorToTarget.magnitude <= attack.MaxPlayerRange && vectorToTarget.magnitude >= attack.minPlayerRange){
                possibleAttacks.Add(attack);
            }
        }

        int i = UnityEngine.Random.Range(0,possibleAttacks.Count);

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

        if (isCurrentPhaseOver)
        {
            ChoosePhase();
            isCurrentPhaseOver = false;
        }

        currAttack.Tick(vectorToTarget);
    }

    //public void LaserAttack()
    //{
    //    laser.GetComponent<Laser>().LaserFire();
    //}

    public void AnimationEventTrigger(string eventType)
    {
        OnAnimationEvent(eventType);
    }

    public void SetAttacksListFromPhase(BossPhase phase)
    {
        bossAttacks = phase.phaseAttacks;
    }
}
