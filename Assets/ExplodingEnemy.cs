using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    public GameObject explosionPrefab;

    public override void Attack()
    {
        Instantiate(explosionPrefab,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
