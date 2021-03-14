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
        playerGO.transform.position = PlayerSpawnPos.position;
        camera.LookAt = playerGO.transform;
        camera.Follow = playerGO.transform;
        playerGO.SetActive(true);
        Player = playerGO;

        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.Initialize(playerGO.transform);
            enemiesCount++;
        }

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
        }
    }
}
