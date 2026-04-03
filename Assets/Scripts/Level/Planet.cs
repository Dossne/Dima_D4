using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Planet : MonoBehaviour
{
    [SerializeField] public float orbitRadius = 1.5f;
    [SerializeField] private float planetRadius = 0.45f;
    [SerializeField] private Color planetColor = new Color(1f, 0.5f, 0.1f, 1f);
    [SerializeField] private Color orbitRingColor = new Color(1f, 0.5f, 0.1f, 0.25f);
    [SerializeField] private float planetLineWidth = 0.1f;
    [SerializeField] private float orbitLineWidth = 0.04f;

    private void Start()
    {
        var col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            col.isTrigger = true;
            col.radius = orbitRadius;
        }

        DrawCircle(GetComponent<LineRenderer>(), planetRadius, planetColor, planetLineWidth, 32);

        var orbitGO = new GameObject("OrbitRing");
        orbitGO.transform.SetParent(transform, false);
        var orbitLr = orbitGO.AddComponent<LineRenderer>();
        var bodyLr = GetComponent<LineRenderer>();
        if (bodyLr != null) orbitLr.sharedMaterial = bodyLr.sharedMaterial;
        DrawCircle(orbitLr, orbitRadius, orbitRingColor, orbitLineWidth, 48);
    }

    private static void DrawCircle(LineRenderer lr, float radius, Color color, float width, int segments)
    {
        if (lr == null) return;
        lr.useWorldSpace = false;
        lr.loop = true;
        lr.positionCount = segments;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.startColor = color;
        lr.endColor = color;
        for (int i = 0; i < segments; i++)
        {
            float angle = 2f * Mathf.PI * i / segments;
            lr.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }
}
