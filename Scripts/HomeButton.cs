using UnityEngine;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    public GameObject websitesParent; // The parent object containing all the website GameObjects
    private GameObject currentActiveWebsite;

    void Start()
    {
        // Add a listener to the button click event
        Button homeButton = GetComponent<Button>();
        if (homeButton != null)
        {
            homeButton.onClick.AddListener(ToggleOffCurrentWebsite);
        }

        // Initially, no website is active
        currentActiveWebsite = null;
    }

    void Update()
    {
        // Update the current active website
        foreach (Transform website in websitesParent.transform)
        {
            if (website.gameObject.activeSelf)
            {
                currentActiveWebsite = website.gameObject;
                break;
            }
        }
    }

    void ToggleOffCurrentWebsite()
    {
        // Deactivate the currently active website
        if (currentActiveWebsite != null)
        {
            currentActiveWebsite.SetActive(false);
            currentActiveWebsite = null;
        }
    }
}

