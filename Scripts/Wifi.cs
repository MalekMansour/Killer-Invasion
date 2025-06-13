using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;

public class Wifi : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_InputField inputField;
    public TMP_Text namesText;
    public TMP_Text bssidText;
    public TMP_Text pwrText;
    public TMP_Text beaconsText;
    public TMP_Text encText;
    public TMP_Text authText;
    public TMP_Text statusText;
    public TMP_Text feedbackText;

    // Made public so other scripts can reference it
    public class WifiNetwork
    {
        public string name, bssid, enc = "WPA2", auth = "PSK", password;
        public int power, beacons, bars;
        public int failedAttempts = 0;
        public bool isBlocked = false, isCracked = false;
    }

    // Made public so WifiSelector can read it
    public List<WifiNetwork> networks = new List<WifiNetwork>();

    private WifiNetwork currentTarget = null;
    private string currentMissing, currentCrackBlock;

    string[] wifiNames = {
        "Gordon's Office","SmartHome","Byte Me","BeerLover69420","Scott's Network",
        "Tell my Wi-Fi love her","NextHome-5G","infinitywifi","Mom click here for Wi-Fi"
    };
    string[] passwordWords = {
        "coffee","admin","dragon","hunter","tsunami","netgear","sneaky","shadow","monkey","kitty",
        "letmein","default","welcome","guest","qwerty","abc123","login","ball9","iloveyou","princess",
        "master","superman","batman","trustno1","password1","admin123","router","internet",
        "wifi4me","hidden123","mywifi","youcantguess","mypass","secureme","private","dangerzone",
        "fortress","n3tw0rk","rootaccess","onionnet","vault","nowifi4u","topsecret","blacknet","undercover",
        "quickpass","wiredin","2fast4u","insym","hackmeifyoucan","hiddenpower"
    };

    void Start()
    {
        GenerateWifis();
        DisplayWifis();
        feedbackText.text = "Type /crack <BSSID> to begin.";
        inputField.onSubmit.AddListener(HandleUserInput);
    }

    void HandleUserInput(string raw)
    {
        string input = raw.Trim();
        inputField.text = "";
        inputField.ActivateInputField();
        feedbackText.text = "";

        // If cracking in progress
        if (currentTarget != null)
        {
            if (ValidateMissingCharacters(input))
            {
                currentTarget.isCracked = true;
                string signal = currentTarget.bars == 2 ? "weak"
                             : currentTarget.bars == 3 ? "moderate"
                             : "strong";
                feedbackText.text = $"Network Cracked. Password: {currentTarget.password}\n" +
                                    $"Signal: {signal}\nType /home";
                currentTarget = null;
            }
            else
            {
                currentTarget.failedAttempts++;
                if (currentTarget.failedAttempts >= 2)
                {
                    currentTarget.isBlocked = true;
                    feedbackText.text = "Failed twice. Network blocked.\nType /home";
                    currentTarget = null;
                }
                else
                {
                    feedbackText.text = "Incorrect. Try again.";
                }
            }
            return;
        }

        // Global commands
        if (input.Equals("/home", StringComparison.OrdinalIgnoreCase))
        {
            currentTarget = null;
            feedbackText.text = "Type /crack <BSSID> to begin.";
            DisplayWifis();
            return;
        }
        if (input.Equals("/help", StringComparison.OrdinalIgnoreCase))
        {
            feedbackText.text = "Commands:\n/crack <BSSID>\n/home\n/help";
            return;
        }
        if (input.StartsWith("/crack ", StringComparison.OrdinalIgnoreCase))
        {
            CrackCommand(input);
        }
    }

    void GenerateWifis()
    {
        networks.Clear();
        // Shuffle name list
        var names = wifiNames.OrderBy(_ => UnityEngine.Random.value).ToList();
        // Prepare exactly 9 bars
        var bars  = new List<int> {2,2,2, 3,3,3, 4,4,4}
                          .OrderBy(_ => UnityEngine.Random.value)
                          .ToList();

        // ALWAYS add nine networks
        for (int i = 0; i < 9; i++)
        {
            networks.Add(new WifiNetwork
            {
                name     = names[i],
                bssid    = GenerateShortBSSID(),
                power    = UnityEngine.Random.Range(-90, -40),
                beacons  = UnityEngine.Random.Range(1, 100),
                bars     = bars[i],
                password = GenerateRandomPassword()
            });
        }
    }

    void DisplayWifis()
    {
        var sbName   = new StringBuilder("Name\n");
        var sbBssid  = new StringBuilder("BSSID\n");
        var sbPwr    = new StringBuilder("PWR\n");
        var sbBea    = new StringBuilder("Beacons\n");
        var sbEnc    = new StringBuilder("ENC\n");
        var sbAuth   = new StringBuilder("AUTH\n");
        var sbStatus = new StringBuilder("STATUS\n");

        foreach (var net in networks)
        {
            sbName.AppendLine(net.name);
            sbBssid.AppendLine(net.bssid);
            sbPwr.AppendLine($"{net.power}dBm");
            sbBea.AppendLine(net.beacons.ToString());
            sbEnc.AppendLine(net.enc);
            sbAuth.AppendLine(net.auth);
            string st = net.isBlocked ? "BLOCKED" : net.isCracked ? "CRACKED" : "AVAILABLE";
            sbStatus.AppendLine(st);
        }

        namesText.text   = sbName.ToString();
        bssidText.text   = sbBssid.ToString();
        pwrText.text     = sbPwr.ToString();
        beaconsText.text = sbBea.ToString();
        encText.text     = sbEnc.ToString();
        authText.text    = sbAuth.ToString();
        statusText.text  = sbStatus.ToString();
    }

    void CrackCommand(string input)
    {
        string b = input.Substring(7).Trim();
        var target = networks.Find(n => n.bssid.Equals(b, StringComparison.OrdinalIgnoreCase));
        if (target == null)
        {
            feedbackText.text = "No such BSSID";
            return;
        }
        if (target.isBlocked || target.isCracked)
        {
            feedbackText.text = "Cannot crack this network.";
            return;
        }

        // Clear text columns
        namesText.text   = "";
        bssidText.text   = "";
        pwrText.text     = "";
        beaconsText.text = "";
        encText.text     = "";
        authText.text    = "";
        statusText.text  = "";

        currentTarget     = target;
        currentCrackBlock = GenerateCrackBlock(out currentMissing);

        namesText.text   = "Cracking...\nType the 3 missing LETTERS and 3 missing NUMBERS (any order) and press Enter:\n\n" +
                           currentCrackBlock;
        feedbackText.text = "";
    }

    // Exposed so WifiSelector can call it
    public void StartConnection(WifiNetwork net)
    {
        float delay = net == null
            ? 0f
            : (net.bars == 2 ? 12f : net.bars == 3 ? 6f : 3f);

        feedbackText.text = $"Connecting to {(net == null ? "Weak Home Network" : net.name)}…\n" +
                            $"This will take {delay} seconds.";

        // TODO: invoke your loading bar / website opener here
    }

    string GenerateShortBSSID()
    {
        string[] hex = {"1","2","3","4","5","6","7","8","9","A","B","C","D","E","F"};
        var sb = new StringBuilder();
        for (int i = 0; i < 4; i++)
        {
            sb.Append(hex[UnityEngine.Random.Range(0, hex.Length)]);
            sb.Append(hex[UnityEngine.Random.Range(0, hex.Length)]);
            if (i < 3) sb.Append(":");
        }
        return sb.ToString();
    }

    string GenerateRandomPassword()
    {
        var w = passwordWords[UnityEngine.Random.Range(0, passwordWords.Length)];
        var n = UnityEngine.Random.Range(100, 999);
        return w + n;
    }

    string GenerateCrackBlock(out string missing)
    {
        var nums = new List<char>("123456789".ToCharArray());
        var lets = new List<char>("ABCDEFGHIJ".ToCharArray());
        ShuffleList(nums);
        ShuffleList(lets);

        var rem  = nums.Take(3).Concat(lets.Take(3)).ToList();
        var pool = nums.Skip(3).Concat(lets.Skip(3)).ToList();

        char[] block = new char[160];
        for (int i = 0; i < block.Length; i++)
            block[i] = pool[UnityEngine.Random.Range(0, pool.Count)];

        missing = new string(rem.ToArray());
        return new string(block);
    }

    bool ValidateMissingCharacters(string input)
    {
        input = input.ToUpper();
        return currentMissing.All(c => input.Contains(c));
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = UnityEngine.Random.Range(i, list.Count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }
}
