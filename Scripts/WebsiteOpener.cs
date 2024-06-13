using UnityEngine;
using UnityEngine.UI;

public class WebsiteOpener : MonoBehaviour
{
    public GameObject websitesParent; // The parent object containing all the website GameObjects
    public GameObject panel; // The parent object containing the 100 clickable website GameObjects

    private GameObject[] websites;

    void Start()
    {
        // Get all the website GameObjects under the websitesParent
        websites = new GameObject[websitesParent.transform.childCount];
        for (int i = 0; i < websitesParent.transform.childCount; i++)
        {
            websites[i] = websitesParent.transform.GetChild(i).gameObject;
            websites[i].SetActive(false); // Make sure all websites are initially inactive
        }

        // Assign click listeners to each of the 100 website buttons under panel
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            int index = i; // Capture the index for the lambda expression
            Button button = panel.transform.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OpenRandomWebsite);
            }
            else
            {
                Debug.LogWarning("No Button component found on: " + panel.transform.GetChild(i).name);
            }
        }
    }

    void OpenRandomWebsite()
    {
        // Deactivate all websites first
        foreach (GameObject website in websites)
        {
            website.SetActive(false);
        }

        // Activate a random website
        int randomIndex = Random.Range(0, websites.Length);
        websites[randomIndex].SetActive(true);
    }
}

