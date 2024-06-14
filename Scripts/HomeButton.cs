using UnityEngine;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    public GameObject websitesParent; // The parent object containing all the website GameObjects

    void Start()
    {
        // Get the button component attached to this GameObject
        Button button = GetComponent<Button>();

        // Ensure there's a button component and add a listener to its click event
        if (button != null)
        {
            button.onClick.AddListener(ToggleOffAllWebsites);
        }
        else
        {
            Debug.LogWarning("No Button component found on: " + gameObject.name);
        }
    }

    void ToggleOffAllWebsites()
    {
        // Loop through each child of the websitesParent and deactivate them
        for (int i = 0; i < websitesParent.transform.childCount; i++)
        {
            websitesParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
