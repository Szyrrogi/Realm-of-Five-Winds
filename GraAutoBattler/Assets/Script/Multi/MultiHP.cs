using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class MultiHP : MonoBehaviour
{
    public List<MultiPlayerObject> Gracze;
    public List<TextMeshProUGUI> HealthPlayer;
    public static int ID;



    void Update()
    {
        if(Multi.multi)
        {
            for(int i = 0; i < HealthPlayer.Count; i++)
            {
                if(i < Multi.Health.Length)
                    HealthPlayer[i].text = Multi.Health[i].ToString();
                else
                    HealthPlayer[i].gameObject.SetActive(false);
            }
            for(int i = 0; i < Gracze.Count; i++)
            {
                if(i < Multi.Health.Length)
                    Gracze[i].gameObject.SetActive(true);
                else
                    Gracze[i].gameObject.SetActive(false);
            }
        }
    }
    void Start()
    {
        if(Multi.multi)
        {
            ID = PhotonNetwork.LocalPlayer.ActorNumber;
            PhotonView photonView = GetComponent<PhotonView>();
            int myPlayerID = PhotonNetwork.LocalPlayer.ActorNumber;
            photonView.RPC("UpdatePlayerJoin", RpcTarget.All, myPlayerID, 3, PlayerManager.Name, PlayerManager.PlayerFaceId);
        }
        else
        {
            for(int i = 0; i < HealthPlayer.Count; i++)
            {

                HealthPlayer[i].gameObject.SetActive(false);
            }
            for(int i = 0; i < Gracze.Count; i++)
            {

                Gracze[i].gameObject.SetActive(false);
            }
        }
    }
    [PunRPC]
    public void UpdatePlayerJoin(int id, int hp, string name, int image)
    {
        Gracze[id - 1].SetObject(hp, name, image);
    }

    [PunRPC]
    public void ZmniejszZdrowie(int id)
    {
        Multi.Health[id - 1]--;
    }
    [PunRPC]
    public void UpZdrowie(int id)
    {
        Multi.Health[id - 1]++;
    }
    
}
