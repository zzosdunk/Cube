using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static object _lock = new object();

    public enum GameState
    {
        PlayState,
        PauseState
    }

    public EventGameState OnGameStateChanges;

    private static GameManager _instance;

    private static bool applicationIsQuitting = false;

    public static GameManager Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    if (GameObject.Find("GameManager"))
                    {
                        GameObject g = GameObject.Find("GameManager");
                        if (g.GetComponent<GameManager>())
                        {
                            _instance = g.GetComponent<GameManager>();
                        }
                        else
                        {
                            _instance = g.AddComponent<GameManager>();
                        }
                    }
                    else
                    {
                        GameObject g = new GameObject();
                        g.name = "GameManager";
                        _instance = g.AddComponent<GameManager>();
                    }
                }
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    [SerializeField]
    private EventManager eventManager;
    public EventManager EventManager => eventManager;
    [SerializeField]
    private UIManager uiManager;
    public UIManager UIManager => uiManager;

    private GameState _currentGameState;
    public GameState CurrenGameState
    {
        get
        {
            return _currentGameState;
        }
        set
        {
            _currentGameState = value;
        }
    }

    private void Start()
    {
        _instance = this;
    }

    void UpdateGameState(GameState newState)
    {
        _currentGameState = newState;

        switch (_currentGameState)
        {
            case GameState.PlayState:
                Time.timeScale = 1.0f;
                break;
            case GameState.PauseState:
                Time.timeScale = 0f;
                break;
            default:
                break;
        }

        OnGameStateChanges.Invoke(_currentGameState);
    }

    public void ChangeGameState(GameState state)
    {
        UpdateGameState(state);
    }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
