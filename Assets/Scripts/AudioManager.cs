using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGAudioSource;
    public AudioSource SfxAudioSource;
    public AudioClip BattleClip;
    public AudioClip ambienceClip;
    public AudioClip BossFightClip;
    public AudioClip BossfightEndClip;

    public CustomAudioClip TeleporterClip;
    public CustomAudioClip GunFireClip;

    public CustomAudioClip EnemyDeadClip;

    public CustomAudioClip BossLaserClip;


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
    public void PlayBossMusic(){
        BGAudioSource.clip = BossFightClip;
        BGAudioSource.Play();
    }
    public void PlayBossEndMusic(){
        BGAudioSource.clip = BossfightEndClip;
        BGAudioSource.Play();
    }

    public void PlayOneShot(CustomAudioClip clip){
        if(clip.clip!=null)
            SfxAudioSource.PlayOneShot(clip.clip,clip.volumeMultiplier);
    }        
}

[System.Serializable]
public struct CustomAudioClip{
    public AudioClip clip;
    [Range(0,1)]
    public float volumeMultiplier;
}
