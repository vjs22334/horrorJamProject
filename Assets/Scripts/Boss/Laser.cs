using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform laserStartPoint;
    public float laserRaycastDistance;
    public LayerMask laserLayermask;
    public GameObject LaserMidsprite;
    public float LaserMidspritelength;

    public int damage = 30;

    public bool firing = false;

    public bool followPlayer;
    Transform target;
  
    void OnEnable()
    {
        firing = false;

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
   
    void OnDisable()
    {
        foreach (Transform child in laserStartPoint)
        {
            Destroy(child.gameObject);
        }
    }

    public void LaserFire(){
        firing = true;
    }
    void Update()
    {
        if(!firing){
            return;
        }
        foreach (Transform child in laserStartPoint)
        {
            Destroy(child.gameObject);
        }

        transform.position = new Vector3(Vector3.zero.x, transform.position.y, transform.position.z);

        float laserLength;

        RaycastHit2D raycast = Physics2D.Raycast(laserStartPoint.position, laserStartPoint.right, laserRaycastDistance, laserLayermask);
        if (raycast) {
            if (followPlayer)
            {
                //get magnitude of distance to player, this magnitude is how long the ray should be at a given time
                laserLength = Mathf.Clamp(((Vector2)target.position - (Vector2)laserStartPoint.position).magnitude, 0, (raycast.point - (Vector2)laserStartPoint.position).magnitude);
            }
            else
                laserLength = (raycast.point - (Vector2)laserStartPoint.position).magnitude;

            int laserSpritesCount = Mathf.CeilToInt(laserLength / LaserMidspritelength);
            for(int i = 0; i < laserSpritesCount; i++) {
                Instantiate(LaserMidsprite, laserStartPoint.position + i * LaserMidspritelength * laserStartPoint.right, laserStartPoint.rotation, laserStartPoint);
            }

            if(raycast.collider.CompareTag("Player") && !raycast.collider.GetComponent<PlayerController>().Iframes) {
                HealthSystem healthSystem = raycast.collider.GetComponent<HealthSystem>();
                if (healthSystem != null) {
                    healthSystem.TakeDamage(damage);
                }
            }
        }   
    }
}
