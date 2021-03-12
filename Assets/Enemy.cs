using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

        
    public float moveMentSpeed = 0;
    public float minDistanceFromTarget = 0;
    public float maxAttackDistanceFromTarget = 0;

    public LayerMask lineOfSightLayerMask;
    public float lineOfSightLength;

    [Header("references")]
    public Transform Target;
    public NavMeshAgent navMeshAgent;

    Vector3 vectorToTarget;


    void Start()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.speed = moveMentSpeed;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        vectorToTarget = Target.position - transform.position;
        if(vectorToTarget.magnitude > maxAttackDistanceFromTarget){
            navMeshAgent.SetDestination(Target.position);
        }
        else if(vectorToTarget.magnitude >= minDistanceFromTarget && vectorToTarget.magnitude <= minDistanceFromTarget){
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }
        else if(vectorToTarget.magnitude < minDistanceFromTarget){
            Vector3 avoidPos = Target.position + vectorToTarget.normalized*-1*minDistanceFromTarget;
            NavMeshPath path = new NavMeshPath();
            navMeshAgent.SetDestination(avoidPos);
        }
    }
}
