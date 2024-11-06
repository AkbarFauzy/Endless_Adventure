using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI[] _scoreText;
    [SerializeField] private TextMeshProUGUI _killCountText;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        GameManager.Instance.OnPlay.AddListener(ResetGame);
        GameManager.Instance.OnGameOver.AddListener(ActivateGameOverUI);
        GameManager.Instance.OnUpdateScore.AddListener(UpdateScore);
        _retryButton.onClick.AddListener(RetryStage);
        _mainMenuButton.onClick.AddListener(BackToMainMenu);
    }


    private void OnDestroy()
    {
        GameManager.Instance.OnPlay.RemoveListener(ResetGame);
        GameManager.Instance.OnGameOver.RemoveListener(ActivateGameOverUI);
        GameManager.Instance.OnUpdateScore.RemoveListener(UpdateScore);
        _retryButton.onClick.RemoveListener(RetryStage);
        _mainMenuButton.onClick.RemoveListener(BackToMainMenu);
    }

    private void RetryStage() {
        GameManager.Instance.PlayGame();
    }

    private void BackToMainMenu()
    {
        LevelLoader.Instance.LoadLevel(0);
    }

    private void ResetGame()
    {
        _gameOverPanel.SetActive(false);
    }

    private void UpdateScore(int score)
    {
        foreach (var scoreText in _scoreText) {
            scoreText.text = "Score: " + score;
        }
    }

    private void ActivateGameOverUI()
    {
        _gameOverPanel.SetActive(true);
    }

}
