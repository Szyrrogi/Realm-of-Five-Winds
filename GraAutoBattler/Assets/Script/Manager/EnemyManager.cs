using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Data.SqlClient;
using System;
using System.IO;
using System.Data;

public class EnemyManager : MonoBehaviour
{
    public TextMeshProUGUI Name;
    //public List<Sprite> FaceIcon;
    public Image FaceImage;

    public List<int> Faces;
    public List<string> Names;
    public List<string> Comps;
    public List<int> LP;

    public void SetPlayer(string name,int nr, int newLP)
    {
        SetRank(newLP);
        Name.text = name;
        FaceImage.sprite = EventSystem.eventSystem.GetComponent<PlayerManager>().spriteFace[nr];
    }

    public Image RankImage;
    public TextMeshProUGUI LPText;
    public TextMeshProUGUI RankNumber;

    void SetRank(int newLP)
    {
        if(LPText != null)
        {
            if(!RankedManager.Ranked)
            {
                RankImage.gameObject.SetActive(false);
                LPText.gameObject.SetActive(false);
                RankNumber.gameObject.SetActive(false);
            }
            else
            {
                RankImage.gameObject.SetActive(true);
                LPText.gameObject.SetActive(true);
                RankNumber.gameObject.SetActive(true);
                int sum = newLP;
                if(sum < 1500)
                {
                    RankImage.sprite = EventSystem.eventSystem.GetComponent<RankedManager>().ranksSprite[(int)sum / 300];
                    RankNumber.text = (((int)sum % 300) / 100 + 1).ToString();
                    LPText.text = ((int)sum % 100) + " LP";
                }
                else
                {
                    RankImage.sprite = EventSystem.eventSystem.GetComponent<RankedManager>().ranksSprite[5];
                    RankNumber.text = "";
                    LPText.text = (sum - 1500) + " LP";
                }
            }
        }
    }

    void Start()
    {
        // if (!PlayerManager.isSave)
        // {
        if(PlayerManager.SI)
        {
            Comps = SI.Official;
            for(int i = 0; i < 13; i++)
            {
                Faces.Add(0);    // Domyślne FaceId
                LP.Add(1000);
                Names.Add("System");
            }
        }
        if(Multi.multi)
        {
            for(int i = 0; i < 13; i++)
            {
                Faces.Add(0);    // Domyślne FaceId
                LP.Add(1000);
                Names.Add("System");
                Comps.Add(Zapasowy);
            }
        }
        if(!PlayerManager.SI && !Multi.multi)
        {
            int roll = 16;
            if(!RankedManager.Ranked)
                roll = Ranking.MaxRound + 3;
            for (int i = 0; i < roll; i++)
                {
                    // Pobierz tylko gry spełniające warunki
                    List<FightManager.AutoBattlerGameViewModel> filteredList = LoadFeaturedGamesFromDatabase(
                        round: i,
                        playerName: PlayerManager.Name,
                        playerId: PlayerManager.Id,
                        version: PlayerManager.Version,
                        LP: PlayerManager.LP
                    );
                    if (filteredList == null)
                    {
                        // Dodaj wartości domyślne do list
                        Names.Add("System");  // Domyślna nazwa
                        Faces.Add(0);    // Domyślne FaceId
                        Comps.Add(Zapasowy);  // Domyślny Comp
                        LP.Add(1000);
                    }
                    else if (filteredList.Count > 0)
                    {
                        var randomGame = filteredList[UnityEngine.Random.Range(0, filteredList.Count)];

                        Names.Add(randomGame.Name);
                        Faces.Add(randomGame.FaceId);
                        Comps.Add(randomGame.Comp);
                        LP.Add(randomGame.LP);
                    }
                }
        // }
        }
    }
    public static List<FightManager.AutoBattlerGameViewModel> LoadFeaturedGamesFromDatabase(int round, string playerName, int playerId, string version, int LP)
    {
    List<FightManager.AutoBattlerGameViewModel> gamesList = new List<FightManager.AutoBattlerGameViewModel>();

    // Zapytanie SQL z filtrami
    string query = @"
        SELECT TOP 1 * 
            FROM (
                SELECT TOP 15 *, ABS(LP - @LP) AS Diff
                FROM AutoBattlerGame 
                WHERE Round = @Round 
                AND LP > 0
                AND Name != @PlayerName
                AND PlayerId != @PlayerId
                ORDER BY ABS(LP - @LP)
            ) AS Top20Diff
            ORDER BY NEWID();";
    if(!RankedManager.Ranked)
    {
       query = @"
        SELECT TOP 1 * 
        FROM AutoBattlerGame 
        WHERE Round = @Round 
          AND Name != @PlayerName
          AND PlayerId != @PlayerId 
		  AND LP = 0
          ORDER BY NEWID()"; 
    }
        //   AND Version = @Version
    SqlCommand cmd = new SqlCommand(query, DB.con);
    cmd.Parameters.AddWithValue("@Round", round);
    cmd.Parameters.AddWithValue("@PlayerName", playerName);
    cmd.Parameters.AddWithValue("@PlayerId", playerId);
    cmd.Parameters.AddWithValue("@Version", version);
    cmd.Parameters.AddWithValue("@LP", LP);

    DataSet ds = DB.selectSQL(cmd);

    if (ds.Tables[0].Rows.Count == 0)
    {
        return null; // Zwróć null, jeśli nie ma wyników
    }

    foreach (DataRow dr in ds.Tables[0].Rows)
    {
        FightManager.AutoBattlerGameViewModel game = new FightManager.AutoBattlerGameViewModel
        {
            Id = Convert.ToInt32(dr["Id"]),
            Version = dr["Version"].ToString(),
            Name = dr["Name"].ToString(),
            FaceId = Convert.ToInt32(dr["FaceId"]),
            Date = Convert.ToDateTime(dr["Date"]),
            LP = Convert.ToInt32(dr["LP"]),
            PlayerId = Convert.ToInt32(dr["PlayerId"]),
            Comp = dr["Comp"].ToString(),
            Round = Convert.ToInt32(dr["Round"]),
        };
        gamesList.Add(game);
    }

    return gamesList;
    }
    public static string Zapasowy = @"
    {
    ""playerName"": ""System"",
    ""number"": 0,
    ""levelUp"": 4,
    ""units"": [
        {
            ""Line"": 0,
            ""Pole"": 0,
            ""Id"": 90,
            ""Name"": ""Mroczny Jeździec"",
            ""Cost"": 24,
            ""RealCost"": 0,
            ""Initiative"": 113,
            ""MaxHealth"": 360,
            ""Health"": 360,
            ""Attack"": 126,
            ""Defense"": 20,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""<b>Początek Walki:</b> usuwa Obronę z przeciwników"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 0,
            ""Pole"": 1,
            ""Id"": 182,
            ""Name"": ""Żągler Dynamitu"",
            ""Cost"": 24,
            ""RealCost"": 0,
            ""Initiative"": 41,
            ""MaxHealth"": 252,
            ""Health"": 252,
            ""Attack"": 54,
            ""Defense"": 10,
            ""Range"": 1,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""Atakuje jednocześnie wszystkiem przeciwników"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 0,
            ""Pole"": 2,
            ""Id"": 224,
            ""Name"": ""Elfi Dowódca"",
            ""Cost"": 24,
            ""RealCost"": 0,
            ""Initiative"": 77,
            ""MaxHealth"": 360,
            ""Health"": 360,
            ""Attack"": 90,
            ""Defense"": 5,
            ""Range"": 1,
            ""AP"": 54,
            ""MagicResist"": 15,
            ""Description"": ""."",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 0,
            ""Pole"": 3,
            ""Id"": 180,
            ""Name"": ""Lider"",
            ""Cost"": 32,
            ""RealCost"": 0,
            ""Initiative"": 83,
            ""MaxHealth"": 308,
            ""Health"": 308,
            ""Attack"": 110,
            ""Defense"": 15,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""<b>Początek Walki: </b>Aktywuj efekt <b>Początek Walki </b>wszystkim jesnotkom w rzędzie"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 0,
            ""Pole"": 4,
            ""Id"": 226,
            ""Name"": ""Feniks"",
            ""Cost"": 27,
            ""RealCost"": 0,
            ""Initiative"": 101,
            ""MaxHealth"": 270,
            ""Health"": 270,
            ""Attack"": 108,
            ""Defense"": 10,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""<b>Ostatni Oddech:</b> Przywróć te jednostkę z pełnym zdrowiem dwukrotnie"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 0,
            ""Pole"": 5,
            ""Id"": 95,
            ""Name"": ""Gildia Magów"",
            ""Cost"": 5,
            ""RealCost"": 0,
            ""Initiative"": 23,
            ""MaxHealth"": 20,
            ""Health"": 20,
            ""Attack"": 0,
            ""Defense"": 5,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""<b>Poczatek Walki: </b>Zapewnia wszysckim jednostką w rzędzie 15 Mocy Zaklęć"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 0,
            ""Pole"": 6,
            ""Id"": 39,
            ""Name"": ""Kuźnia Tytanów"",
            ""Cost"": 6,
            ""RealCost"": 0,
            ""Initiative"": 20,
            ""MaxHealth"": 30,
            ""Health"": 30,
            ""Attack"": 0,
            ""Defense"": 0,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""W sklepie pojawiają się jedynie jednostki warte 4 lub więcej złota"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 1,
            ""Pole"": 0,
            ""Id"": 33,
            ""Name"": ""Anioł"",
            ""Cost"": 24,
            ""RealCost"": 0,
            ""Initiative"": 89,
            ""MaxHealth"": 324,
            ""Health"": 324,
            ""Attack"": 72,
            ""Defense"": 20,
            ""Range"": 0,
            ""AP"": 72,
            ""MagicResist"": 10,
            ""Description"": ""Wykonuje trzy ataki, magiczny, fizyczny oraz losowy"",
            ""UpgradeLevel"": 2,
            ""SpellId"": 0
        },
        {
            ""Line"": 1,
            ""Pole"": 1,
            ""Id"": 35,
            ""Name"": ""Archanioł"",
            ""Cost"": 27,
            ""RealCost"": 0,
            ""Initiative"": 84,
            ""MaxHealth"": 360,
            ""Health"": 360,
            ""Attack"": 72,
            ""Defense"": 20,
            ""Range"": 0,
            ""AP"": 72,
            ""MagicResist"": 35,
            ""Description"": ""Pierwsze dwie jednostka, która zginie, zostaje wskrzeszona"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 1,
            ""Pole"": 2,
            ""Id"": 139,
            ""Name"": ""Energomant "",
            ""Cost"": 24,
            ""RealCost"": 0,
            ""Initiative"": 65,
            ""MaxHealth"": 360,
            ""Health"": 360,
            ""Attack"": 18,
            ""Defense"": 10,
            ""Range"": 1,
            ""AP"": 90,
            ""MagicResist"": 0,
            ""Description"": ""<b>Początek Walki: </b>Zwiększ Moc Zaklęć jednostce przed sobą o Moc Zaklęć jednostki za sobą i odwrotnie"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 1,
            ""Pole"": 3,
            ""Id"": 100,
            ""Name"": ""Nekromanta"",
            ""Cost"": 24,
            ""RealCost"": 0,
            ""Initiative"": 89,
            ""MaxHealth"": 360,
            ""Health"": 360,
            ""Attack"": 72,
            ""Defense"": 5,
            ""Range"": 1,
            ""AP"": 90,
            ""MagicResist"": 0,
            ""Description"": ""Po śmierci sojusznika przywołuje ulepszone zwierze"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 11
        },
        {
            ""Line"": 1,
            ""Pole"": 4,
            ""Id"": 88,
            ""Name"": ""Arcywampir"",
            ""Cost"": 27,
            ""RealCost"": 0,
            ""Initiative"": 96,
            ""MaxHealth"": 360,
            ""Health"": 360,
            ""Attack"": 126,
            ""Defense"": 10,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 10,
            ""Description"": ""<b>Atak: </b>Leczy się o 70% zadanych obrażeń"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 1,
            ""Pole"": 5,
            ""Id"": 96,
            ""Name"": ""Kapliczka"",
            ""Cost"": 8,
            ""RealCost"": 0,
            ""Initiative"": 30,
            ""MaxHealth"": 20,
            ""Health"": 20,
            ""Attack"": 0,
            ""Defense"": 5,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""<b>Po Walce: </b> zwiększa proces ulepszenia pierwszej jednostki w rzędzie o 1"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 1,
            ""Pole"": 6,
            ""Id"": 94,
            ""Name"": ""Schronienie"",
            ""Cost"": 3,
            ""RealCost"": 0,
            ""Initiative"": 20,
            ""MaxHealth"": 40,
            ""Health"": 40,
            ""Attack"": 0,
            ""Defense"": 0,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""<b>Poczatek Walki: </b>Zwiększ atak oraz zdrowie o 20 dla Wampirów"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 2,
            ""Pole"": 0,
            ""Id"": 37,
            ""Name"": ""Palladyn"",
            ""Cost"": 27,
            ""RealCost"": 0,
            ""Initiative"": 96,
            ""MaxHealth"": 288,
            ""Health"": 288,
            ""Attack"": 108,
            ""Defense"": 30,
            ""Range"": 0,
            ""AP"": 54,
            ""MagicResist"": 20,
            ""Description"": ""Posiada niewrażliwość na obrażenia przez pierwsze dwie tury"",
            ""UpgradeLevel"": 2,
            ""SpellId"": 0
        },
        {
            ""Line"": 2,
            ""Pole"": 1,
            ""Id"": 136,
            ""Name"": ""Chimera"",
            ""Cost"": 32,
            ""RealCost"": 0,
            ""Initiative"": 17,
            ""MaxHealth"": 117,
            ""Health"": 117,
            ""Attack"": 39,
            ""Defense"": 5,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""<b>Począrek Walki: </b>Otrzymuje tyle Ataku i Zdrowia, ile iniciatywy ma jednostka przed nią"",
            ""UpgradeLevel"": 4,
            ""SpellId"": 0
        },
        {
            ""Line"": 2,
            ""Pole"": 2,
            ""Id"": 222,
            ""Name"": ""Arcydruid"",
            ""Cost"": 27,
            ""RealCost"": 0,
            ""Initiative"": 77,
            ""MaxHealth"": 252,
            ""Health"": 252,
            ""Attack"": 90,
            ""Defense"": 5,
            ""Range"": 0,
            ""AP"": 54,
            ""MagicResist"": 0,
            ""Description"": ""<b>Ostatni Oddech: </b>Przemień się w bestie i podwój jej statystyki"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 2,
            ""Pole"": 3,
            ""Id"": 131,
            ""Name"": ""Czerw"",
            ""Cost"": 21,
            ""RealCost"": 0,
            ""Initiative"": 88,
            ""MaxHealth"": 144,
            ""Health"": 144,
            ""Attack"": 108,
            ""Defense"": 10,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""<b>Skok</b> Po zabiciu przeciwnika otrzymuje 40/40"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 2,
            ""Pole"": 4,
            ""Id"": 178,
            ""Name"": ""Dowódca"",
            ""Cost"": 24,
            ""RealCost"": 0,
            ""Initiative"": 77,
            ""MaxHealth"": 360,
            ""Health"": 360,
            ""Attack"": 72,
            ""Defense"": 15,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""Wykonuje trzy ataki"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 2,
            ""Pole"": 5,
            ""Id"": 7,
            ""Name"": ""Anielska Świątynia"",
            ""Cost"": 5,
            ""RealCost"": 0,
            ""Initiative"": 20,
            ""MaxHealth"": 30,
            ""Health"": 30,
            ""Attack"": 0,
            ""Defense"": 0,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""Zapewnia anioła w sklepie na poczatku każdej rundy"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        },
        {
            ""Line"": 2,
            ""Pole"": 6,
            ""Id"": 141,
            ""Name"": ""Oaza"",
            ""Cost"": 5,
            ""RealCost"": 0,
            ""Initiative"": 20,
            ""MaxHealth"": 20,
            ""Health"": 20,
            ""Attack"": 0,
            ""Defense"": 0,
            ""Range"": 0,
            ""AP"": 0,
            ""MagicResist"": 0,
            ""Description"": ""<b>Początek Walki: </b>Daj pierwszej jednostce w rzędzie <b>Skok</b>"",
            ""UpgradeLevel"": 1,
            ""SpellId"": 0
        }
    ]
}";
}
