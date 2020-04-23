using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject alertUI;
    public GameObject interactionMessage;
    public Text message;

    private void Awake()
    {
        GameManager.Instance.OnGameStateChanges.AddListener(GameStateUI);
    }
    
    void GameStateUI(GameManager.GameState currentState)
    {
        switch (currentState)
        {
            case GameManager.GameState.PlayState:
                pauseUI.SetActive(false);
                break;
            case GameManager.GameState.PauseState:
                pauseUI.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ShowInteractionObjectUI(string text)
    {
        interactionMessage.SetActive(true);
        message.text = text;
    }

    public void HideInteractionObjectUI()
    {
        interactionMessage.SetActive(false);
    }

    public void ShowInteractionWorldUI()
    {
        alertUI.SetActive(true);
    }

    public void HideInteractionWorldUI()
    {
        alertUI.SetActive(false);
    }
}
