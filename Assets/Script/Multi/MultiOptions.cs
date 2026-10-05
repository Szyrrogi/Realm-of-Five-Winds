using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;


public class MultiOptions : MonoBehaviourPunCallbacks 
{
    public TextMeshProUGUI LobbyName;
    public List<MultiPlayerObject> Gracze;
    PhotonView photonView;
    public GameObject StartButton;

    public static int Hearth = 3;
    public TextMeshProUGUI HearthText;

    public void UpOrDown(bool up)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            if(up && Hearth < 9)
            {
                Hearth++;
            }
            if(!up && Hearth > 1)
            {
                Hearth--;
            }
            photonView.RPC("UpdateHearth", RpcTarget.All, Hearth);
        }
        
    }
    [PunRPC]
    void UpdateHearth(int ile)
    {
        Hearth = ile;
    }

    void Update()
    {
        HearthText.text = Hearth.ToString();
        LobbyName.text = PhotonNetwork.CurrentRoom.Name;
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1 && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            StartButton.SetActive(true);
        else
            StartButton.SetActive(false);
        for(int i = 0; i < Gracze.Count; i++)
        {
            if(i < PhotonNetwork.CurrentRoom.PlayerCount)
                Gracze[i].gameObject.SetActive(true);
            else
                Gracze[i].gameObject.SetActive(false);
        }
    }
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        int myPlayerID = PhotonNetwork.LocalPlayer.ActorNumber;
        photonView.RPC("UpdatePlayerJoin", RpcTarget.All, myPlayerID, Hearth, PlayerManager.Name, PlayerManager.PlayerFaceId);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
            photonView.RPC("UpdateHearth", RpcTarget.All, Hearth);
        Start();
    }

    public void StartGame()
    {
        photonView.RPC("StartGameMulti", RpcTarget.All);
    }

    [PunRPC]
    public void StartGameMulti()
    {
        PhotonNetwork.LoadLevel(3);
    }

    [PunRPC]
    public void UpdatePlayerJoin(int id, int hp, string name, int image)
    {
        Gracze[id - 1].SetObject(hp, name, image);
    }
}
