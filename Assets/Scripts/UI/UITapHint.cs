using TMPro;
using UnityEngine;

public class UITapHint : MonoBehaviour
{
    [SerializeField] private GameObject hintRoot;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float pulseSpeed = 2.5f;
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 1f;

    private bool _hasShown;
    private bool _isVisible;

    private void Awake()
    {
        if (hintRoot == null)
        {
            hintRoot = gameObject;
        }

        hintRoot.SetActive(false);

        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnTap += HandleTap;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnTap -= HandleTap;
    }

    private void Update()
    {
        if (!_isVisible || canvasGroup == null)
        {
            return;
        }

        float t = Mathf.PingPong(Time.unscaledTime * pulseSpeed, 1f);
        canvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, t);
    }

    private void HandleGameStarted()
    {
        if (_hasShown)
        {
            return;
        }

        _hasShown = true;
        _isVisible = true;

        if (hintText != null)
        {
            hintText.text = "TAP";
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = maxAlpha;
        }

        hintRoot.SetActive(true);
    }

    private void HandleTap()
    {
        if (!_isVisible)
        {
            return;
        }

        _isVisible = false;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        hintRoot.SetActive(false);
    }
}
