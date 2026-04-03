using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIPauseButton : MonoBehaviour
{
    [SerializeField] private GameObject buttonRoot;
    [SerializeField] private GameStateManager stateManager;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (buttonRoot == null)
        {
            buttonRoot = gameObject;
        }

        _button.onClick.AddListener(HandleClick);
        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnPauseChanged += HandlePauseChanged;
        GameEvents.OnGameOver += HandleGameOver;
        buttonRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnPauseChanged -= HandlePauseChanged;
        GameEvents.OnGameOver -= HandleGameOver;
        _button.onClick.RemoveListener(HandleClick);
    }

    private void HandleGameStarted() => buttonRoot.SetActive(true);
    private void HandlePauseChanged(bool isPaused) => buttonRoot.SetActive(!isPaused);
    private void HandleGameOver() => buttonRoot.SetActive(false);

    private void HandleClick()
    {
        if (stateManager != null)
        {
            stateManager.TogglePause();
        }
    }
}
