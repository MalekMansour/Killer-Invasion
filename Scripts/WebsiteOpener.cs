using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WebsiteOpener : MonoBehaviour
{
    public GameObject websitesParent; // Parent object containing all website GameObjects
    public GameObject panel; // Parent object containing all the buttons

    private Dictionary<GameObject, GameObject> buttonToWebsiteMap = new Dictionary<GameObject, GameObject>();
    private List<GameObject> availableWebsites = new List<GameObject>();

    void Start()
    {
        // Initialize the available websites list with all children of websitesParent
        for (int i = 0; i < websitesParent.transform.childCount; i++)
        {
            availableWebsites.Add(websitesParent.transform.GetChild(i).gameObject);
        }

        // Assign a click listener to each button under the panel
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            GameObject button = panel.transform.GetChild(i).gameObject;
            Button btnComponent = button.GetComponent<Button>();

            if (btnComponent != null)
            {
                btnComponent.onClick.AddListener(() => OnButtonClick(button));
            }
            else
            {
                Debug.LogWarning("No Button component found on: " + button.name);
            }
        }
    }

    void OnButtonClick(GameObject button)
    {
        if (buttonToWebsiteMap.ContainsKey(button))
        {
            // Open the already assigned website
            OpenWebsite(buttonToWebsiteMap[button]);
        }
        else
        {
            if (availableWebsites.Count > 0)
            {
                // Assign a random website to the button
                int randomIndex = Random.Range(0, availableWebsites.Count);
                GameObject assignedWebsite = availableWebsites[randomIndex];
                availableWebsites.RemoveAt(randomIndex);

                buttonToWebsiteMap[button] = assignedWebsite;
                OpenWebsite(assignedWebsite);
            }
            else
            {
                Debug.LogWarning("No more websites available to assign.");
            }
        }
    }

    void OpenWebsite(GameObject website)
    {
        // Deactivate all websites first
        for (int i = 0; i < websitesParent.transform.childCount; i++)
        {
            websitesParent.transform.GetChild(i).gameObject.SetActive(false);
        }

        // Activate the assigned website
        website.SetActive(true);
    }
}
