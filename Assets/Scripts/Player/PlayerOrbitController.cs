using UnityEngine;

public class PlayerOrbitController : MonoBehaviour
{
    [SerializeField] private float baseOrbitSpeed = 120f;
    [SerializeField] private float speedPerPlanet = 2f;   // +2 deg/s per planet after ramp start
    [SerializeField] private int speedRampStart = 15;     // score at which speed ramp begins

    public Transform currentPivot;

    private PlayerMovement _playerMovement;
    private float _currentSpeed;

    private void Awake()
    {
        PlayerRegistry.Register(transform);
        _playerMovement = GetComponent<PlayerMovement>();
        _currentSpeed = baseOrbitSpeed;
        GameEvents.OnTap += HandleTap;
        GameEvents.OnScoreUpdated += HandleScoreUpdated;
        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnPlanetLanded += HandlePlanetLanded;
    }

    private void OnDestroy()
    {
        PlayerRegistry.Unregister();
        GameEvents.OnTap -= HandleTap;
        GameEvents.OnScoreUpdated -= HandleScoreUpdated;
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnPlanetLanded -= HandlePlanetLanded;
    }

    private void Update()
    {
        if (!enabled || currentPivot == null) return;
        transform.RotateAround(currentPivot.position, Vector3.forward, _currentSpeed * Time.deltaTime);
    }

    private void HandleTap()
    {
        if (currentPivot == null) return;
        enabled = false;
        SetMovementEnabled(true);
        _playerMovement?.Launch();
    }

    private void HandleScoreUpdated(int score)
    {
        _currentSpeed = score > speedRampStart
            ? baseOrbitSpeed + (score - speedRampStart) * speedPerPlanet
            : baseOrbitSpeed;

        enabled = true;
        SetMovementEnabled(false);
    }

    private void HandleGameStarted()
    {
        _currentSpeed = baseOrbitSpeed;
        enabled = true;
        SetMovementEnabled(false);
    }

    private void HandlePlanetLanded(Transform planet)
    {
        currentPivot = planet;
    }

    private void SetMovementEnabled(bool value)
    {
        if (_playerMovement != null)
        {
            _playerMovement.enabled = value;
        }
    }
}
