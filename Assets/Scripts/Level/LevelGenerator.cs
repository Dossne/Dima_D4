using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Planet planetPrefab;
    [SerializeField] private float firstPlanetDistance = 4f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private float minPlanetSeparation = 3.5f;

    private Camera _mainCamera;
    private Planet _orbitPlanet;
    private Planet _nextTarget;

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
        if (!isActiveAndEnabled) return;
        if (PlayerRegistry.Player == null || planetPrefab == null) return;

        CleanUp();
        _orbitPlanet = Instantiate(planetPrefab, PlayerRegistry.Player.position + Vector3.up * firstPlanetDistance, Quaternion.identity);
        GameEvents.PlanetLanded(_orbitPlanet.transform);
        SpawnNextTarget();
        NotifyRoutePreview();
    }

    private void HandleScoreUpdated(int score)
    {
        if (!isActiveAndEnabled) return;
        if (score <= 0 || PlayerRegistry.Player == null || planetPrefab == null) return;

        Planet old = _orbitPlanet;
        _orbitPlanet = _nextTarget;
        if (old != null) Destroy(old.gameObject);
        SpawnNextTarget();
        NotifyRoutePreview();
    }

    private void SpawnNextTarget()
    {
        if (PlayerRegistry.Player == null || planetPrefab == null) return;
        _nextTarget = Instantiate(planetPrefab, FindSpawnPosition(), Quaternion.identity);
    }

    private void NotifyRoutePreview()
    {
        GameEvents.RoutePreviewChanged(_orbitPlanet != null ? _orbitPlanet.transform : null, _nextTarget != null ? _nextTarget.transform : null);
    }

    private void CleanUp()
    {
        if (_orbitPlanet != null) { Destroy(_orbitPlanet.gameObject); _orbitPlanet = null; }
        if (_nextTarget != null) { Destroy(_nextTarget.gameObject); _nextTarget = null; }
    }

    private Vector3 FindSpawnPosition()
    {
        Vector3 playerPosition = PlayerRegistry.Player.position;
        Vector3 blockedPosition = _orbitPlanet != null ? _orbitPlanet.transform.position : playerPosition;
        for (int i = 0; i < 10; i++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            if (direction == Vector2.zero)
            {
                continue;
            }

            float distance = Random.Range(minDistance, maxDistance);
            Vector3 candidate = playerPosition + (Vector3)(direction * distance);
            if (Vector3.Distance(candidate, blockedPosition) < minPlanetSeparation)
            {
                continue;
            }

            Vector3 viewportPoint = _mainCamera.WorldToViewportPoint(candidate);
            if (viewportPoint.x is > 0.1f and < 0.9f && viewportPoint.y is > 0.1f and < 0.9f)
            {
                return candidate;
            }
        }

        return GetFallbackSpawnPosition(playerPosition, blockedPosition);
    }

    private Vector3 GetFallbackSpawnPosition(Vector3 playerPosition, Vector3 blockedPosition)
    {
        if (_mainCamera == null)
        {
            return playerPosition + Vector3.right * minDistance;
        }

        float cameraDistance = Mathf.Abs(_mainCamera.transform.position.z - playerPosition.z);
        Vector3[] fallbackViewportPoints =
        {
            new(0.75f, 0.68f, cameraDistance),
            new(0.25f, 0.68f, cameraDistance),
            new(0.75f, 0.35f, cameraDistance),
            new(0.25f, 0.35f, cameraDistance)
        };

        foreach (Vector3 viewportPoint in fallbackViewportPoints)
        {
            Vector3 candidate = _mainCamera.ViewportToWorldPoint(viewportPoint);
            candidate.z = playerPosition.z;
            if (Vector3.Distance(candidate, blockedPosition) >= minPlanetSeparation)
            {
                return candidate;
            }
        }

        return playerPosition + Vector3.right * minDistance;
    }
}
