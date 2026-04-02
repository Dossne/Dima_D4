using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private AsteroidMover asteroidPrefab;
    [SerializeField] private float baseSpeed = 4f;
    [SerializeField] private float viewportMargin = 0.12f;
    [SerializeField] private int safeEdgeAttempts = 5;

    private Camera _mainCamera;
    private Transform _player;
    private PlayerOrbitController _orbitController;

    private void Awake()
    {
        _mainCamera = Camera.main;
        GameEvents.OnScoreUpdated += HandleScoreUpdated;
    }

    private void OnDestroy()
    {
        GameEvents.OnScoreUpdated -= HandleScoreUpdated;
    }

    private void HandleScoreUpdated(int score)
    {
        if (!ShouldSpawnAsteroid(score))
        {
            return;
        }

        RefreshPlayerContext();
        if (_player == null || asteroidPrefab == null || _mainCamera == null)
        {
            return;
        }

        float speed = GetSpawnSpeed(score);
        Vector3 spawnPosition = GetSafeEdgePoint();
        AsteroidMover asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        asteroid.Init(speed);
    }

    private bool ShouldSpawnAsteroid(int score)
    {
        if (score < 6)
        {
            return false;
        }

        if (score < 16)
        {
            return score >= 8 && (score - 8) % 4 == 0;
        }

        return (score - 16) % 3 == 0;
    }

    private float GetSpawnSpeed(int score)
    {
        return score >= 16 ? baseSpeed * 1.2f : baseSpeed;
    }

    private void RefreshPlayerContext()
    {
        if (_player == null)
        {
            _player = PlayerRegistry.Player;
        }

        if (_player != null && _orbitController == null)
        {
            _orbitController = _player.GetComponent<PlayerOrbitController>();
        }
    }

    private Vector3 GetSafeEdgePoint()
    {
        for (int attempt = 0; attempt < safeEdgeAttempts; attempt++)
        {
            Vector3 candidate = GetRandomEdgePoint();
            if (IsSafeFromCurrentPlanet(candidate))
            {
                return candidate;
            }
        }

        return GetFallbackEdgePoint();
    }

    private Vector3 GetRandomEdgePoint()
    {
        float zDistance = GetCameraDistanceToPlane();
        int edge = Random.Range(0, 4);
        float t = Random.Range(viewportMargin, 1f - viewportMargin);

        Vector3 viewportPoint = edge switch
        {
            0 => new Vector3(-viewportMargin, t, zDistance),
            1 => new Vector3(1f + viewportMargin, t, zDistance),
            2 => new Vector3(t, -viewportMargin, zDistance),
            _ => new Vector3(t, 1f + viewportMargin, zDistance),
        };

        Vector3 worldPoint = _mainCamera.ViewportToWorldPoint(viewportPoint);
        worldPoint.z = _player.position.z;
        return worldPoint;
    }

    private Vector3 GetFallbackEdgePoint()
    {
        float zDistance = GetCameraDistanceToPlane();
        Vector3 planetDirection = GetPlanetDirection();

        Vector3 viewportPoint = Mathf.Abs(planetDirection.x) > Mathf.Abs(planetDirection.y)
            ? new Vector3(planetDirection.x > 0f ? -viewportMargin : 1f + viewportMargin, 0.5f, zDistance)
            : new Vector3(0.5f, planetDirection.y > 0f ? -viewportMargin : 1f + viewportMargin, zDistance);

        Vector3 worldPoint = _mainCamera.ViewportToWorldPoint(viewportPoint);
        worldPoint.z = _player.position.z;
        return worldPoint;
    }

    private bool IsSafeFromCurrentPlanet(Vector3 candidate)
    {
        Vector3 planetDirection = GetPlanetDirection();
        Vector2 asteroidDirection = (Vector2)(candidate - _player.position);

        if (planetDirection == Vector3.zero || asteroidDirection == Vector2.zero)
        {
            return true;
        }

        return Vector2.Angle(asteroidDirection, planetDirection) > 60f;
    }

    private Vector3 GetPlanetDirection()
    {
        if (_orbitController == null || _orbitController.currentPivot == null || _player == null)
        {
            return Vector3.zero;
        }

        return _orbitController.currentPivot.position - _player.position;
    }

    private float GetCameraDistanceToPlane()
    {
        if (_mainCamera == null || _player == null)
        {
            return 0f;
        }

        return Mathf.Abs(_mainCamera.transform.position.z - _player.position.z);
    }
}
