
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Wifi : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text wifiListText;         // Text to display available networks
    public TMP_InputField inputField;     // User input for commands
    public TMP_Text outputText;           // Text to show cracking result

    public class WifiNetwork
    {
        public string name;
        public string bssid;
        public int power;
        public int beacons;
        public string enc = "WPA2";
        public string auth = "PSK";
        public int bars; // 2, 3, or 4 (hidden from player)
        public string password;
    }

    public List<WifiNetwork> networks = new List<WifiNetwork>();

    private string[] wifiNames = new string[]
    {
        "Adam's Office", "SmartHome8920", "Byte Me", "BeerLover69420", "Scott's Network",
        "Tell my Wi-Fi love her", "NextHome-5G", "xfinitywifi", "Mom click here for Wi-Fi"
    };

    private string[] passwordWords = new string[]
    {
        "coffee", "admin", "dragon", "hunter", "tsunami", "netgear", "sneaky", "shadow", "monkey", "kitty", "letmein", "accessdenied", "default", "welcome",
        "guest", "qwerty", "abc123", "login", "ball9", "iloveyou", "princess", "master", "adam", "qqqqqq", "drugaddict", "superman", "spoonmebaby", "batman", "trustno1",
        "123456", "69420", "987654", "111111", "222222", "333333", "password1", "admin123", "router", "modem123", "internet", "wifi4me", "hidden123", "mywifi", "youcantguess",
        "home123", "mypass", "secureme", "private", "dangerzone", "ucanthackme", "linker", "tp3096", "computerwifi", "office2026", "wireless", "network", "signalstrong", "hackmeifucan",
        "coolwifi", "schoolnet", "mommy", "daddy", "cyberzone", "ghostnet", "invisible", "zeroaccess", "fortress", "n3tw0rk", "rootaccess", "nodata", "onionnet", "internet4all",
        "homepasswordwifi", "vault", "nowifi4u", "wpa2secure", "topsecret", "adminpanel", "loginadmin", "spectrum", "version40", "giga123", "fiberfast", "hotspot", "mobiledata", "streamline",
        "blacknet", "undercover", "quickpass", "dangerzone", "nopassword", "696969", "420smoker", "2fast4u", "ilovebeer", "signalbooster", "networkboss", "techsupport", "insym", "computerwiz",
        "bazinga", "beerlover", "markzucc", "wiredin"
    };

    void Start()
    {
        GenerateWifis();
        DisplayWifis();

        if (inputField != null)
        {
            inputField.onSubmit.AddListener(HandleUserInput);
        }
    }

    void HandleUserInput(string input)
    {
        CrackCommand(input);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    void GenerateWifis()
    {
        networks.Clear();

        // Evenly assign 2, 3, 4 bars
        List<int> barsPool = new List<int> { 2, 2, 2, 3, 3, 3, 4, 4, 4 };
        ShuffleList(barsPool);

        for (int i = 0; i < 9; i++)
        {
            WifiNetwork wifi = new WifiNetwork
            {
                name = wifiNames[i],
                bssid = GenerateRandomBSSID(),
                power = UnityEngine.Random.Range(-90, -40),
                beacons = UnityEngine.Random.Range(1, 100),
                bars = barsPool[i],
                password = GenerateRandomPassword()
            };

            networks.Add(wifi);
        }
    }

    void DisplayWifis()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Name\t\t\t\tBSSID\t\t\t\tPWR\tBeacons\tENC\tAUTH");

        foreach (var net in networks)
        {
            sb.AppendLine($"{net.name}\t{net.bssid}\t{net.power}\t{net.beacons}\t{net.enc}\t{net.auth}");
        }

        if (wifiListText != null)
        {
            wifiListText.text = sb.ToString();
        }
        else
        {
            Debug.Log(sb.ToString());
        }
    }

    string GenerateRandomBSSID()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < 6; i++)
        {
            sb.Append(UnityEngine.Random.Range(0, 256).ToString("X2"));
            if (i < 5) sb.Append(":");
        }
        return sb.ToString();
    }

    string GenerateRandomPassword()
    {
        string word = passwordWords[UnityEngine.Random.Range(0, passwordWords.Length)];
        int number = UnityEngine.Random.Range(100, 999);
        return word + number;
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

    public void CrackCommand(string input)
    {
        if (!input.StartsWith("/crack ")) return;

        string bssid = input.Replace("/crack ", "").Trim();
        WifiNetwork target = networks.Find(n => n.bssid.ToLower() == bssid.ToLower());

        if (target == null)
        {
            outputText.text = "No Wi-Fi found with that BSSID.";
            return;
        }

        // Fake cracking challenge
        string fullCode = GenerateCrackCode();
        string missing = GetMissingChars(fullCode, out string display);

        System.Text.StringBuilder result = new System.Text.StringBuilder();
        result.AppendLine($"Cracking {target.name}...");
        result.AppendLine($"Code: {display}");
        result.AppendLine("Enter missing characters (in any order):");

        // (Auto-success simulation)
        result.AppendLine("Cracked successfully!");
        result.AppendLine($"Password: {target.password}");

        float loadTime = target.bars == 2 ? 12f : target.bars == 3 ? 6f : 3f;
        result.AppendLine($"This Wi-Fi loads websites in {loadTime} seconds.");

        if (outputText != null)
        {
            outputText.text = result.ToString();
        }
        else
        {
            Debug.Log(result.ToString());
        }
    }

    string GenerateCrackCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] code = new char[16];
        for (int i = 0; i < code.Length; i++)
        {
            code[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
        }
        return new string(code);
    }

    string GetMissingChars(string fullCode, out string display)
    {
        char[] code = fullCode.ToCharArray();
        List<int> indicesToHide = new List<int>();

        while (indicesToHide.Count < 4)
        {
            int index = UnityEngine.Random.Range(0, code.Length);
            if (!indicesToHide.Contains(index))
                indicesToHide.Add(index);
        }

        string missing = "";
        for (int i = 0; i < code.Length; i++)
        {
            if (indicesToHide.Contains(i))
            {
                missing += code[i];
                code[i] = '_';
            }
        }

        display = new string(code);
        return missing;
    }
}
