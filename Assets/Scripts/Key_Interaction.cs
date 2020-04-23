using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Interaction : Interactable
{
    public override string InteractionMessage
    {
        get
        {
            string txt = "Press <b>F</b> to Pick up the key";
            return txt;
        }
    }
    public override void Show()
    {
        GameManager.Instance.UIManager.ShowInteractionObjectUI(InteractionMessage);
    }

    public override void Hide()
    {
        GameManager.Instance.UIManager.HideInteractionObjectUI();
    }

    public override void Interaction()
    {
        GameManager.Instance.EventManager.ItemPick();
        gameObject.SetActive(false);
    }
}
