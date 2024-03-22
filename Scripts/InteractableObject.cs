using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    public string interactText = "Interact"; 
    public TextMeshProUGUI interactTextUI; 
    public float interactionRadius = 5f; 

    private void Start()
    {
        interactTextUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Camera.main.transform.position);

        if (distanceToPlayer <= interactionRadius)
        {
            interactTextUI.gameObject.SetActive(true);
            interactTextUI.text = interactText;
        }
        else
        {
            interactTextUI.gameObject.SetActive(false);
        }
    }
}

