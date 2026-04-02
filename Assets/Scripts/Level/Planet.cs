using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(LineRenderer))]
public class Planet : MonoBehaviour
{
    [SerializeField] private float orbitRadius = 1.5f;

    private CircleCollider2D _collider;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _collider.radius = orbitRadius;
        DrawOrbitRing();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }

    private void DrawOrbitRing()
    {
        const int segments = 64;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 point = new(Mathf.Cos(angle) * orbitRadius, Mathf.Sin(angle) * orbitRadius, 0f);
            _lineRenderer.SetPosition(i, point);
        }
    }
}
