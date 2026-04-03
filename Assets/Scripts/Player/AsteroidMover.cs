using UnityEngine;

public class AsteroidMover : MonoBehaviour
{
    [SerializeField] private float homingStrength = 1.2f;

    private Vector2 _velocity;
    private Transform _player;
    private float _speed;

    private void Awake()
    {
        // Disable physics if Rigidbody2D exists — movement handled via transform
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Init(float speed)
    {
        _speed = speed;
        _player = PlayerRegistry.Player;

        Vector2 toPlayer = _player != null
            ? ((Vector2)_player.position - (Vector2)transform.position).normalized
            : Vector2.up;

        _velocity = toPlayer * _speed;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        Destroy(gameObject, 10f);
    }

    private bool _nearMissFired;

    private void Update()
    {
        if (_player != null)
        {
            Vector2 toPlayer = ((Vector2)_player.position - (Vector2)transform.position).normalized;
            _velocity = Vector2.Lerp(_velocity.normalized, toPlayer, Time.deltaTime * homingStrength) * _speed;

            float dist = Vector2.Distance(transform.position, _player.position);
            if (dist < 0.5f && dist > 0.15f && !_nearMissFired)
            {
                _nearMissFired = true;
                GameEvents.NearMiss();
            }
            else if (dist >= 0.5f)
            {
                _nearMissFired = false;
            }
        }

        transform.Translate(_velocity * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, 80 * Time.deltaTime);

        // Destroy if far outside viewport
        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 vp = cam.WorldToViewportPoint(transform.position);
            if (vp.x < -0.3f || vp.x > 1.3f || vp.y < -0.3f || vp.y > 1.3f)
                Destroy(gameObject);
        }
    }
}
