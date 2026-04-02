using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float launchForce = 10f;

    public float LaunchForce => launchForce;

    private Camera _mainCamera;
    private Rigidbody2D _rigidbody;
    private PlayerOrbitController _orbitController;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        _orbitController = GetComponent<PlayerOrbitController>();
        _rigidbody.gravityScale = 0f;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void OnEnable()
    {
        if (_orbitController == null || _orbitController.currentPivot == null)
        {
            return;
        }

        Vector2 offset = transform.position - _orbitController.currentPivot.position;
        Vector2 tangent = Vector2.Perpendicular(offset.normalized);
        _rigidbody.AddForce(tangent * launchForce, ForceMode2D.Impulse);
    }

    private void OnDisable()
    {
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (_mainCamera == null)
        {
            return;
        }

        Vector3 viewportPoint = _mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPoint.x < 0f || viewportPoint.x > 1f || viewportPoint.y < 0f || viewportPoint.y > 1f)
        {
            GameEvents.GameOver();
        }
    }
}
