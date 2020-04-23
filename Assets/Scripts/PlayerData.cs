using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public bool hasKey = false;

    private void Start()
    {
        GameManager.Instance.EventManager.OnItemsPick += GetKey;
    }

    void GetKey()
    {
        hasKey = true;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EventManager.OnItemsPick -= GetKey;
        }
    }
}
