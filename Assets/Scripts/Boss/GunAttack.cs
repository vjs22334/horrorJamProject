using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunAttack : BossAttack
{
    public Gun gun;
    public Animator BossAnimator;

    public bool aimatplayer=true;
    public float fireRate;
    public int clipSize;
    public ShotPattern shotPattern;

    public override void OnEnter()
    {
        BossAnimator = boss.animator;
        BossAnimator.SetTrigger("GunAim");
        gun = boss.gun;
        gun.fireRate = fireRate;
        gun.shotPattern = shotPattern;
        gun.clipSize = clipSize;
        gun.reloadTime = 0;
        gun.Reload(); 
    }

    public override void OnExit()
    {
        BossAnimator.SetTrigger("idle");
    }

    public override void Tick(Vector3 vectorToTarget)
    {
        if(gun.BulletsInClip > 0){
            if(aimatplayer)
            {
            Vector3 aimDirection = vectorToTarget.normalized;
            float angle = Mathf.Atan2(aimDirection.y,aimDirection.x)*Mathf.Rad2Deg;
            gun.transform.eulerAngles = new Vector3(0,0,angle);

            }
           
            gun.Fire();
        }
        else{
            boss.ChooseAttack();
        }
    }
}
