using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int maxLevels = 5;
    public string startingScene;
    public string endingScene;

    public List<string> Levels;

    public GameObject GameOverPanel;
    
    public GameObject PausePanel;

    public GameObject playerGO;
    int LevelsLoadedCount = 0;

    string CurrLoadedLevel;
    string levelToLoad;

    public GameObject PlayerSpawnPs;


    private static LevelManager _instance = null;
    public static LevelManager Instance{
        get{
            if(_instance == null){
                _instance = FindObjectOfType<LevelManager>();
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

 
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoadedHandler;

    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedHandler;
    }
    

    void Start()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(startingScene,LoadSceneMode.Additive);
        levelToLoad = startingScene;
    }

    private void OnSceneLoadedHandler(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == levelToLoad){
            Level loadedLevel = FindObjectOfType<Level>();
            loadedLevel.Initialize(playerGO);
            LevelsLoadedCount++;
            CurrLoadedLevel = levelToLoad;
        }
        
    }

   
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
            Pause();
        }
    }

    public void LoadNextLevel(){
        if(LevelsLoadedCount<=maxLevels){
            int i = UnityEngine.Random.Range(0,Levels.Count);
            string nextLevel = Levels[i];
            Levels.RemoveAt(i);
            levelToLoad = nextLevel;
            SceneManager.UnloadSceneAsync(CurrLoadedLevel);
            SceneManager.LoadScene(levelToLoad,LoadSceneMode.Additive);
        }
        else{
            levelToLoad = endingScene;
            SceneManager.UnloadSceneAsync(CurrLoadedLevel);
            SceneManager.LoadScene(levelToLoad,LoadSceneMode.Additive);
        }
    }

    public void GameOver(){
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("Game");
    }

    public void Pause(){
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }

    public void Quit(){
        Application.Quit();
    }

    public void Resume(){
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    

}

