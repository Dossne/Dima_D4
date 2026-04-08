using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new(0f, 0f, -10f);
    [SerializeField] private float orbitCameraSize = 6.7f;
    [SerializeField] private float flightCameraSize = 6.7f;
    [SerializeField] private float sizeSmoothSpeed = 2.5f;
    [SerializeField] private float orbitAnchorWeight = 0.45f;
    [SerializeField] private float orbitLookAhead = 1.6f;
    [SerializeField] private float flightLookAhead = 2.4f;
    [SerializeField] private float routeFrameWeight = 0.4f;
    [SerializeField] private float routeLookAhead = 1.2f;
    [SerializeField] private float routeSizeFactor = 0f;
    [SerializeField] private float maxRouteSizeBonus = 0f;

    public Vector3 ShakeOffset { get; set; }

    private Camera _camera;
    private Transform _cachedPlayer, _currentPlanet, _nextPlanet;
    private PlayerMovement _movement;
    private PlayerOrbitController _orbit;
    private Rigidbody2D _body;

    private void Awake() { _camera = GetComponent<Camera>(); GameEvents.OnRoutePreviewChanged += HandleRoutePreviewChanged; }

    private void OnDestroy() => GameEvents.OnRoutePreviewChanged -= HandleRoutePreviewChanged;

    private void LateUpdate()
    {
        Transform followTarget = target != null ? target : PlayerRegistry.Player;
        if (followTarget == null) return;

        if (_cachedPlayer != followTarget)
        {
            _cachedPlayer = followTarget;
            _movement = followTarget.GetComponent<PlayerMovement>();
            _orbit = followTarget.GetComponent<PlayerOrbitController>();
            _body = followTarget.GetComponent<Rigidbody2D>();
        }

        Transform routeTarget = ResolveRouteTarget();
        Vector3 focus = followTarget.position;
        if (_movement != null && !_movement.IsFlying && _orbit != null && _orbit.currentPivot != null)
            focus = routeTarget == null
                ? Vector3.Lerp(followTarget.position, _orbit.currentPivot.position, orbitAnchorWeight)
                : Vector3.Lerp(Vector3.Lerp(followTarget.position, _orbit.currentPivot.position, orbitAnchorWeight), Vector3.Lerp(_orbit.currentPivot.position, routeTarget.position, 0.5f), routeFrameWeight);

        Vector3 lookAhead = Vector3.zero;
        if (_movement != null && _movement.IsFlying && _body != null && _body.linearVelocity.sqrMagnitude > 0.001f)
            lookAhead = (Vector3)_body.linearVelocity.normalized * flightLookAhead;
        else if (_orbit != null && _orbit.currentPivot != null && routeTarget != null)
            lookAhead = (routeTarget.position - _orbit.currentPivot.position).normalized * routeLookAhead;
        else if (_orbit != null && _orbit.currentPivot != null)
        {
            Vector2 radial = followTarget.position - _orbit.currentPivot.position;
            if (radial != Vector2.zero) lookAhead = (Vector3)Vector2.Perpendicular(radial.normalized) * orbitLookAhead;
        }

        transform.position = Vector3.Lerp(transform.position, focus + lookAhead + offset + ShakeOffset, smoothSpeed * Time.deltaTime);

        if (_camera != null && _camera.orthographic)
        {
            float targetSize = _movement != null && _movement.IsFlying ? flightCameraSize : orbitCameraSize;
            if (_movement != null && !_movement.IsFlying && _currentPlanet != null && routeTarget != null) targetSize += Mathf.Min(Vector3.Distance(_currentPlanet.position, routeTarget.position) * routeSizeFactor, maxRouteSizeBonus);
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetSize, sizeSmoothSpeed * Time.deltaTime);
        }
    }

    private void HandleRoutePreviewChanged(Transform currentPlanet, Transform nextPlanet) { _currentPlanet = currentPlanet; _nextPlanet = nextPlanet; }

    private Transform ResolveRouteTarget()
    {
        if (_nextPlanet != null) return _nextPlanet;
        if (_orbit == null || _orbit.currentPivot == null) return null;
        Transform best = null;
        float bestDistance = float.MaxValue;
        foreach (var hit in Physics2D.OverlapCircleAll(_orbit.currentPivot.position, 8f))
        {
            if (hit == null || !hit.CompareTag("Planet") || hit.transform == _orbit.currentPivot) continue;
            float distance = Vector2.Distance(_orbit.currentPivot.position, hit.transform.position);
            if (distance < bestDistance) { bestDistance = distance; best = hit.transform; }
        }
        return best;
    }
}
