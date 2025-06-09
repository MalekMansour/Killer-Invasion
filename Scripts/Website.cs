using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Website : MonoBehaviour
{
    public TMP_InputField urlInputField;  // The URL text field (visible in browser UI)
    public Image progressBarImage;        // The blue loading bar inside the input field
    public GameObject thisWebsiteScreen;  // This specific website's screen (the child to show)

    private string baseURL = "https://www.thedarkwiki.com/redirect/main/";
    private float loadDuration = 7f;

    void OnEnable()
    {
        GenerateRandomURL();
        StartCoroutine(LoadWebsiteScreen());
    }

    void GenerateRandomURL()
    {
        string randomNumber = Random.Range(100000000, 999999999).ToString();
        string randomURL = baseURL + randomNumber + ".onion";
        urlInputField.text = randomURL;
    }

    IEnumerator LoadWebsiteScreen()
    {
        float timer = 0f;
        progressBarImage.fillAmount = 0f;

        // Animate progress bar over 7 seconds
        while (timer < loadDuration)
        {
            timer += Time.deltaTime;
            progressBarImage.fillAmount = Mathf.Clamp01(timer / loadDuration);
            yield return null;
        }

        // Once loaded, activate the website content
        if (thisWebsiteScreen != null)
            thisWebsiteScreen.SetActive(true);
    }
}
