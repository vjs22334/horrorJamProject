using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGAudioSource;
    public AudioClip BattleClip;
    public AudioClip ambienceClip;
    private static AudioManager _instance = null;
    public static AudioManager Instance{
        get{
            if(_instance == null){
                _instance = FindObjectOfType<AudioManager>();
            }
            return _instance;
        }
    }
    void Awake(){
        if(_instance == null){
            _instance = this;
        }
        else if(_instance != this){
            Destroy(this.gameObject);
        }
    }
     

    public void PlayBattleMusic(){
        BGAudioSource.clip = BattleClip;
        BGAudioSource.Play();
    } 

    public void PlayAmbienceMusic(){
        BGAudioSource.clip = ambienceClip;
        BGAudioSource.Play();
    }        
}
