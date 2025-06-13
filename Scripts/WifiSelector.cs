using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WifiListController : MonoBehaviour
{
    [Header("Wi-Fi Button")]
    public Button wifiButton;           // The icon/button you click to open the list
    public RawImage wifiButtonImage;    // The RawImage on that button

    [Header("Bar-Icon Textures")]
    public Texture2D twoBarTexture;
    public Texture2D threeBarTexture;
    public Texture2D fourBarTexture;

    [Header("Network List UI (size = 10)")]
    public GameObject listPanel;        // Parent of your 10 entries
    public TMP_Text[] titleTexts;       // 0–9
    public Button[]   connectButtons;   // 0–9

    [Header("Password Popup UI")]
    public GameObject   passwordPopup;
    public TMP_Text     popupTitle;
    public TMP_InputField popupInput;
    public Button       popupOk;
    public Button       popupCancel;
    public TMP_Text     feedbackText;

    Wifi.WifiNetwork pendingNetwork;
    Wifi             wifiManager;

    void Awake()
    {
        wifiManager = GetComponent<Wifi>();

        // Hide at start
        listPanel.SetActive(false);
        passwordPopup.SetActive(false);

        // Wi-Fi icon opens the list
        wifiButton.onClick.AddListener(() =>
        {
            PopulateList();
            listPanel.SetActive(true);
        });

        // Popup Cancel
        popupCancel.onClick.AddListener(() =>
        {
            passwordPopup.SetActive(false);
            pendingNetwork = null;
        });
        // Popup OK
        popupOk.onClick.AddListener(OnPasswordSubmitted);
    }

    void PopulateList()
{
    int realCount = wifiManager.networks.Count; // e.g. 9

    // --- SLOT 0: current network (2-bar, free connect) ---
    titleTexts[0].text = wifiManager.networks[0].name;
    connectButtons[0].onClick.RemoveAllListeners();
    connectButtons[0].onClick.AddListener(() =>
    {
        listPanel.SetActive(false);
        wifiManager.StartConnection(wifiManager.networks[0]);
        UpdateWifiButtonIcon(wifiManager.networks[0]);
    });
    titleTexts[0].gameObject.SetActive(true);
    connectButtons[0].gameObject.SetActive(true);

    // --- SLOTS 1 .. realCount-1: other networks with passwords ---
    for (int i = 1; i < realCount; i++)
    {
        var net = wifiManager.networks[i];
        titleTexts[i].text = net.name;

        connectButtons[i].onClick.RemoveAllListeners();
        connectButtons[i].onClick.AddListener(() =>
        {
            pendingNetwork = net;
            popupTitle.text   = $"Enter password for {net.name}";
            popupInput.text   = "";
            feedbackText.text = "";
            passwordPopup.SetActive(true);
            popupInput.ActivateInputField();
        });

        titleTexts[i].gameObject.SetActive(true);
        connectButtons[i].gameObject.SetActive(true);
    }

    // --- HIDE any leftover UI slots beyond realCount-1 ---
    // e.g. if realCount=9, we used slots 0..8; hide slot 9 (index 9)
    for (int i = realCount; i < titleTexts.Length; i++)
    {
        titleTexts[i].gameObject.SetActive(false);
        connectButtons[i].gameObject.SetActive(false);
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
            UpdateWifiButtonIcon(pendingNetwork);
            pendingNetwork = null;
        }
        else
        {
            feedbackText.text = "❌ Incorrect password!";
            popupInput.ActivateInputField();
        }
    }

    void UpdateWifiButtonIcon(Wifi.WifiNetwork net)
    {
        var tex = net.bars == 2
            ? twoBarTexture
            : net.bars == 3
                ? threeBarTexture
                : fourBarTexture;
        wifiButtonImage.texture = tex;
    }
}
