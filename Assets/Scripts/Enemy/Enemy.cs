using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action OnEnemyKilled;
    public float moveMentSpeed = 0;
    public float minDistanceFromTarget = 0;
    public float maxAttackDistanceFromTarget = 0;

    public LayerMask lineOfSightLayerMask;
    public float lineOfSightLength;

    public GameObject DeathPs;

    [Header("references")]
    public Transform Target;
    public NavMeshAgent navMeshAgent;
    public Transform SpriteTransform;

    protected Vector2 vectorToTarget;
    protected bool playerInLineOfSight;


    void Start()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.speed = moveMentSpeed;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public virtual void Update()
    {
        // if there is no target do nothing
        if(Target == null){
            return;
        }
        vectorToTarget = Target.position - transform.position;
        playerInLineOfSight = GetPlayerInLineOfSight();
        //if player is too far away move towards him 
        if(vectorToTarget.magnitude > maxAttackDistanceFromTarget || !playerInLineOfSight){
            navMeshAgent.SetDestination(Target.position);
        }
        // if player is in attack range stop and attack
        else if(vectorToTarget.magnitude >= minDistanceFromTarget && vectorToTarget.magnitude <= maxAttackDistanceFromTarget && playerInLineOfSight){
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            Attack();
        }
        //If player is too close move away from him
        else if(vectorToTarget.magnitude < minDistanceFromTarget && playerInLineOfSight){
            Avoid();
        }
        enemyUpdate();
    }

    protected bool GetPlayerInLineOfSight(){
        RaycastHit2D raycastHit1,raycastHit2;
        Debug.DrawRay(transform.position+(Vector3)Vector2.Perpendicular(vectorToTarget.normalized).normalized*0.1f,vectorToTarget.normalized*lineOfSightLength,Color.green);
        Debug.DrawRay(transform.position-(Vector3)Vector2.Perpendicular(vectorToTarget.normalized).normalized*0.1f,vectorToTarget.normalized*lineOfSightLength,Color.green);
        raycastHit1 = Physics2D.Raycast(transform.position+(Vector3)Vector2.Perpendicular(vectorToTarget.normalized).normalized*0.1f,vectorToTarget.normalized,lineOfSightLength,lineOfSightLayerMask);
        raycastHit2 = Physics2D.Raycast(transform.position-(Vector3)Vector2.Perpendicular(vectorToTarget.normalized).normalized*0.1f,vectorToTarget.normalized,lineOfSightLength,lineOfSightLayerMask);
        if(raycastHit1 && raycastHit1.collider.CompareTag("Player") && raycastHit2 && raycastHit2.collider.CompareTag("Player")){
            return true;
        }
        return false;
    }

    public virtual void Attack(){

    }

    public virtual void Avoid(){
        Vector3 avoidPos = Target.position + (Vector3)vectorToTarget.normalized*-1*minDistanceFromTarget;
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.SetDestination(avoidPos);
    }

    public virtual void enemyUpdate(){
        //make the sprite face the player
        if(vectorToTarget.x < 0){
            SpriteTransform.localScale = new Vector3(-1,1,1);
        }
        else{
            SpriteTransform.localScale = new Vector3(1,1,1);
        }
    }

    public virtual void Initialize(Transform playerTransform){
        Target = playerTransform;
    }

    public void EnemyKilled(){
        if(DeathPs!=null)
            Instantiate(DeathPs,transform.position,Quaternion.identity);
        
        if(OnEnemyKilled!=null)
            OnEnemyKilled();
    }
    
   
}
