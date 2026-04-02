using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButtonBinder : MonoBehaviour
{
    [SerializeField] private UIMainMenu menu;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (menu == null)
        {
            menu = GetComponentInParent<UIMainMenu>();
        }

        if (_button != null && menu != null)
        {
            _button.onClick.AddListener(menu.OnPlayPressed);
        }
    }

    private void OnDestroy()
    {
        if (_button != null && menu != null)
        {
            _button.onClick.RemoveListener(menu.OnPlayPressed);
        }
    }
}