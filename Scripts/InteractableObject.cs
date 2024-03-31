using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour
{
    public string interactText = "Interact";
    public TextMeshProUGUI interactTextUI;
    public float interactionRadius = 5f;
    public GameObject crosshair; 
    private static List<InteractableObject> interactableObjects = new List<InteractableObject>();
    private Vector3 originalCrosshairScale; 

    private void OnEnable()
    {
        interactableObjects.Add(this);
        originalCrosshairScale = crosshair.transform.localScale; 
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
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        float closestDistance = float.MaxValue;
        InteractableObject closestObject = null;

        foreach (InteractableObject obj in interactableObjects)
        {
            float distanceToPlayer = Vector3.Distance(obj.transform.position, Camera.main.transform.position);

            if (Physics.Raycast(mouseRay, out RaycastHit hit) && hit.collider.gameObject == obj.gameObject && distanceToPlayer <= interactionRadius)
            {
                closestDistance = distanceToPlayer;
                closestObject = obj;
                break;
            }
        }

        if (closestObject != null)
        {
            interactTextUI.gameObject.SetActive(true);
            interactTextUI.text = closestObject.interactText;
            crosshair.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        }
        else
        {
            interactTextUI.gameObject.SetActive(false);
            crosshair.transform.localScale = originalCrosshairScale;
        }
    }
}
