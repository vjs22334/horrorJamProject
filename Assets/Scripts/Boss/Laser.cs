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

  
    void OnEnable()
    {
        firing = false;
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

        RaycastHit2D raycast = Physics2D.Raycast(laserStartPoint.position,laserStartPoint.right,laserRaycastDistance,laserLayermask);
        if(raycast){
            float laserLength = (raycast.point-(Vector2)laserStartPoint.position).magnitude;
            int laserSpritesCount = Mathf.CeilToInt(laserLength/LaserMidspritelength);
            for(int i=0;i<laserSpritesCount;i++){
                Instantiate(LaserMidsprite,laserStartPoint.position + i*LaserMidspritelength*laserStartPoint.right,laserStartPoint.rotation,laserStartPoint);
            }

            if(raycast.collider.CompareTag("Player") && !raycast.collider.GetComponent<PlayerController>().Iframes){
                HealthSystem healthSystem = raycast.collider.GetComponent<HealthSystem>();
                if(healthSystem!=null){
                    healthSystem.TakeDamage(damage);
                }
            }
        }   
    }
}
