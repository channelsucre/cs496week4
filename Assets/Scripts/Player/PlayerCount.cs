using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using PhotonPlayer = Photon.Realtime.Player;
using TMPro;

public class PlayerCount : MonoBehaviour {
    public int count;
    TMP_Text playerCountText;

    void Start() {
        playerCountText = GameObject.Find("Player Count").GetComponent<TMP_Text>();
    }

    void Update() {
        count = 0;

        foreach (PhotonPlayer p in PhotonNetwork.PlayerList) {
            Hashtable cp = p.CustomProperties;
            if (cp["Dead"] == null) {
                count++;
            }
        }

        playerCountText.text = (count / 2).ToString() + "/4";

        if (count == 8) PlayerActions.mode = 0;
        else if (count <= 6) PlayerActions.mode = 1;
        else if (count <= 4) PlayerActions.mode = 2;
    }
}
