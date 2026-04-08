using UnityEngine;

public class OrbitCaptureSmoother : MonoBehaviour
{
    [SerializeField] private float captureDuration = 0.22f;

    private Transform _pivot;
    private Vector2 _radial;
    private float _startRadius;
    private float _targetRadius;
    private float _orbitDirection;
    private float _orbitSpeed;
    private float _progress = 1f;

    public bool IsCapturing => _progress < 1f;

    public void BeginCapture(Transform pivot, float orbitRadius, float orbitDirection, float orbitSpeed)
    {
        _pivot = pivot;
        _radial = transform.position - pivot.position;
        if (_radial == Vector2.zero) _radial = Vector2.left;
        _startRadius = _radial.magnitude;
        _targetRadius = orbitRadius;
        _orbitDirection = orbitDirection;
        _orbitSpeed = orbitSpeed;
        _progress = 0f;
    }

    public void Tick()
    {
        if (!IsCapturing) return;
        _progress = Mathf.MoveTowards(_progress, 1f, Time.deltaTime / captureDuration);
        _radial = Quaternion.Euler(0f, 0f, _orbitSpeed * _orbitDirection * Time.deltaTime) * _radial.normalized;
        float radius = Mathf.Lerp(_startRadius, _targetRadius, _progress);
        transform.position = _pivot.position + (Vector3)(_radial * radius);
    }

    public Vector2 GetRadial(Vector3 pivotPosition)
    {
        return (transform.position - pivotPosition).normalized;
    }
}
