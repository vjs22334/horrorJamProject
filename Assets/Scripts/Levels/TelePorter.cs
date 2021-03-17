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
        }
    }

    void MoveToNextLevel(){
        FindObjectOfType<Level>().MoveToNextLevel();
    }
}
