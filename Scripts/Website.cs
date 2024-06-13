using UnityEngine;
using TMPro;

public class Website : MonoBehaviour
{
    public TMP_InputField urlInputField; // The TextMeshPro Input Field to display the URL

    private string baseURL = "https://www.thedarkwiki.com/redirect/main/666/";

    void OnEnable()
    {
        GenerateRandomURL();
    }

    void GenerateRandomURL()
    {
        string randomNumber = Random.Range(100000000, 999999999).ToString(); // Generate a random 9-digit number
        string randomURL = baseURL + randomNumber + ".onion";
        urlInputField.text = randomURL;
    }
}
