using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventGameState : UnityEvent<GameManager.GameState> { }

public class EventManager : MonoBehaviour
{
    
    public event Action OnItemsPick;
    public void ItemPick()
    {
        OnItemsPick?.Invoke();
    }

    public event Action<int> OnDoorEnter;
    public void DoorEnter(int id)
    {
        OnDoorEnter?.Invoke(id);
    }

    public event Action<int> OnDoorExit;
    public void DoorExit(int id)
    {
        OnDoorExit?.Invoke(id);
    }
}
