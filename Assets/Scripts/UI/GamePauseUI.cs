using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_GamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_GameUnpaused;
        
        Hide();
    }
    
    void KitchenGameManager_GamePaused(object sender, EventArgs e)
    {
        Show();
    }
    
    void KitchenGameManager_GameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
