using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuRoot;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameStateManager stateManager;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (menuRoot == null)
        {
            menuRoot = gameObject;
        }

        if (canvasGroup == null)
        {
            canvasGroup = menuRoot.GetComponent<CanvasGroup>();
        }

        if (titleText != null)
        {
            titleText.text = "PAUSED";
        }

        SetVisible(false);
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

    public void OnResumePressed()
    {
        if (stateManager != null)
        {
            stateManager.SetPaused(false);
        }
    }

    public void OnRestartPressed()
    {
        if (stateManager != null)
        {
            stateManager.RestartGame();
            return;
        }

        SceneManager.LoadScene(0);
    }

    private void HandleGameStarted() => SetVisible(false);
    private void HandlePauseChanged(bool isPaused) => SetVisible(isPaused);
    private void HandleGameOver() => SetVisible(false);

    private void SetVisible(bool isVisible)
    {
        if (menuRoot != null && !menuRoot.activeSelf)
        {
            menuRoot.SetActive(true);
        }

        if (canvasGroup == null)
        {
            return;
        }

        canvasGroup.alpha = isVisible ? 1f : 0f;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }
}
