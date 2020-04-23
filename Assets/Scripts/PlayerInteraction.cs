using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public delegate void EventHandler(GameObject obj);
    public static event EventHandler OnInteracted;

    public static void Interacted(GameObject obj)
    {
        if (OnInteracted != null)
            OnInteracted.Invoke(obj);
    }

    public float InteractionDistance = 3.0f;

    public KeyCode InteractionKey = KeyCode.F;

    public RaycastHit Hit;

    private Interactable LastInteractable;

    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out Hit, InteractionDistance))
        {
            if (Hit.collider.GetComponentInParent<Interactable>() && Hit.collider.GetComponentInParent<Interactable>().enabled)
            {
                LastInteractable = Hit.collider.GetComponentInParent<Interactable>();

                Hit.collider.GetComponentInParent<Interactable>().Show();

                if (Input.GetKeyUp(InteractionKey))
                {
                    Interacted(Hit.collider.gameObject);

                    Hit.collider.GetComponentInParent<Interactable>().Interaction();

                    LastInteractable.Hide();
                }
            }
            else
                if (LastInteractable != null)
                LastInteractable.Hide();
        }
        else
            if (LastInteractable != null)
            LastInteractable.Hide();
    }
}
