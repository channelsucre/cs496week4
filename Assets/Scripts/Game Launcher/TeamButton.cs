using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using photonPlayer = Photon.Realtime.Player;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TeamButton : MonoBehaviour
{
    private string teamName;
    private TMP_Text player1;
    private TMP_Text player2;
    [SerializeField]
    private GameObject myButton;
    // Start is called before the first frame update
    void Start()
    {
        player1 = myButton.transform.Find("Player 1").GetComponent<TMP_Text>();
        player2 = myButton.transform.Find("Player 2").GetComponent<TMP_Text>();
        teamName = myButton.transform.Find("RoomName").GetComponent<TMP_Text>().text;
    }

    // Update is called once per frame
    void Update()
    {
        player1.SetText("");
        player2.SetText("");
        foreach (photonPlayer p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties["Team"] != null)
            {
                if (p.CustomProperties["Team"].ToString() == teamName && p.CustomProperties["Role"].ToString() == "Player1")
                    player1.SetText(p.NickName);
                else if (p.CustomProperties["Team"].ToString() == teamName && p.CustomProperties["Role"].ToString() == "Player2")
                    player2.SetText(p.NickName);
            }
        }
    }

    public void onClick()
    {
        Debug.Log("onClick" + teamName);
        bool player1 = false, player2 = false;
        foreach (photonPlayer p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties["Team"] != null && p.CustomProperties["Team"].ToString() == teamName)
            {
                if (p.CustomProperties["Role"].ToString() == "Player1") player1 = true;
                else player2 = true;
            }
        }

        if (!player1)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { "Team", teamName }, { "Role", "Player1" } });
        } 
        else if (!player2)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { "Team", teamName }, { "Role", "Player2" } });
        }
    }
}
