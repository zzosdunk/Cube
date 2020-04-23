using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public List<Material> doorMaterials;
    public int id;
    private bool isUp;
    private Vector2 initialPos;

    private void Start()
    {
        GameManager.Instance.EventManager.OnDoorEnter += OnDoorOpen;
        GameManager.Instance.EventManager.OnDoorExit += OnDoorClose;
        GameManager.Instance.EventManager.OnItemsPick += DoorCanOpen;

        initialPos = transform.position;
        gameObject.GetComponent<Renderer>().sharedMaterial = doorMaterials[1];
    }

    void OnDoorOpen(int id)
    {
        if (id == this.id)
        {
            isUp = true;
            StartCoroutine(MoveDoor(isUp));
        }
    }

    void OnDoorClose(int id)
    {
        if (id == this.id)
        {
            isUp = false;
            StartCoroutine(MoveDoor(isUp));
        }
    }

    void DoorCanOpen()
    {
        gameObject.GetComponent<Renderer>().sharedMaterial = doorMaterials[0];
    }

    IEnumerator MoveDoor(bool isOpen)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos;

        endPos = isUp ? new Vector2(transform.position.x, transform.position.y + 2) : initialPos;

        float progressTime = 0f;
        float animTime = 0.5f;

        while (progressTime < animTime)
        {
            progressTime += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, endPos, progressTime);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EventManager.OnDoorEnter -= OnDoorOpen;
            GameManager.Instance.EventManager.OnDoorExit -= OnDoorClose;
            GameManager.Instance.EventManager.OnItemsPick -= DoorCanOpen;
        }
    }
}
