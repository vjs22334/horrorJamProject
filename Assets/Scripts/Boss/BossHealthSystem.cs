using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthSystem : EnemyHealthSystem
{
    //TODO: pass current health for phase change check
    public delegate void OnBossHealthChanged(int currentHealth);
    public OnBossHealthChanged onBossHealthChanged;

    public override void Death()
    {
        GetComponent<Animator>().SetTrigger("Die");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<BossEnemy>().laser.SetActive(false);
        GetComponent<BossEnemy>().enabled = false;
    }

    public void DestroyThis(){
        GetComponent<Enemy>().EnemyKilled();
        Destroy(gameObject);
    }

    //overridden for delegate call
    public override void OnHealthChanged(int health)
    {
        base.OnHealthChanged(health);

        onBossHealthChanged(GetCurrentHealth());
    }
}
