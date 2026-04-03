using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private readonly List<RaycastResult> _raycastResults = new();
    private bool _isGameActive;
    private bool _isPaused;

    private void Awake()
    {
        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnPauseChanged += HandlePauseChanged;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnPauseChanged -= HandlePauseChanged;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void Update()
    {
        if (!_isGameActive || _isPaused)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !IsTapOnButton(Input.mousePosition))
        {
            GameEvents.Tap();
            return;
        }

        if (HasTouchBegan() && !IsTapOnButton(Input.GetTouch(0).position))
        {
            GameEvents.Tap();
        }
    }

    private void HandleGameStarted()
    {
        _isGameActive = true;
        _isPaused = false;
    }

    private void HandlePauseChanged(bool isPaused)
    {
        _isPaused = isPaused;
    }

    private void HandleGameOver()
    {
        _isGameActive = false;
        _isPaused = false;
    }

    private static bool HasTouchBegan()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    private bool IsTapOnButton(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
        {
            return false;
        }

        _raycastResults.Clear();
        var eventData = new PointerEventData(EventSystem.current) { position = screenPosition };
        EventSystem.current.RaycastAll(eventData, _raycastResults);

        for (int i = 0; i < _raycastResults.Count; i++)
        {
            if (_raycastResults[i].gameObject.GetComponentInParent<Button>() != null)
            {
                return true;
            }
        }

        return false;
    }
}
