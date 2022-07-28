using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using PhotonPlayer = Photon.Realtime.Player;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MatchMaking : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text _player_count;
    [SerializeField]
    private TMP_Text _count_down;
    [SerializeField]
    private GameObject _room1;
    [SerializeField]
    private GameObject _room2;
    [SerializeField]
    private GameObject _room3;
    [SerializeField]
    private GameObject _room4;

    private bool loadRoom;

    // [SerializeField]
    // Start is called before the first frame update
    void Start()
    {
        _count_down.gameObject.SetActive(false);
        loadRoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeText();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 8 && !loadRoom)
        {
            foreach (PhotonPlayer p in PhotonNetwork.PlayerList)
            {
                if (p.CustomProperties["Team"] == null) return;
            }
            startLoading();
        }
    }

    void startLoading()
    {
        _count_down.gameObject.SetActive(true);
        StartCoroutine(countDown());
    }

    public void ChangeText()
    {
        _player_count.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/8";
    }

    IEnumerator countDown()
    {
        loadRoom = true;
        for (int i = 5; i > 0; --i) {
            yield return new WaitForSeconds(1);
            _count_down.text = i.ToString();
        }
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("Battleground");
    }
}