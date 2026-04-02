using TMPro;
using UnityEngine;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private GameObject hudRoot;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string scorePrefix = "SCORE: ";

    private void Awake()
    {
        if (hudRoot == null)
        {
            hudRoot = gameObject;
        }

        hudRoot.SetActive(false);
        UpdateScoreText(0);

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

    private void HandleGameStarted()
    {
        hudRoot.SetActive(true);
    }

    private void HandleGameOver()
    {
        hudRoot.SetActive(false);
    }

    private void HandleScoreUpdated(int score)
    {
        UpdateScoreText(score);
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText == null)
        {
            return;
        }

        scoreText.text = scorePrefix + score;
    }
}
