using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : Enemy
{
    public Gun gun;

    public override void Attack()
    {
        if(gun.canFire){
            Vector3 aimDirection = vectorToTarget.normalized;
            float angle = Mathf.Atan2(aimDirection.y,aimDirection.x)*Mathf.Rad2Deg;
            gun.transform.eulerAngles = new Vector3(0,0,angle);
            gun.Fire();
        }
        else if(!gun.Reloading){
            gun.Reload();
        }
        
           
    }
}
