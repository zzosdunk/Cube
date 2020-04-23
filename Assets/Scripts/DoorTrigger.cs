using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public int id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerData>().hasKey)
        {
            GameManager.Instance.EventManager.DoorEnter(id);
        }
        else
        {
            GameManager.Instance.UIManager.ShowInteractionWorldUI();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerData>().hasKey)
        {
            GameManager.Instance.EventManager.DoorExit(id);
        }
        else
        {
            GameManager.Instance.UIManager.HideInteractionWorldUI();
        }
    }
}
