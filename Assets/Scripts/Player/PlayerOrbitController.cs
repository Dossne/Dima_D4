using UnityEngine;

public class PlayerOrbitController : MonoBehaviour
{
    [SerializeField] private float orbitSpeed = 120f;
    public Transform currentPivot;

    private Behaviour _playerMovement;

    private void Awake()
    {
        PlayerRegistry.Register(transform);
        _playerMovement = GetComponent("PlayerMovement") as Behaviour;
        GameEvents.OnTap += HandleTap;
        GameEvents.OnScoreUpdated += HandleScoreUpdated;
        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnPlanetLanded += HandlePlanetLanded;
    }

    private void OnDestroy()
    {
        PlayerRegistry.Unregister();
        GameEvents.OnTap -= HandleTap;
        GameEvents.OnScoreUpdated -= HandleScoreUpdated;
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnPlanetLanded -= HandlePlanetLanded;
    }

    private void Update()
    {
        if (!enabled || currentPivot == null)
        {
            return;
        }

        transform.RotateAround(currentPivot.position, Vector3.forward, orbitSpeed * Time.deltaTime);
    }

    private void HandleTap()
    {
        enabled = false;
        SetMovementEnabled(true);
    }

    private void HandleScoreUpdated(int _)
    {
        enabled = true;
        SetMovementEnabled(false);
    }

    private void HandleGameStarted()
    {
        enabled = true;
    }

    private void HandlePlanetLanded(Transform planet)
    {
        currentPivot = planet;
    }

    private void SetMovementEnabled(bool value)
    {
        if (_playerMovement != null)
        {
            _playerMovement.enabled = value;
        }
    }
}
