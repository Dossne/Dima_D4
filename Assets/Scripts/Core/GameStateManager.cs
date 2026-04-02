using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameStateManager : MonoBehaviour
{
    [SerializeField] private float deathSlowMotionScale = 0.15f;
    [SerializeField] private float deathSlowMotionDuration = 0.45f;

    private bool _isDead;
    private Coroutine _gameOverRoutine;

    private void Awake()
    {
        Time.timeScale = 1f;

        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        _isDead = false;

        if (_gameOverRoutine != null)
        {
            StopCoroutine(_gameOverRoutine);
            _gameOverRoutine = null;
        }

        SceneManager.LoadScene(0);
    }

    private void HandleGameStarted()
    {
        _isDead = false;
        Time.timeScale = 1f;

        if (_gameOverRoutine != null)
        {
            StopCoroutine(_gameOverRoutine);
            _gameOverRoutine = null;
        }
    }

    private void HandleGameOver()
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;
        _gameOverRoutine = StartCoroutine(HandleGameOverSequence());
    }

    private IEnumerator HandleGameOverSequence()
    {
        Time.timeScale = deathSlowMotionScale;
        yield return new WaitForSecondsRealtime(deathSlowMotionDuration);
        Time.timeScale = 0f;
        _gameOverRoutine = null;
    }
}
