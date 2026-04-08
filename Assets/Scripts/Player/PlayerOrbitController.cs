using UnityEngine;

public class PlayerOrbitController : MonoBehaviour
{
    [SerializeField] private float baseOrbitSpeed = 120f;
    [SerializeField] private float speedPerPlanet = 2f;
    [SerializeField] private int speedRampStart = 15;
    public Transform currentPivot;
    private OrbitCaptureSmoother _captureSmoother;
    private PlayerMovement _playerMovement;
    private float _orbitDirection = 1f;
    private float _currentSpeed;

    private void Awake()
    {
        PlayerRegistry.Register(transform);
        _captureSmoother = GetComponent<OrbitCaptureSmoother>();
        _playerMovement = GetComponent<PlayerMovement>();
        _currentSpeed = baseOrbitSpeed;
        GameEvents.OnTap += HandleTap;
        GameEvents.OnScoreUpdated += HandleScoreUpdated;
        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnPlanetLanded += HandlePlanetLanded;
    }

    private void OnDestroy() { PlayerRegistry.Unregister(); GameEvents.OnTap -= HandleTap; GameEvents.OnScoreUpdated -= HandleScoreUpdated; GameEvents.OnGameStarted -= HandleGameStarted; GameEvents.OnPlanetLanded -= HandlePlanetLanded; }

    private void Update()
    {
        if (!enabled || currentPivot == null) return;
        _captureSmoother?.Tick();
        if (_captureSmoother != null && _captureSmoother.IsCapturing) return;
        transform.RotateAround(currentPivot.position, Vector3.forward, _currentSpeed * _orbitDirection * Time.deltaTime);
    }

    private void HandleTap() { if (currentPivot == null) return; enabled = false; SetMovementEnabled(true); _playerMovement?.Launch(); }

    private void HandleScoreUpdated(int score)
    {
        _currentSpeed = score > speedRampStart ? baseOrbitSpeed + (score - speedRampStart) * speedPerPlanet : baseOrbitSpeed;
        enabled = true;
        SetMovementEnabled(false);
    }

    private void HandleGameStarted()
    {
        _currentSpeed = baseOrbitSpeed;
        _orbitDirection = 1f;
        enabled = true;
        SetMovementEnabled(false);
    }

    private void HandlePlanetLanded(Transform planet) { UpdateOrbitDirection(planet); currentPivot = planet; }

    private void UpdateOrbitDirection(Transform planet)
    {
        Vector2 radial = _captureSmoother != null ? _captureSmoother.GetRadial(planet.position) : (transform.position - planet.position).normalized;
        if (radial == Vector2.zero) return;
        float turn = radial.x * _playerMovement.CurrentVelocity.y - radial.y * _playerMovement.CurrentVelocity.x;
        if (Mathf.Abs(turn) > 0.05f) _orbitDirection = Mathf.Sign(turn);
        Planet body = planet.GetComponent<Planet>();
        _captureSmoother?.BeginCapture(planet, body != null ? body.orbitRadius : 1.5f, _orbitDirection, _currentSpeed);
    }

    private void SetMovementEnabled(bool value) { if (_playerMovement != null) _playerMovement.enabled = value; }

    public Vector2 GetLaunchTangent(Vector2 radial) => Vector2.Perpendicular(radial.normalized) * _orbitDirection;
}
