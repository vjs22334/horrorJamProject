using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class LaserAttack : BossAttack
{
    public float sweepAngle;
    public float sweepSpeed;
    float currAngle;
    GameObject laserWeapon;
    public Laser laser;

    public override void OnEnter()
    {        
        //sub to bossEnemy's action
        BossEnemy.OnAnimationEvent += InitLaser;

        boss.animator.SetTrigger("Laserattack");

        //search for laser in boss's weapons array
        laserWeapon = boss.SearchForWeapon("Laser");

        //get the laser comp of the weapon
        laser = laserWeapon.GetComponent<Laser>();

        laserWeapon.SetActive(true);
        currAngle = -sweepAngle/2;
        laserWeapon.transform.localEulerAngles = new Vector3(0,0,currAngle);
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.BossLaserClip);
    }

    //method to check if the animation callback was for 'laser' attack
    public void InitLaser(string obj)
    {
        if (obj == "laser")
        {
            //fire laser
            laser.LaserFire();
        }
    }

    public override void OnExit(){
        boss.animator.SetTrigger("idle");
        laserWeapon.SetActive(false);

        //unsub to bossEnemy's action
        BossEnemy.OnAnimationEvent -= InitLaser;
    }

    public override void Tick(Vector3 vectorToTarget){
        if(!laser.firing){
            return;
        } 
        currAngle = Mathf.Clamp(currAngle + sweepSpeed * Time.deltaTime,-sweepAngle/2,sweepAngle/2);
        laserWeapon.transform.localEulerAngles = new Vector3(0,0,currAngle);
        if(currAngle == sweepAngle/2){
            boss.ChooseAttack();
        }
    }
}
