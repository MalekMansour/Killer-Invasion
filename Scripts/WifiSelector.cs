using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WifiSelector : MonoBehaviour
{
    [Header("UI References")]
    public Button wifiIconButton;       // The Wi-Fi icon button you click
    public GameObject listPanel;        // Panel containing the list of networks
    public Transform listContent;       // Content container for list entries
    public GameObject entryPrefab;      // Prefab with: Icon (Image), Name (TMP_Text), Connect (Button)
    public GameObject passwordPopup;    // Popup panel for entering password
    public TMP_Text popupTitleText;     // “Enter password for …”
    public TMP_InputField popupInput;   // User types password here
    public Button popupConfirmButton;   // “OK” button
    public Button popupCancelButton;    // “Cancel” button

    [Header("Icons")]
    public Sprite homeIcon;             // Icon for your own network
    public Sprite twoBarIcon;
    public Sprite threeBarIcon;
    public Sprite fourBarIcon;

    [Header("Manager")]
    public Wifi wifiManager;            // Reference to your existing Wifi script

    private Wifi.WifiNetwork selectedNetwork;

    void Start()
    {
        // Hide UI initially
        listPanel.SetActive(false);
        passwordPopup.SetActive(false);

        // Hook up events
        wifiIconButton.onClick.AddListener(ShowList);
        popupCancelButton.onClick.AddListener(() => passwordPopup.SetActive(false));
        popupConfirmButton.onClick.AddListener(OnPasswordConfirmed);
    }

    void ShowList()
    {
        // Clear old entries
        foreach (Transform t in listContent) Destroy(t.gameObject);

        // Add "Weak Home Network" (free)
        CreateEntry(
            name: "Weak Home Network",
            net: null,
            barIcon: homeIcon,
            freeToConnect: true
        );

        // Add each real network
        foreach (var net in wifiManager.networks)
        {
            Sprite icon = net.bars == 2 ? twoBarIcon
                        : net.bars == 3 ? threeBarIcon
                        : fourBarIcon;

            CreateEntry(
                name: net.name,
                net: net,
                barIcon: icon,
                freeToConnect: false
            );
        }

        listPanel.SetActive(true);
    }

    void CreateEntry(string name, Wifi.WifiNetwork net, Sprite barIcon, bool freeToConnect)
    {
        // Instantiate prefab under the listContent
        var entry = Instantiate(entryPrefab, listContent);
        
        // Set icon
        entry.transform.Find("Icon").GetComponent<Image>().sprite = barIcon;
        
        // Set name
        entry.transform.Find("Name").GetComponent<TMP_Text>().text = name;

        // Configure Connect button
        var btn = entry.transform.Find("ConnectButton").GetComponent<Button>();
        btn.GetComponentInChildren<TMP_Text>().text = "Connect";
        if (freeToConnect)
        {
            btn.onClick.AddListener(() => {
                listPanel.SetActive(false);
                wifiManager.StartConnection(net); // free, so directly connect
            });
        }
        else
        {
            btn.onClick.AddListener(() => {
                selectedNetwork = net;
                popupTitleText.text = $"Enter password for {net.name}";
                popupInput.text = "";
                passwordPopup.SetActive(true);
            });
        }
    }

    void OnPasswordConfirmed()
    {
        if (popupInput.text == selectedNetwork.password)
        {
            passwordPopup.SetActive(false);
            listPanel.SetActive(false);
            wifiManager.StartConnection(selectedNetwork);
        }
        else
        {
            feedbackText.text = "❌ Incorrect password!";
        }
    }
}
