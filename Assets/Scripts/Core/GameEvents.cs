using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnTap;
    public static event Action OnGameStarted;
    public static event Action<bool> OnPauseChanged;
    public static event Action<int> OnScoreUpdated;
    public static event Action OnLanding;
    public static event Action OnPlanetReached;
    public static event Action<Transform> OnPlanetLanded;
    public static event Action OnNearMiss;
    public static event Action OnGameOver;

    public static void Tap() => OnTap?.Invoke();
    public static void StartGame() => OnGameStarted?.Invoke();
    public static void PauseChanged(bool isPaused) => OnPauseChanged?.Invoke(isPaused);
    public static void ScoreUpdated(int score) => OnScoreUpdated?.Invoke(score);
    public static void Landing() => OnLanding?.Invoke();
    public static void PlanetReached() => OnPlanetReached?.Invoke();
    public static void PlanetLanded(Transform planet) => OnPlanetLanded?.Invoke(planet);
    public static void NearMiss() => OnNearMiss?.Invoke();
    public static void GameOver() => OnGameOver?.Invoke();

    public static void ClearAllSubscribers()
    {
        OnTap = null;
        OnGameStarted = null;
        OnPauseChanged = null;
        OnScoreUpdated = null;
        OnLanding = null;
        OnPlanetReached = null;
        OnPlanetLanded = null;
        OnNearMiss = null;
        OnGameOver = null;
    }
}
