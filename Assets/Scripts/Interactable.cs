using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual string InteractionMessage { get; }

    public virtual void Show() { }

    public virtual void Hide() { }

    public virtual void Interaction() { }
}
