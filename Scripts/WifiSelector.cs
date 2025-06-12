using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WifiSelector : MonoBehaviour
{
    [Header("UI References")]
    public Button wifiIconButton;    // Your on-screen Wi-Fi icon
    public GameObject listPanel;     // Panel containing the scroll-view
    public Transform listContent;    // Where entries get instantiated
    public GameObject entryPrefab;   // Prefab: [Icon(Image) | Name(TMP) | Connect(Button)]
    
    public GameObject passwordPopup;
    public TMP_Text popupTitle;
    public TMP_InputField popupInput;
    public Button popupOk, popupCancel;
    public TMP_Text feedbackText;    // Reuse your feedback text for error messages

    [Header("Signal Icons")]
    public Sprite homeIcon, twoBarIcon, threeBarIcon, fourBarIcon;

    Wifi wifiManager;
    Wifi.WifiNetwork pendingNetwork;

    void Awake()
    {
        wifiManager = GetComponent<Wifi>();
        listPanel.SetActive(false);
        passwordPopup.SetActive(false);

        wifiIconButton.onClick.AddListener(ShowList);
        popupCancel.onClick.AddListener(() => passwordPopup.SetActive(false));
        popupOk.onClick.AddListener(OnPasswordSubmitted);
    }

    void ShowList()
    {
        // clear old entries
        foreach (Transform t in listContent) Destroy(t.gameObject);

        // 1) Home network (free)
        CreateEntry(
            icon: homeIcon,
            displayName: "Weak Home Network",
            net: null,
            freeConnect: true
        );

        // 2) Real networks
        foreach (var net in wifiManager.networks)
        {
            Sprite icon = net.bars == 2 ? twoBarIcon
                        : net.bars == 3 ? threeBarIcon
                        : fourBarIcon;

            CreateEntry(icon, net.name, net, false);
        }

        listPanel.SetActive(true);
    }

    void CreateEntry(Sprite icon, string displayName, Wifi.WifiNetwork net, bool freeConnect)
    {
        var go = Instantiate(entryPrefab, listContent);
        go.transform.Find("Icon").GetComponent<Image>().sprite = icon;
        go.transform.Find("Name").GetComponent<TMP_Text>().text = displayName;
        var btn = go.transform.Find("ConnectButton").GetComponent<Button>();
        btn.GetComponentInChildren<TMP_Text>().text = "Connect";

        if (freeConnect)
        {
            btn.onClick.AddListener(() =>
            {
                listPanel.SetActive(false);
                wifiManager.StartConnection(null);
            });
        }
        else
        {
            btn.onClick.AddListener(() =>
            {
                pendingNetwork = net;
                popupTitle.text = $"Enter password for {net.name}";
                popupInput.text = "";
                passwordPopup.SetActive(true);
            });
        }
    }

    void OnPasswordSubmitted()
    {
        if (pendingNetwork == null) return;

        if (popupInput.text == pendingNetwork.password)
        {
            passwordPopup.SetActive(false);
            listPanel.SetActive(false);
            wifiManager.StartConnection(pendingNetwork);
        }
        else
        {
            feedbackText.text = "❌ Incorrect password!";
        }
    }
}
