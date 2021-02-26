using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReceive : MonoBehaviour
{
    [SerializeField] float MinSoundPower = 10f;               //The minimun amount of sound power that takes to do something
    SpriteRenderer sr;
    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }
    public void ReceiveSound(float SoundPower) {
        if(SoundPower > MinSoundPower) {
            sr.color = Color.red; //Debug
        }
    }
}
