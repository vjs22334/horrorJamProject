using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Level : MonoBehaviour
{
    public Transform PlayerSpawnPos;
    public CinemachineVirtualCamera camera;

    public GameObject Teleporter;

    GameObject Player;
    int enemiesCount = 0;
    
    public void Initialize(GameObject playerGO){
        Player = playerGO;
        Player.transform.position = PlayerSpawnPos.position;
        camera.LookAt = Player.transform;
        camera.Follow = Player.transform;
        Instantiate(LevelManager.Instance.PlayerSpawnPs,PlayerSpawnPos.position,Quaternion.identity);
        Invoke(nameof(ActivatePlayer),0.6f);

        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.Initialize(Player.transform);
            enemiesCount++;
        }
        if(enemiesCount > 0){
            AudioManager.Instance.PlayBattleMusic();
        }
        else{
            AudioManager.Instance.PlayAmbienceMusic();
        }



    }

    void ActivatePlayer(){
        Player.SetActive(true);
    }

    public void MoveToNextLevel(){
        LevelManager.Instance.LoadNextLevel();
        Player.SetActive(false);
    }

    void OnEnable()
    {
        Enemy.OnEnemyKilled += OnEnemyKilledHandler;
    }

    void OnDisable()
    {
        Enemy.OnEnemyKilled -= OnEnemyKilledHandler;
    }

    private void OnEnemyKilledHandler()
    {
        enemiesCount--;
        if(enemiesCount <= 0){
            Teleporter.SetActive(true);
            AudioManager.Instance.PlayAmbienceMusic();
        }
    }
}
