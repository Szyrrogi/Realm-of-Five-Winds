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

    public void SetPlayer(string name,int nr)
    {
        Name.text = name;
        FaceImage.sprite = EventSystem.eventSystem.GetComponent<PlayerManager>().spriteFace[nr];
    }

    void Start()
    {
        // if (!PlayerManager.isSave)
        // {
            for (int i = 0; i < 16; i++)
            {
                // Pobierz tylko gry spełniające warunki
                List<FightManager.AutoBattlerGameViewModel> filteredList = LoadFeaturedGamesFromDatabase(
                    round: i,
                    playerName: PlayerManager.Name,
                    playerId: PlayerManager.Id,
                    version: PlayerManager.Version
                );

                if (filteredList.Count > 0)
                {
                    var randomGame = filteredList[UnityEngine.Random.Range(0, filteredList.Count)];

                    Names.Add(randomGame.Name);
                    Faces.Add(randomGame.FaceId);
                    Comps.Add(randomGame.Comp);
                }
            }
        // }
    }
    public static List<FightManager.AutoBattlerGameViewModel> LoadFeaturedGamesFromDatabase(int round, string playerName, int playerId, string version)
{
    List<FightManager.AutoBattlerGameViewModel> gamesList = new List<FightManager.AutoBattlerGameViewModel>();

    // Zapytanie SQL z filtrami
    string query = @"
        SELECT TOP 1 * 
        FROM AutoBattlerGame 
        WHERE Round = @Round 
          AND Name != @PlayerName 
          AND PlayerId != @PlayerId 
          ORDER BY NEWID()";
        //   AND Version = @Version
    SqlCommand cmd = new SqlCommand(query, DB.con);
    cmd.Parameters.AddWithValue("@Round", round);
    cmd.Parameters.AddWithValue("@PlayerName", playerName);
    cmd.Parameters.AddWithValue("@PlayerId", playerId);
    cmd.Parameters.AddWithValue("@Version", version);

    DataSet ds = DB.selectSQL(cmd);

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
}
