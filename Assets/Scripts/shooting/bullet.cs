using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float lifeTime = 5f;

    public int damage = 20;

    public bool damagesPlayer = false;
    public bool damagesEnemy = false;

    public GameObject BulletHitPs;

    void Start()
    {
        Destroy(gameObject,lifeTime);        
    }

    void Update()
    {
        transform.position += transform.right*bulletSpeed*Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if((other.CompareTag("Player") && damagesPlayer && !other.GetComponent<PlayerController>().Iframes)){
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if(healthSystem!=null){
                healthSystem.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if((other.CompareTag("enemy") && damagesEnemy)){
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if(healthSystem!=null){
                healthSystem.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if(other.CompareTag("wall")){
            Destroy(gameObject);
        }
        
    }

    
    void OnDestroy()
    {
        Instantiate(BulletHitPs,transform.position,Quaternion.identity);
    }
}
