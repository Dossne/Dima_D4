using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Planet planetPrefab;
    [SerializeField] private float firstPlanetDistance = 4f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 8f;

    private Camera _mainCamera;
    private Planet _currentPlanet;

    private void Awake()
    {
        _mainCamera = Camera.main;
        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnScoreUpdated += HandleScoreUpdated;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnScoreUpdated -= HandleScoreUpdated;
    }

    private void HandleGameStarted()
    {
        if (PlayerRegistry.Player == null || planetPrefab == null)
        {
            return;
        }

        _currentPlanet = Instantiate(planetPrefab, PlayerRegistry.Player.position + Vector3.up * firstPlanetDistance, Quaternion.identity);
    }

    private void HandleScoreUpdated(int _)
    {
        if (PlayerRegistry.Player == null || planetPrefab == null)
        {
            return;
        }

        Planet previousPlanet = _currentPlanet;
        _currentPlanet = Instantiate(planetPrefab, FindSpawnPosition(), Quaternion.identity);
        if (previousPlanet != null)
        {
            Destroy(previousPlanet.gameObject, 2f);
        }
    }

    private Vector3 FindSpawnPosition()
    {
        Vector3 playerPosition = PlayerRegistry.Player.position;
        for (int i = 0; i < 10; i++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDistance, maxDistance);
            Vector3 candidate = playerPosition + (Vector3)(direction * distance);
            Vector3 viewportPoint = _mainCamera.WorldToViewportPoint(candidate);
            if (viewportPoint.x is > 0.1f and < 0.9f && viewportPoint.y is > 0.1f and < 0.9f)
            {
                return candidate;
            }
        }

        return playerPosition + Vector3.up * firstPlanetDistance;
    }
}
