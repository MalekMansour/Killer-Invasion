using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class WebsiteOpener : MonoBehaviour
{
    public GameObject websitesParent;             // Parent containing all website screens
    public GameObject panel;                      // Parent containing all the website buttons
    public TMP_InputField urlInputField;          // The fake browser URL bar
    public RawImage progressBar;                  // The blue RawImage loading bar

    private string baseURL = "https://www.thedarkwiki.com/redirect/main/";

    private Dictionary<GameObject, Sprite> websiteThumbnails = new Dictionary<GameObject, Sprite>();
    private Dictionary<GameObject, GameObject> buttonToWebsiteMap = new Dictionary<GameObject, GameObject>();
    private List<GameObject> availableWebsites = new List<GameObject>();
    private Coroutine loadingCoroutine = null;

    void Start()
    {
        // Collect all website screens
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

        // Assign buttons
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
        GameObject assignedWebsite;

        if (buttonToWebsiteMap.ContainsKey(button))
        {
            assignedWebsite = buttonToWebsiteMap[button];
        }
        else if (availableWebsites.Count > 0)
        {
            int randomIndex = Random.Range(0, availableWebsites.Count);
            assignedWebsite = availableWebsites[randomIndex];
            availableWebsites.RemoveAt(randomIndex);

            buttonToWebsiteMap[button] = assignedWebsite;
            UpdateButtonThumbnail(button, assignedWebsite);
        }
        else
        {
            Debug.LogWarning("No more websites available to assign.");
            return;
        }

        // Cancel any current loading coroutine
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
        }

        // Start loading the selected website
        loadingCoroutine = StartCoroutine(OpenWebsiteWithDelay(assignedWebsite));
    }

    IEnumerator OpenWebsiteWithDelay(GameObject website)
    {
        // Set a random URL
        string randomNumber = Random.Range(100000000, 999999999).ToString();
        urlInputField.text = baseURL + randomNumber + ".onion";

        // Show and reset progress bar
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
        }

        float duration = 12f;
        float timer = 0f;

        float targetWidth = urlInputField.GetComponent<RectTransform>().rect.width * 2f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            float currentWidth = targetWidth * progress;

            if (progressBar != null)
            {
                progressBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);
            }

            yield return null;
        }

        // Instantly hide the progress bar
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }

        // Shrink in background
        float shrinkSpeed = 5000f;
        float width = progressBar.rectTransform.rect.width;

        while (width > 1f)
        {
            width -= Time.deltaTime * shrinkSpeed;
            progressBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(0f, width));
            yield return null;
        }

        progressBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);

        // Hide all websites
        for (int i = 0; i < websitesParent.transform.childCount; i++)
        {
            websitesParent.transform.GetChild(i).gameObject.SetActive(false);
        }

        // Show the selected website
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
