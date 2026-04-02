using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuRoot;

    private void Awake()
    {
        if (menuRoot == null)
        {
            menuRoot = gameObject;
        }

        menuRoot.SetActive(true);
        GameEvents.OnGameStarted += HandleGameStarted;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
    }

    public void OnPlayPressed()
    {
        menuRoot.SetActive(false);
        GameEvents.StartGame();
    }

    private void HandleGameStarted()
    {
        menuRoot.SetActive(false);
    }
}
