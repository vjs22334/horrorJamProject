using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;

    //called by animation event
    public void ExplosionComplete(){
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if(healthSystem!=null){
                healthSystem.TakeDamage(damage);
            }
            Destroy(this.GetComponent<Collider2D>());
        }
    }
}
