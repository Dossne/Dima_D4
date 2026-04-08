using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float planetFailRadius = 8f;

    public float LaunchForce => launchForce;
    public bool IsFlying => _hasLaunched;
    public Vector2 CurrentVelocity => _rigidbody != null ? _rigidbody.linearVelocity : Vector2.zero;

    private Rigidbody2D _rigidbody;
    private PlayerOrbitController _orbitController;
    private Camera _camera;
    private bool _hasLaunched;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _orbitController = GetComponent<PlayerOrbitController>();
        _camera = Camera.main;
        _rigidbody.gravityScale = 0f;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void OnEnable()
    {
        _hasLaunched = false;
        SetOrbitMode();
    }

    private void OnDisable()
    {
        ResetBody();
        _hasLaunched = false;
    }

    private void Update()
    {
        if (_camera == null || !_hasLaunched) return;
        Vector3 vp = _camera.WorldToViewportPoint(transform.position);
        bool outX = vp.x < -0.2f || vp.x > 1.2f;
        bool outY = vp.y < -0.1f || vp.y > 1.1f;
        if (outX || outY || !HasNearbyPlanet())
            GameEvents.GameOver();
    }

    public void Launch()
    {
        if (_hasLaunched || _orbitController == null || _orbitController.currentPivot == null)
        {
            return;
        }

        Vector2 offset = transform.position - _orbitController.currentPivot.position;
        Vector2 tangent = _orbitController.GetLaunchTangent(offset);
        if (tangent == Vector2.zero)
        {
            tangent = Vector2.right;
        }

        SetDynamicMode();
        _rigidbody.AddForce(tangent * launchForce, ForceMode2D.Impulse);
        _hasLaunched = true;
    }

    private void SetOrbitMode()
    {
        if (_rigidbody == null) return;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
    }

    private void SetDynamicMode()
    {
        if (_rigidbody == null) return;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
    }

    private void ResetBody()
    {
        if (_rigidbody == null) return;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
    }

    private bool HasNearbyPlanet()
    {
        foreach (var hit in Physics2D.OverlapCircleAll(transform.position, planetFailRadius))
            if (hit != null && hit.CompareTag("Planet"))
                return true;
        return false;
    }
}
