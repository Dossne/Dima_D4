using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameStateManager : MonoBehaviour
{
    [SerializeField] private float deathSlowMotionScale = 0.15f;
    [SerializeField] private float deathSlowMotionDuration = 0.45f;

    private bool _isDead;
    private bool _isPaused;
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

    public void TogglePause()
    {
        SetPaused(!_isPaused);
    }

    public void SetPaused(bool isPaused)
    {
        if (_isDead || _isPaused == isPaused)
        {
            return;
        }

        _isPaused = isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
        GameEvents.PauseChanged(_isPaused);
    }

    public void RestartGame()
    {
        StopGameOverRoutine();
        _isDead = false;
        _isPaused = false;
        Time.timeScale = 1f;
        GameEvents.ClearAllSubscribers();
        SceneManager.LoadScene(0);
    }

    private void HandleGameStarted()
    {
        StopGameOverRoutine();
        _isDead = false;
        _isPaused = false;
        Time.timeScale = 1f;
        GameEvents.PauseChanged(false);
    }

    private void HandleGameOver()
    {
        if (_isDead)
        {
            return;
        }

        StopGameOverRoutine();
        _isDead = true;
        _isPaused = false;
        _gameOverRoutine = StartCoroutine(HandleGameOverSequence());
    }

    private IEnumerator HandleGameOverSequence()
    {
        Time.timeScale = deathSlowMotionScale;
        yield return new WaitForSecondsRealtime(deathSlowMotionDuration);
        Time.timeScale = 0f;
        _gameOverRoutine = null;
    }

    private void StopGameOverRoutine()
    {
        if (_gameOverRoutine == null)
        {
            return;
        }

        StopCoroutine(_gameOverRoutine);
        _gameOverRoutine = null;
    }
}
