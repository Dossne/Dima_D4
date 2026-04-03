using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PauseRestartButtonBinder : MonoBehaviour
{
    [SerializeField] private UIPauseMenu menu;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (menu == null)
        {
            menu = GetComponentInParent<UIPauseMenu>();
        }

        if (menu != null)
        {
            _button.onClick.AddListener(menu.OnRestartPressed);
        }
    }

    private void OnDestroy()
    {
        if (_button != null && menu != null)
        {
            _button.onClick.RemoveListener(menu.OnRestartPressed);
        }
    }
}
