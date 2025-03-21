using HttpUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public int Golds { get; set; }
    public int MageCount { get; set; }
    public int SoldierCount { get; set; }
    public int SothiefCount { get; set; }

    public Player() { }
}

public class Score
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Rang { get; set; }
    public int PlayerId { get; set; }

    public Score() { }
}

public class DailyReward
{
    public int IdDailyReward { get; set; }
    public int IdPlayer { get; set; }
    public DateTime LastTimeSinceCollected { get; set; }

    public DailyReward() { }
}

public class GameManager : MonoBehaviour
{
    public GameDatabase GameDatabase;
    public RessourceHolder[] ressourceHolders;

    public Button CoinGenerator;
    public Button ChestButton;
    public Button AttackButton;
    public Image ChestImage;

    public Button leadboardBtn;
    public GameObject leadboard;
    public GameObject PrefabLineLeadboard;

    public BuyUnitButton[] buyUnitButton;

    public bool canOpenChest = true;

    public Player player;
    public int IDToLoad = 2;
    public Score playerScore;
    public DailyReward dailyReward;

    public DateTime LastTimeClicked = DateTime.MinValue;
    public TimeSpan cooldownDuration = TimeSpan.FromHours(1);

    public RestClient client;

    public void Start()
    {
        RestClient.Init();
        // on crée un client 
        client = new RestClient();

        player = LoadPlayerById(IDToLoad);

        string data1 = client.SendToWebService($"Highscores/{IDToLoad}", HttpVerb.GET, null, "");
        playerScore = Newtonsoft.Json.JsonConvert.DeserializeObject<Score>(data1);

        string data = client.SendToWebService($"DailyReward/Get/{IDToLoad}", HttpVerb.GET, null, "");
        dailyReward = Newtonsoft.Json.JsonConvert.DeserializeObject<DailyReward>(data);

        ressourceHolders[0].InitHolder(GameDatabase.coinSprite, player.Golds);
        ressourceHolders[1].InitHolder(GameDatabase.units[0].sprite, player.SoldierCount);
        ressourceHolders[2].InitHolder(GameDatabase.units[1].sprite, player.MageCount);
        ressourceHolders[3].InitHolder(GameDatabase.units[2].sprite, player.SothiefCount);

        buyUnitButton[0].InitButton(() =>
        {
            if (GameDatabase.units[0].price <= player.Golds)
            {
                player.SoldierCount++;
                player.Golds -= GameDatabase.units[0].price;
                ressourceHolders[0].UpdateText(player.Golds);
                ressourceHolders[1].UpdateText(player.SoldierCount);
            }
        },
                                    GameDatabase.units[0].sprite,
                                    GameDatabase.units[0].name,
                                    GameDatabase.coinSprite,
                                    GameDatabase.units[0].price);

        buyUnitButton[1].InitButton(() =>
        {
            if (GameDatabase.units[1].price <= player.Golds)
            {
                player.MageCount++;
                player.Golds -= GameDatabase.units[1].price;
                ressourceHolders[0].UpdateText(player.Golds);
                ressourceHolders[2].UpdateText(player.MageCount);
            }
        },
                                    GameDatabase.units[1].sprite,
                                    GameDatabase.units[1].name,
                                    GameDatabase.coinSprite,
                                    GameDatabase.units[1].price);

        buyUnitButton[2].InitButton(() =>
        {
            if (GameDatabase.units[2].price <= player.Golds)
            {
                player.SothiefCount++;
                player.Golds -= GameDatabase.units[2].price;
                ressourceHolders[0].UpdateText(player.Golds);
                ressourceHolders[3].UpdateText(player.SothiefCount);
            }
        },
                                    GameDatabase.units[2].sprite,
                                    GameDatabase.units[2].name,
                                    GameDatabase.coinSprite,
                                    GameDatabase.units[2].price);

        CoinGenerator.onClick.AddListener(() =>
        {
            player.Golds++;
            ressourceHolders[0].UpdateText(player.Golds);
        });

        StartCoroutine(CoroutineOpenChest());

        if (canOpenChest)
        {
            ChestImage.sprite = GameDatabase.ClosedChest;
        }
        else
        {
            ChestImage.sprite = GameDatabase.OpenChest;
        }

        ChestButton.onClick.AddListener(() =>
        {
            if (canOpenChest)
            {
                ChestImage.sprite = GameDatabase.ClosedChest;
                player.Golds += 500;
                ressourceHolders[0].UpdateText(player.Golds);
                LastTimeClicked = GetDateTime();
                dailyReward.LastTimeSinceCollected = LastTimeClicked;
                client.SendToWebService($"DailyReward/Update", HttpVerb.PUT, dailyReward, "");
                StartCoroutine(CoroutineOpenChest());
            }
        });

        AttackButton.onClick.AddListener(() =>
        {
            if (GetPlayerScore(player) > GetRandomPlayerScore())
            {
                playerScore.Value++;
                client.SendToWebService($"Highscores/Update", HttpVerb.PUT, playerScore, "");
            }
        });

        for (int i = 0; i < 8; i++)
        {
            Instantiate(PrefabLineLeadboard, leadboard.transform);
        }

        leadboardBtn.onClick.AddListener(() =>
        {
            leadboard.SetActive(!leadboard.activeSelf);

            if (leadboard.activeSelf)
            {
                UpdateLineLeadboard();
            }
        });
    }

    private void UpdateLineLeadboard()
    {
        string data = client.SendToWebService($"Highscores/GetLeaderboard", HttpVerb.GET, null, "");
        List<Score> AllScores = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Score>>(data);

        int i = 0;
        foreach (Score Score in AllScores)
        {
            leadboard.transform.GetChild(i + 1).GetComponentInChildren<TextMeshProUGUI>().text =
            $"{Score.Name} {Score.PlayerId} -> win : {Score.Value}";

            i++;
        }
    }

    public int GetPlayerScore(Player player)
    {
        return player.SoldierCount * GameDatabase.units[0].score +
                player.MageCount * GameDatabase.units[2].score +
                player.SothiefCount * GameDatabase.units[1].score;
    }
    public int GetRandomPlayerScore()
    {
        string data = client.SendToWebService($"Players/All", HttpVerb.GET, null, "");
        List<Player> allplayers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Player>>(data);
        return GetPlayerScore(allplayers[UnityEngine.Random.Range(0, allplayers.Count)]);
    }

    public Player LoadPlayerById(int ID)
    {
        string data = client.SendToWebService($"Players/{ID}", HttpVerb.GET, null, "");
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(data);
    }

    public DateTime GetDateTime()
    {
        return DateTime.Now;
    }
    public DateTime GetLastDateTime()
    {
        return LastTimeClicked;
    }

    public double GetSecondsBeforeOpenChest()
    {
        return (cooldownDuration - (GetDateTime() - GetLastDateTime())).TotalSeconds;
    }

    public IEnumerator CoroutineOpenChest()
    {
        canOpenChest = false;

        yield return new WaitForSeconds((float)GetSecondsBeforeOpenChest());

        canOpenChest = true;
        ChestImage.sprite = GameDatabase.OpenChest;
    }
}