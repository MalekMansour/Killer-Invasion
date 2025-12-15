using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WifiListController : MonoBehaviour
{
    [Header("Wi-Fi Button")]
    public Button wifiButton;
    public RawImage wifiButtonImage;

    [Header("Bar-Icon Textures")]
    public Texture2D twoBarTexture;
    public Texture2D threeBarTexture;
    public Texture2D fourBarTexture;

    [Header("Network List UI (size = 10)")]
    public GameObject listPanel;        
    public TMP_Text[] titleTexts;       // 0–9
    public Button[] connectButtons;     // 0–9

    [Header("Password Popup UI (kept for compatibility, not used)")]
    public GameObject passwordPopup;
    public TMP_Text popupTitle;
    public TMP_InputField popupInput;
    public Button popupOk;
    public Button popupCancel;
    public TMP_Text feedbackText;       // used for transient messages

    [Header("Legacy / Compatibility")]
    public Wifi.WifiNetwork pendingNetwork; // kept so other scripts don't break
    public Wifi wifiManager;

    const float MESSAGE_DURATION = 1.6f;

    void Awake()
    {
        wifiManager = GetComponent<Wifi>();

        listPanel.SetActive(false);
        if (passwordPopup != null)
            passwordPopup.SetActive(false);

        // Open network list
        if (wifiButton != null)
            wifiButton.onClick.AddListener(() =>
            {
                PopulateList();
                listPanel.SetActive(true);
            });

        // Keep popup buttons wired for legacy scripts, won't be used
        if (popupCancel != null)
            popupCancel.onClick.AddListener(() =>
            {
                if (passwordPopup != null) passwordPopup.SetActive(false);
                pendingNetwork = null;
            });
        if (popupOk != null)
            popupOk.onClick.AddListener(OnPasswordSubmitted);
    }

    void PopulateList()
    {
        int realCount = wifiManager.networks.Count;

        // SLOT 0: free network
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

        // Other networks
        for (int i = 1; i < realCount; i++)
        {
            var net = wifiManager.networks[i]; // local copy for closure
            titleTexts[i].text = net.name;

            connectButtons[i].onClick.RemoveAllListeners();
            connectButtons[i].onClick.AddListener(() =>
            {
                ConnectToNetwork(net);
            });

            titleTexts[i].gameObject.SetActive(true);
            connectButtons[i].gameObject.SetActive(true);
        }

        // Hide unused slots
        for (int i = realCount; i < titleTexts.Length; i++)
        {
            titleTexts[i].gameObject.SetActive(false);
            connectButtons[i].gameObject.SetActive(false);
        }
    }

    // New behavior: instant connect if password is known, else transient error
    public void ConnectToNetwork(Wifi.WifiNetwork net)
    {
        if (net == null) return;

        pendingNetwork = net; // keep for legacy scripts

        if (net.isCracked)
        {
            listPanel.SetActive(false);
            wifiManager.StartConnection(net);
            UpdateWifiButtonIcon(net);
            StartCoroutine(ShowTransientMessage($"✔ Successfully connected to {net.name}", MESSAGE_DURATION));
            pendingNetwork = null;
        }
        else
        {
            StartCoroutine(ShowTransientMessage("❌ Incorrect / Unknown password", MESSAGE_DURATION));
        }
    }

    // Keep for compatibility with popup OK buttons
    void OnPasswordSubmitted()
    {
        if (pendingNetwork == null) return;

        if (popupInput != null && popupInput.text == pendingNetwork.password)
        {
            if (passwordPopup != null) passwordPopup.SetActive(false);
            if (listPanel != null) listPanel.SetActive(false);
            wifiManager.StartConnection(pendingNetwork);
            UpdateWifiButtonIcon(pendingNetwork);
            StartCoroutine(ShowTransientMessage("✔ Successfully connected", MESSAGE_DURATION));
            pendingNetwork = null;
        }
        else
        {
            if (feedbackText != null) feedbackText.text = "❌ Incorrect password!";
            if (popupInput != null) popupInput.ActivateInputField();
        }
    }

    IEnumerator ShowTransientMessage(string msg, float duration)
    {
        if (feedbackText == null) yield break;

        string previous = feedbackText.text;
        feedbackText.text = msg;

        yield return new WaitForSeconds(duration);

        if (feedbackText != null && feedbackText.text == msg)
            feedbackText.text = previous;
    }

    void UpdateWifiButtonIcon(Wifi.WifiNetwork net)
    {
        if (net == null || wifiButtonImage == null) return;

        var tex = net.bars == 2 ? twoBarTexture
                 : net.bars == 3 ? threeBarTexture
                 : fourBarTexture;
        wifiButtonImage.texture = tex;
    }
}
