using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wifi : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text wifis;
    public TMP_Text feedbackText;

    public class WifiNetwork
    {
        public string name;
        public string bssid;
        public int power;
        public int beacons;
        public string enc = "WPA2";
        public string auth = "PSK";
        public int bars;
        public string password;
        public int failedAttempts = 0;
        public bool isBlocked = false;
        public bool isCracked = false;
    }

    private List<WifiNetwork> networks = new List<WifiNetwork>();

    private string[] wifiNames = new string[]
    {
        "Adam's Office", "SmartHome8920", "Byte Me", "BeerLover69420", "Scott's Network",
        "Tell my Wi-Fi love her", "NextHome-5G", "xfinitywifi", "Mom click here for Wi-Fi"
    };

    private string[] passwordWords = new string[]
    {
        "coffee", "admin", "dragon", "hunter", "tsunami", "netgear", "sneaky", "shadow", "monkey", "kitty",
        "letmein", "default", "welcome", "guest", "qwerty", "abc123", "login", "ball9", "iloveyou", "princess",
        "master", "adam", "superman", "batman", "trustno1", "password1", "admin123", "router", "internet",
        "wifi4me", "hidden123", "mywifi", "youcantguess", "mypass", "secureme", "private", "dangerzone",
        "fortress", "n3tw0rk", "rootaccess", "onionnet", "vault", "nowifi4u", "topsecret", "adminpanel",
        "streamline", "blacknet", "undercover", "quickpass", "wiredin", "420smoker", "2fast4u", "signalbooster"
    };

    private WifiNetwork currentTarget = null;
    private string currentMissing = "";
    private string currentCrackBlock = "";

    void Start()
    {
        GenerateWifis();
        DisplayWifis();
        inputField.onSubmit.AddListener(HandleUserInput);
    }

    void HandleUserInput(string input)
    {
        input = input.Trim();
        feedbackText.text = "";

        if (currentTarget != null)
        {
            // Only allow password attempt input during cracking
            if (ValidateMissingCharacters(input))
            {
                currentTarget.isCracked = true;
                string signal = currentTarget.bars == 2 ? "weak" : currentTarget.bars == 3 ? "moderate" : "strong";
                wifis.text = $"✅ Cracked successfully!\nPassword: {currentTarget.password}\nSignal: {signal}\n\nType /home to return.";
                currentTarget = null;
            }
            else
            {
                currentTarget.failedAttempts++;
                if (currentTarget.failedAttempts >= 2)
                {
                    currentTarget.isBlocked = true;
                    feedbackText.text = "❌ Incorrect again. This network is now blocked.\n\nType /home to return.";
                    currentTarget = null;
                }
                else
                {
                    feedbackText.text = "❌ Incorrect. Try again.";
                    wifis.text = currentCrackBlock + "\n\nEnter missing characters (3 letters + 3 numbers, any order):";
                }
            }

            inputField.text = "";
            inputField.ActivateInputField();
            return;
        }

        if (input.Equals("/home", StringComparison.OrdinalIgnoreCase))
        {
            feedbackText.text = "";
            currentTarget = null;
            DisplayWifis();
            return;
        }

        if (input.Equals("/help", StringComparison.OrdinalIgnoreCase))
        {
            feedbackText.text = "";
            wifis.text = "Available commands:\n" +
                         "/crack <BSSID> - attempt to hack a Wi-Fi\n" +
                         "/home - return to list\n" +
                         "/help - show this help";
            return;
        }

        if (input.StartsWith("/crack "))
        {
            CrackCommand(input);
            return;
        }
    }

    void GenerateWifis()
    {
        networks.Clear();
        List<string> shuffledNames = new List<string>(wifiNames);
        ShuffleList(shuffledNames);
        List<int> barsPool = new List<int> { 2, 2, 2, 3, 3, 3, 4, 4, 4 };
        ShuffleList(barsPool);

        for (int i = 0; i < 9; i++)
        {
            networks.Add(new WifiNetwork
            {
                name = shuffledNames[i],
                bssid = GenerateShortBSSID(),
                power = UnityEngine.Random.Range(-90, -40),
                beacons = UnityEngine.Random.Range(1, 100),
                bars = barsPool[i],
                password = GenerateRandomPassword()
            });
        }
    }

void DisplayWifis()
{
    // 1. Find max length for each column
    int maxName = 0, maxBSSID = 0, maxPWR = 0, maxBeacons = 0, maxENC = 0, maxAUTH = 0;

    foreach (var net in networks)
    {
        maxName = Mathf.Max(maxName, net.name.Length);
        maxBSSID = Mathf.Max(maxBSSID, net.bssid.Length);
        maxPWR = Mathf.Max(maxPWR, (net.power + "dBm").Length);
        maxBeacons = Mathf.Max(maxBeacons, net.beacons.ToString().Length);
        maxENC = Mathf.Max(maxENC, net.enc.Length);
        maxAUTH = Mathf.Max(maxAUTH, net.auth.Length);
    }

    // 2. Create header using max lengths + 10 spacing
    string header =
        "Name".PadRight(maxName + 10) +
        "BSSID".PadRight(maxBSSID + 10) +
        "PWR".PadRight(maxPWR + 10) +
        "Beacons".PadRight(maxBeacons + 10) +
        "ENC".PadRight(maxENC + 10) +
        "AUTH".PadRight(maxAUTH + 10) +
        "STATUS\n";

    string separator = new string('-', header.Length) + "\n";

    string rows = "";

    foreach (var net in networks)
    {
        string status = net.isBlocked ? "BLOCKED" : net.isCracked ? "✓" : "";

        string name = net.name.PadRight(maxName) + new string(' ', 10);
        string bssid = net.bssid.PadRight(maxBSSID) + new string(' ', 10);
        string pwr = (net.power + "dBm").PadRight(maxPWR) + new string(' ', 10);
        string beacons = net.beacons.ToString().PadRight(maxBeacons) + new string(' ', 10);
        string enc = net.enc.PadRight(maxENC) + new string(' ', 10);
        string auth = net.auth.PadRight(maxAUTH) + new string(' ', 10);
        string stat = status;

        rows += name + bssid + pwr + beacons + enc + auth + stat + "\n";
    }

    wifis.text = header + separator + rows + "\nType /crack <BSSID> to begin hacking.";
}


    void CrackCommand(string input)
    {
        string bssid = input.Replace("/crack ", "").Trim();
        WifiNetwork target = networks.Find(n => n.bssid == bssid);

        if (target == null)
        {
            wifis.text = "No Wi-Fi found with that BSSID.";
            return;
        }

        if (target.isBlocked)
        {
            wifis.text = "🚫 This Wi-Fi is permanently blocked.";
            return;
        }

        if (target.isCracked)
        {
            wifis.text = $"Already cracked! Password: {target.password}\n\nType /home to return.";
            return;
        }

        currentTarget = target;
        currentCrackBlock = GenerateCrackBlock(out currentMissing);
        wifis.text = $"Cracking {target.name}...\n\n{currentCrackBlock}\n\nEnter missing characters (3 letters + 3 numbers, any order):";
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

    string GenerateCrackBlock(out string missingChars)
    {
        List<char> numbers = new List<char>("123456789".ToCharArray());
        List<char> letters = new List<char>("ABCDEFGHIJ".ToCharArray());

        ShuffleList(numbers);
        ShuffleList(letters);

        List<char> removed = new List<char>();
        removed.AddRange(numbers.GetRange(0, 3));
        removed.AddRange(letters.GetRange(0, 3));

        List<char> allowed = new List<char>();
        allowed.AddRange(numbers.GetRange(3, numbers.Count - 3));
        allowed.AddRange(letters.GetRange(3, letters.Count - 3));

        char[] block = new char[160];
        for (int i = 0; i < block.Length; i++)
        {
            block[i] = allowed[UnityEngine.Random.Range(0, allowed.Count)];
        }

        missingChars = string.Join("", removed);
        return new string(block);
    }

    bool ValidateMissingCharacters(string input)
    {
        input = input.ToUpper();
        foreach (char c in currentMissing)
        {
            if (!input.Contains(c.ToString())) return false;
        }
        return true;
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
