using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    private int _currentScore;
    private int _highScore;

    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnPlanetReached += HandlePlanetReached;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnPlanetReached -= HandlePlanetReached;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }

    public int GetHighScore()
    {
        return _highScore;
    }

    private void HandleGameStarted()
    {
        _currentScore = 0;
        GameEvents.ScoreUpdated(_currentScore);
    }

    private void HandlePlanetReached()
    {
        _currentScore++;
        GameEvents.ScoreUpdated(_currentScore);
    }

    private void HandleGameOver()
    {
        if (_currentScore <= _highScore)
        {
            return;
        }

        _highScore = _currentScore;
        PlayerPrefs.SetInt(HighScoreKey, _highScore);
        PlayerPrefs.Save();
    }
}
