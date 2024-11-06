using UnityEngine.UI;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;


    private void Start()
    {
        _playButton.onClick.AddListener(StartGame);
        _exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(StartGame);
        _exitButton.onClick.RemoveListener(ExitGame);
    }

    private void StartGame()
    {
        GameManager.Instance.PlayGame();
    }

    private void ExitGame()
    {
        Application.Quit();
    }

}
