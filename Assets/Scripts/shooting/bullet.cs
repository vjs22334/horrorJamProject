using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject,lifeTime);        
    }

    void Update()
    {
        transform.position += transform.right*bulletSpeed*Time.deltaTime;
    }
}
