using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePorter : MonoBehaviour
{
    public GameObject TeleportPS;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            Instantiate(TeleportPS,transform.position,Quaternion.identity);
            Invoke(nameof(MoveToNextLevel),0.75f);
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.TeleporterClip);
        }
    }

    void MoveToNextLevel(){
        FindObjectOfType<Level>().MoveToNextLevel();
    }
}
