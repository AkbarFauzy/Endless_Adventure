using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Data data;
    public float CurrentScore;
    public Dictionary<string, int> CurrentEnemyKilled;

    public bool isPlaying = false;

    public UnityEvent OnPlay = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();
    public UnityEvent<int> OnUpdateScore = new UnityEvent<int>();
    public UnityEvent<Enemy> OnEnemyKilled = new UnityEvent<Enemy>();

    public bool loadNextLevel;
    private bool IsLevel1 { get => SceneManager.GetActiveScene().buildIndex == 1; }
    private bool IsLevel2 { get => SceneManager.GetActiveScene().buildIndex == 2; } 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        data = SaveSystem.Load("save");
        if (data == null) {
            data = new Data();
        }

        CurrentEnemyKilled = new Dictionary<string, int>();
    }

    private void Update()
    {
        if (isPlaying) {
            UpdateScore();
            if(CurrentScore > 120 && IsLevel1 && !loadNextLevel)
            {
                loadNextLevel = true;
                LevelLoader.Instance.LoadLevel(2);
                isPlaying = true;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadNextLevel = false;
        if (scene.buildIndex == 0) {
            isPlaying = false;
        }
        else if (scene.buildIndex == 1) {
            OnPlay.Invoke();
            CurrentEnemyKilled = new Dictionary<string, int>();
            CurrentScore = 0;
            isPlaying = true;
        }
    }

    public void PlayGame()
    {
        LevelLoader.Instance.LoadLevel(1);
    }

    public void GameOver()
    {
        OnGameOver.Invoke();
        isPlaying = false;

        if(data.HighScore < CurrentScore)
            data.HighScore = CurrentScore;

        foreach (var enemy in CurrentEnemyKilled)
        {
            if (data.EnemyKilled.ContainsKey(enemy.Key)) {
                data.EnemyKilled[enemy.Key] += enemy.Value;
            }
            else
            {
                data.EnemyKilled.Add(enemy.Key, enemy.Value);
            }
            Debug.Log(data.EnemyKilled[enemy.Key]);
        }

        string saveString = JsonConvert.SerializeObject(data);
        SaveSystem.Save("save", saveString);
    }

    public void UpdateScore()
    {
        CurrentScore += Time.deltaTime;
        OnUpdateScore.Invoke((int)CurrentScore);
    }

    public void EnemyKilled(Enemy enemy)
    {
        OnEnemyKilled.Invoke(enemy);
        if (CurrentEnemyKilled.ContainsKey(enemy.EnemyName))
        {
            CurrentEnemyKilled[enemy.EnemyName] += 1;
        }
        else
        {
            CurrentEnemyKilled.Add(enemy.EnemyName, 1);
        }
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
