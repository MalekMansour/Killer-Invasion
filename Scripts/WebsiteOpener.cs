using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WebsiteOpener : MonoBehaviour
{
    public GameObject websitesParent; 
    public GameObject panel;

    public Dictionary<GameObject, Sprite> websiteThumbnails = new Dictionary<GameObject, Sprite>(); 

    private Dictionary<GameObject, GameObject> buttonToWebsiteMap = new Dictionary<GameObject, GameObject>();
    private List<GameObject> availableWebsites = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < websitesParent.transform.childCount; i++)
        {
            GameObject website = websitesParent.transform.GetChild(i).gameObject;
            availableWebsites.Add(website);

            // Assume thumbnails are named after websites for simplicity
            // You can manually assign these in the inspector if needed
            Sprite thumbnail = Resources.Load<Sprite>("Thumbnails/" + website.name);
            if (thumbnail != null)
            {
                websiteThumbnails[website] = thumbnail;
            }
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
                UpdateButtonThumbnail(button, assignedWebsite);
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

    void UpdateButtonThumbnail(GameObject button, GameObject website)
    {
        // Find the Image component under the button and update its sprite
        Image imageComponent = button.GetComponentInChildren<Image>();
        if (imageComponent != null && websiteThumbnails.ContainsKey(website))
        {
            imageComponent.sprite = websiteThumbnails[website];
        }
        else
        {
            Debug.LogWarning("Thumbnail not found for: " + website.name);
        }
    }
}
