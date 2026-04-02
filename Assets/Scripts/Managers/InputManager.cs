using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool _isGameActive;

    private void Awake()
    {
        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void Update()
    {
        if (!_isGameActive)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) || HasTouchBegan())
        {
            GameEvents.Tap();
        }
    }

    private void HandleGameStarted()
    {
        _isGameActive = true;
    }

    private void HandleGameOver()
    {
        _isGameActive = false;
    }

    private static bool HasTouchBegan()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }
}
