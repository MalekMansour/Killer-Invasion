using UnityEngine;
using TMPro;

public class Website : MonoBehaviour
{
    public TMP_InputField urlInputField; 

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
