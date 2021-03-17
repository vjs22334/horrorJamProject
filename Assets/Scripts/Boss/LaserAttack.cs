using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class LaserAttack : BossAttack
{
    public float sweepAngle;
    public float sweepSpeed;
    float currAngle;
    GameObject laser;
    public override void OnEnter()
    {
        boss.animator.SetTrigger("Laserattack");
        laser = boss.laser;
        laser.SetActive(true);
        currAngle = -sweepAngle/2;
        laser.transform.localEulerAngles = new Vector3(0,0,currAngle);
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.BossLaserClip);
    }

    public override void OnExit(){
        boss.animator.SetTrigger("idle");
        laser.SetActive(false);
    }

    public override void Tick(Vector3 vectorToTarget){
        if(!boss.laser.GetComponent<Laser>().firing){
            return;
        } 
        currAngle = Mathf.Clamp(currAngle + sweepSpeed * Time.deltaTime,-sweepAngle/2,sweepAngle/2);
        laser.transform.localEulerAngles = new Vector3(0,0,currAngle);
        if(currAngle == sweepAngle/2){
            boss.ChooseAttack();
        }
    }

    
}
