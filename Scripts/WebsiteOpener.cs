using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class WebsiteOpener : MonoBehaviour
{
    public GameObject websitesParent; // Parent object containing all website screens
    public GameObject panel; // Button container
    public TMP_InputField urlInputField; // Shared URL bar
    public Image progressBarImage; // Blue progress fill image in URL bar

    public Dictionary<GameObject, Sprite> websiteThumbnails = new Dictionary<GameObject, Sprite>();
    private Dictionary<GameObject, GameObject> buttonToWebsiteMap = new Dictionary<GameObject, GameObject>();
    private List<GameObject> availableWebsites = new List<GameObject>();
    private string baseURL = "https://www.thedarkwiki.com/redirect/main/";

    void Start()
    {
        // Gather all available websites
        for (int i = 0; i < websitesParent.transform.childCount; i++)
        {
            GameObject website = websitesParent.transform.GetChild(i).gameObject;
            availableWebsites.Add(website);

            Sprite thumbnail = Resources.Load<Sprite>("Thumbnails/" + website.name);
            if (thumbnail != null)
            {
                websiteThumbnails[website] = thumbnail;
            }
        }

        // Set up button click events
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
            StartCoroutine(OpenWebsiteWithDelay(buttonToWebsiteMap[button]));
        }
        else if (availableWebsites.Count > 0)
        {
            int randomIndex = Random.Range(0, availableWebsites.Count);
            GameObject assignedWebsite = availableWebsites[randomIndex];
            availableWebsites.RemoveAt(randomIndex);

            buttonToWebsiteMap[button] = assignedWebsite;
            UpdateButtonThumbnail(button, assignedWebsite);
            StartCoroutine(OpenWebsiteWithDelay(assignedWebsite));
        }
        else
        {
            Debug.LogWarning("No more websites available to assign.");
        }
    }

    IEnumerator OpenWebsiteWithDelay(GameObject website)
    {
        // Set a new random URL
        string randomNumber = Random.Range(100000000, 999999999).ToString();
        urlInputField.text = baseURL + randomNumber + ".onion";

        // Reset progress bar
        progressBarImage.fillAmount = 0f;
        float duration = 7f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            progressBarImage.fillAmount = Mathf.Clamp01(timer / duration);
            yield return null;
        }

        // Hide all websites
        for (int i = 0; i < websitesParent.transform.childCount; i++)
        {
            websitesParent.transform.GetChild(i).gameObject.SetActive(false);
        }

        // Show selected one
        website.SetActive(true);
    }

    void UpdateButtonThumbnail(GameObject button, GameObject website)
    {
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
