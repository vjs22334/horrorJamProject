using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthSystem : EnemyHealthSystem
{
    public override void Death()
    {
        GetComponent<Animator>().SetTrigger("Die");
        GetComponent<BossEnemy>().laser.SetActive(false);
        GetComponent<BossEnemy>().enabled = false;
    }

    public void DestroyThis(){
        Destroy(gameObject);
    }
}
