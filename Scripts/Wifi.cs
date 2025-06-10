using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Wifi : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text wifiListText;
    public TMP_InputField inputField;
    public TMP_Text outputText;

    public class WifiNetwork
    {
        public string name;
        public string bssid;
        public int power;
        public int beacons;
        public string enc = "WPA2";
        public string auth = "PSK";
        public int bars; // 2, 3, or 4 (hidden)
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
        "coffee", "admin", "dragon", "hunter", "tsunami", "netgear", "sneaky", "shadow", "monkey", "kitty", "letmein",
        "accessdenied", "default", "welcome", "guest", "qwerty", "abc123", "login", "ball9", "iloveyou", "princess",
        "master", "adam", "qqqqqq", "drugaddict", "superman", "spoonmebaby", "batman", "trustno1", "123456", "69420",
        "987654", "111111", "222222", "333333", "password1", "admin123", "router", "modem123", "internet", "wifi4me",
        "hidden123", "mywifi", "youcantguess", "home123", "mypass", "secureme", "private", "dangerzone", "ucanthackme",
        "linker", "tp3096", "computerwifi", "office2026", "wireless", "network", "signalstrong", "hackmeifucan",
        "coolwifi", "schoolnet", "mommy", "daddy", "cyberzone", "ghostnet", "invisible", "zeroaccess", "fortress",
        "n3tw0rk", "rootaccess", "nodata", "onionnet", "internet4all", "homepasswordwifi", "vault", "nowifi4u",
        "wpa2secure", "topsecret", "adminpanel", "loginadmin", "spectrum", "version40", "giga123", "fiberfast",
        "hotspot", "mobiledata", "streamline", "blacknet", "undercover", "quickpass", "696969", "420smoker",
        "2fast4u", "ilovebeer", "signalbooster", "networkboss", "techsupport", "insym", "computerwiz", "bazinga",
        "beerlover", "markzucc", "wiredin"
    };

    private string pendingBSSID = null;
    private string currentMissing = "";

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
        if (!string.IsNullOrWhiteSpace(pendingBSSID))
        {
            if (ValidateMissingCharacters(input))
            {
                WifiNetwork target = networks.Find(n => n.bssid == pendingBSSID);
                outputText.text += $"\nCracked successfully!\nPassword: {target.password}";
                float loadTime = target.bars == 2 ? 12f : target.bars == 3 ? 6f : 3f;
                outputText.text += $"\nThis Wi-Fi loads websites in {loadTime} seconds.";
                pendingBSSID = null;
            }
            else
            {
                outputText.text += "\n❌ Incorrect. Try again.";
            }
            inputField.text = "";
            inputField.ActivateInputField();
            return;
        }

        CrackCommand(input);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    void GenerateWifis()
    {
        networks.Clear();
        List<int> barsPool = new List<int> { 2, 2, 2, 3, 3, 3, 4, 4, 4 };
        ShuffleList(barsPool);

        for (int i = 0; i < 9; i++)
        {
            WifiNetwork wifi = new WifiNetwork
            {
                name = wifiNames[i],
                bssid = GenerateShortBSSID(),
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
        string header = $"{"Name",-26} {"BSSID",-12} {"PWR",-5} {"Beacons",-8} {"ENC",-5} AUTH\n";
        string rows = "";
        foreach (var net in networks)
        {
            rows += string.Format("{0,-26} {1,-12} {2,-5} {3,-8} {4,-5} {5}\n",
                net.name, net.bssid, net.power, net.beacons, net.enc, net.auth);
        }
        wifiListText.text = header + rows;
    }

    string GenerateShortBSSID()
    {
        string[] hexDigits = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < 4; i++)
        {
            sb.Append(hexDigits[UnityEngine.Random.Range(0, hexDigits.Length)]);
            sb.Append(hexDigits[UnityEngine.Random.Range(0, hexDigits.Length)]);
            if (i < 3) sb.Append(":");
        }
        return sb.ToString();
    }

    string GenerateRandomPassword()
    {
        string word = passwordWords[UnityEngine.Random.Range(0, passwordWords.Length)];
        int number = UnityEngine.Random.Range(100, 999);
        return word + number;
    }

    void CrackCommand(string input)
    {
        if (!input.StartsWith("/crack ")) return;

        string bssid = input.Replace("/crack ", "").Trim();
        WifiNetwork target = networks.Find(n => n.bssid == bssid);

        if (target == null)
        {
            outputText.text = "No Wi-Fi found with that BSSID.";
            return;
        }

        string fullBlock = GenerateCrackBlock(out string missingChars);
        pendingBSSID = bssid;
        currentMissing = missingChars;

        outputText.text = $"Cracking {target.name}...\n";
        outputText.text += $"Block:\n{fullBlock}\n";
        outputText.text += "Enter missing characters (in any order):";
    }

    string GenerateCrackBlock(out string missingChars)
{
    List<char> numbers = new List<char>("123456789".ToCharArray());
    List<char> letters = new List<char>("ABCDEFG".ToCharArray());

    // Select 3 numbers and 3 letters to remove
    ShuffleList(numbers);
    ShuffleList(letters);

    List<char> removed = new List<char>();
    removed.AddRange(numbers.GetRange(0, 3));
    removed.AddRange(letters.GetRange(0, 3));

    // Create the remaining valid pool
    List<char> allowedPool = new List<char>();
    allowedPool.AddRange(numbers.GetRange(3, 6)); // 6 remaining numbers
    allowedPool.AddRange(letters.GetRange(3, 4)); // 4 remaining letters

    // Fill the block
    char[] block = new char[100];
    for (int i = 0; i < block.Length; i++)
    {
        block[i] = allowedPool[UnityEngine.Random.Range(0, allowedPool.Count)];
    }

    // Output the removed (missing) characters for validation
    missingChars = string.Join("", removed);
    return new string(block);
}


    bool ValidateMissingCharacters(string input)
    {
        foreach (char c in currentMissing)
        {
            if (!input.ToUpper().Contains(c.ToString())) return false;
        }
        return true;
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
}
