using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestartButtonBinder : MonoBehaviour
{
    [SerializeField] private UIGameOver gameOver;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (gameOver == null)
        {
            gameOver = GetComponentInParent<UIGameOver>();
        }

        if (_button != null && gameOver != null)
        {
            _button.onClick.AddListener(gameOver.OnRestartPressed);
        }
    }

    private void OnDestroy()
    {
        if (_button != null && gameOver != null)
        {
            _button.onClick.RemoveListener(gameOver.OnRestartPressed);
        }
    }
}