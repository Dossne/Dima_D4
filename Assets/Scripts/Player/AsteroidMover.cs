using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidMover : MonoBehaviour
{
    [SerializeField] private float homingStrength = 1.2f;
    [SerializeField] private float viewportCullMargin = 0.3f;

    private Rigidbody2D _rigidbody;
    private Transform _player;
    private float _speed;
    private bool _isInitialized;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void Init(float speed)
    {
        _speed = speed;
        _player = PlayerRegistry.Player;

        if (_player == null)
        {
            return;
        }

        Vector2 direction = ((Vector2)(_player.position - transform.position)).normalized;
        if (direction == Vector2.zero)
        {
            direction = Vector2.up;
        }

        _rigidbody.linearVelocity = direction * _speed;
        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized || _player == null)
        {
            return;
        }

        if (IsOutsideViewport())
        {
            Destroy(gameObject);
            return;
        }

        Vector2 desiredVelocity = ((Vector2)(_player.position - transform.position)).normalized * _speed;
        _rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, desiredVelocity, homingStrength * Time.deltaTime);
    }

    private bool IsOutsideViewport()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return false;
        }

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        return viewportPosition.x < -viewportCullMargin || viewportPosition.x > 1f + viewportCullMargin || viewportPosition.y < -viewportCullMargin || viewportPosition.y > 1f + viewportCullMargin;
    }
}
