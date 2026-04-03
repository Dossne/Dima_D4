using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{
    [SerializeField] private int previewPointCount = 24;
    [SerializeField] private float previewDistanceMultiplier = 0.75f;
    [SerializeField] private Color startColor = new(0.6f, 0.32f, 1f, 0.8f);
    [SerializeField] private Color endColor = new(0.6f, 0.32f, 1f, 0f);
    [SerializeField] private float lineWidth = 0.04f;

    private LineRenderer _lineRenderer;
    private PlayerMovement _playerMovement;
    private PlayerOrbitController _orbitController;
    private Action _onTap;
    private Action _onGameStarted;
    private Action<Transform> _onPlanetLanded;
    private Action _onGameOver;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
        _orbitController = GetComponent<PlayerOrbitController>();

        ConfigureLineRenderer();

        _onTap = HandleTap;
        _onGameStarted = HandleGameStarted;
        _onPlanetLanded = HandlePlanetLanded;
        _onGameOver = HandleGameOver;

        GameEvents.OnTap += _onTap;
        GameEvents.OnGameStarted += _onGameStarted;
        GameEvents.OnPlanetLanded += _onPlanetLanded;
        GameEvents.OnGameOver += _onGameOver;

        HideTrajectory();
    }

    private void OnDestroy()
    {
        if (_onTap != null)
        {
            GameEvents.OnTap -= _onTap;
        }

        if (_onGameStarted != null)
        {
            GameEvents.OnGameStarted -= _onGameStarted;
        }

        if (_onPlanetLanded != null)
        {
            GameEvents.OnPlanetLanded -= _onPlanetLanded;
        }

        if (_onGameOver != null)
        {
            GameEvents.OnGameOver -= _onGameOver;
        }
    }

    private void Update()
    {
        if (!_lineRenderer.enabled)
        {
            return;
        }

        RefreshTrajectory();
    }

    private void HandleTap()
    {
        HideTrajectory();
    }

    private void HandleGameStarted()
    {
        ShowTrajectory();
    }

    private void HandlePlanetLanded(Transform _)
    {
        ShowTrajectory();
    }

    private void HandleGameOver()
    {
        HideTrajectory();
    }

    private void ConfigureLineRenderer()
    {
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.positionCount = 0;
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = 0f;
        _lineRenderer.startColor = startColor;
        _lineRenderer.endColor = endColor;
    }

    private void ShowTrajectory()
    {
        if (!CanDrawTrajectory())
        {
            HideTrajectory();
            return;
        }

        _lineRenderer.enabled = true;
        RefreshTrajectory();
    }

    private void HideTrajectory()
    {
        _lineRenderer.enabled = false;
        _lineRenderer.positionCount = 0;
    }

    private void RefreshTrajectory()
    {
        if (!CanDrawTrajectory())
        {
            HideTrajectory();
            return;
        }

        Transform playerTransform = transform;
        Vector3 pivotPosition = _orbitController.currentPivot.position;
        Vector2 radial = (Vector2)(playerTransform.position - pivotPosition);
        Vector2 tangent = Vector2.Perpendicular(radial.normalized);
        if (tangent == Vector2.zero)
        {
            tangent = Vector2.right;
        }

        float launchForce = _playerMovement != null ? _playerMovement.LaunchForce : 10f;
        float previewDistance = launchForce * previewDistanceMultiplier;
        Vector3 start = playerTransform.position;

        _lineRenderer.positionCount = previewPointCount;
        for (int i = 0; i < previewPointCount; i++)
        {
            float t = previewPointCount <= 1 ? 1f : (float)i / (previewPointCount - 1);
            Vector3 point = start + (Vector3)(tangent * (previewDistance * t));
            point.z = start.z;
            _lineRenderer.SetPosition(i, point);
        }
    }

    private bool CanDrawTrajectory()
    {
        return _orbitController != null && _orbitController.currentPivot != null;
    }
}

