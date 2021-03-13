using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gun : MonoBehaviour
{
    [Header("references")]
    public Transform muzzleTransform;
    public ShotPattern shotPattern;

    [Header("gun data")]
    [Tooltip("time between shots in seconds")]
    public float fireRate = 1f;
    public int clipSize = 5;
    public float reloadTime = 2f;

    int bulletsInclip;
    float fireDelayTimer;
    float reloadTimer;
    bool reloading;
    public bool canFire{
        get{
            return bulletsInclip > 0 && fireDelayTimer <=0 && !reloading;
        }
    }

    public int BulletsInClip{
        get{
            return bulletsInclip;
        }
    }

    public bool Reloading{
        get{
            return reloading;
        }
    }

    void Awake()
    {
        fireDelayTimer = 0;
        reloadTimer = 0;
        bulletsInclip = clipSize;
        reloading = false;
    }

    void Update()
    {
        if(reloadTime > 0 && reloading){
            reloadTime -= Time.deltaTime;
        }else if(reloading){
            bulletsInclip = clipSize;
            reloadTime = 0;
            reloading = false;
        }

        if(fireDelayTimer > 0){
            fireDelayTimer -= Time.deltaTime;
        }else{
            fireDelayTimer = 0;
        }

    }


    public void Fire()
    {
        if(canFire){
            foreach (float angle in shotPattern.spreadAngles)
            {
                //TODO: Pool this
                GameObject bulletGo = Instantiate(shotPattern.bulletPrefab,muzzleTransform.position,Quaternion.Euler(0,0,angle)*muzzleTransform.rotation);
            }
            
            bulletsInclip--;
            fireDelayTimer = fireRate;
        }
    }

    public void Reload()
    {
        reloading = true;
        reloadTimer = reloadTime;
    }


}
