using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LaserFollowAttack : LaserAttack
{
    public int counterLength;

    //[Tooltip("How short each increment of the counter should be")]
    //public int durationFactor;

    int counter;
    GameObject laserWeapon;
    float currAngle;

    public override void OnEnter()
    {
        counter = counterLength;
        //sub to bossEnemy's action
        BossEnemy.OnAnimationEvent += InitLaser;

        boss.animator.SetTrigger("Laserattack");

        //search for laser in boss's weapons array
        laserWeapon = boss.SearchForWeapon("Laser");

        //get the laser comp of the weapon
        laser = laserWeapon.GetComponent<Laser>();

        laserWeapon.SetActive(true);
        currAngle = -sweepAngle / 2;
        laserWeapon.transform.localEulerAngles = new Vector3(0, 0, currAngle);
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.BossLaserClip);
    }

    public override void Tick(Vector3 vectorToTarget)
    {
        if (!laser.firing)
        {
            return;
        }

        if (counter <= 0)
        {
            boss.ChooseAttack();
        }
        else
        {
            //check angle between player and laser origin
            currAngle = Vector3.Angle(vectorToTarget, laserWeapon.transform.position);

            //based on angle sign change laser dir
            if (currAngle > 0)
            {
                //right side            
                laserWeapon.transform.rotation = Quaternion.AngleAxis(-currAngle, Vector3.up);
            }
            else
            {
                //left side
                laserWeapon.transform.rotation = Quaternion.AngleAxis(currAngle, Vector3.up);
            }


            //simultaneously adjust laser length to match recent player postion, may use polymorphed update of Laser script

            counter -= counterLength / 100;
        }
    }

    public override void OnExit()
    {
        boss.animator.SetTrigger("idle");
        laserWeapon.SetActive(false);

        //unsub to bossEnemy's action
        BossEnemy.OnAnimationEvent -= InitLaser;
    }
}
