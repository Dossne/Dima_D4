using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerCollider : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float maxInwardCaptureDot = 0.75f;

    private PlayerMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            if (IsBadFrontalEntry(other.transform))
            {
                GameEvents.GameOver();
                return;
            }

            GameEvents.PlanetLanded(other.transform);
            GameEvents.Landing();
            GameEvents.PlanetReached();
            return;
        }

        if (other.CompareTag("Asteroid"))
        {
            GameEvents.GameOver();
        }
    }

    private bool IsBadFrontalEntry(Transform planet)
    {
        if (_movement == null || !_movement.IsFlying)
        {
            return false;
        }

        Vector2 velocity = _movement.CurrentVelocity;
        if (velocity.sqrMagnitude < 0.01f)
        {
            return false;
        }

        Vector2 radial = ((Vector2)(transform.position - planet.position)).normalized;
        if (radial == Vector2.zero)
        {
            return false;
        }

        float inwardDot = Vector2.Dot(velocity.normalized, -radial);
        return inwardDot > maxInwardCaptureDot;
    }
}
