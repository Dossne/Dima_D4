using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverRoot;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameStateManager stateManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private string currentScorePrefix = "SCORE: ";
    [SerializeField] private string highScorePrefix = "BEST: ";

    private Coroutine _refreshRoutine;

    private void Awake()
    {
        if (gameOverRoot == null)
        {
            gameOverRoot = gameObject;
        }

        gameOverRoot.SetActive(false);
        RefreshScores();

        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnScoreUpdated += HandleScoreUpdated;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnScoreUpdated -= HandleScoreUpdated;
    }

    public void OnRestartPressed()
    {
        if (stateManager != null)
        {
            stateManager.RestartGame();
            return;
        }

        SceneManager.LoadScene(0);
    }

    private void HandleGameStarted()
    {
        HidePanel();
    }

    private void HandleGameOver()
    {
        ShowPanel();

        if (_refreshRoutine != null)
        {
            StopCoroutine(_refreshRoutine);
        }

        _refreshRoutine = StartCoroutine(RefreshScoresNextFrame());
    }

    private void HandleScoreUpdated(int score)
    {
        if (!gameOverRoot.activeSelf)
        {
            return;
        }

        RefreshScores();
    }

    private void ShowPanel()
    {
        gameOverRoot.SetActive(true);
    }

    private void HidePanel()
    {
        gameOverRoot.SetActive(false);
    }

    private IEnumerator RefreshScoresNextFrame()
    {
        yield return null;
        RefreshScores();
        _refreshRoutine = null;
    }

    private void RefreshScores()
    {
        if (currentScoreText != null)
        {
            int currentScore = scoreManager != null ? scoreManager.GetCurrentScore() : 0;
            currentScoreText.text = currentScorePrefix + currentScore;
        }

        if (highScoreText != null)
        {
            int highScore = scoreManager != null ? scoreManager.GetHighScore() : 0;
            highScoreText.text = highScorePrefix + highScore;
        }
    }
}
