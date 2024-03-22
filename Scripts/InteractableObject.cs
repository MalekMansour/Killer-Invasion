using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour
{
    public string interactText = "Interact";
    public TextMeshProUGUI interactTextUI;
    public float interactionRadius = 5f;

    private static List<InteractableObject> interactableObjects = new List<InteractableObject>();

    private void OnEnable()
    {
        interactableObjects.Add(this);
    }

    private void OnDisable()
    {
        interactableObjects.Remove(this);
    }

    private void Update()
    {
        UpdateInteractText();
    }

    private void UpdateInteractText()
    {
        float closestDistance = float.MaxValue;
        InteractableObject closestObject = null;

        foreach (InteractableObject obj in interactableObjects)
        {
            float distanceToPlayer = Vector3.Distance(obj.transform.position, Camera.main.transform.position);

            if (distanceToPlayer <= interactionRadius && distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestObject = obj;
            }
        }

        if (closestObject != null)
        {
            interactTextUI.gameObject.SetActive(true);
            interactTextUI.text = closestObject.interactText;
        }
        else
        {
            interactTextUI.gameObject.SetActive(false);
        }
    }
}
