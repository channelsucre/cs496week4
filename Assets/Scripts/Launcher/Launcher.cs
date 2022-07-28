using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Threading;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using PhotonPlayer = Photon.Realtime.Player;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks {
    public PhotonView playerPrefab;
    GameObject playerInstance;
    Slider slider;

    float timeStamp = 0f;
    const int timeInterval = 15;
    private GameObject EndingWin;
    private GameObject EndingLose;
    private GameObject Replay;

    // Start positions (Alpha, Bravo, Charlie, Delta)
    public Vector3[] startPositions = new Vector3[] {new Vector3(-90, 1, 90), new Vector3(90, 1, 90), new Vector3(-90, 1, -90), new Vector3(90, 1, -90)};

    // Start is called before the first frame update
    void Start() {
        slider = GameObject.Find("Progress Bar").GetComponent<Slider>();
        EndingWin = GameObject.Find("EndingWin");
        EndingLose = GameObject.Find("EndingLose");
        EndingWin.SetActive(false);
        EndingLose.SetActive(false);
        if (PhotonNetwork.LocalPlayer.CustomProperties["Role"].ToString() == "Player1") {
            Debug.Log(startPositions[(int) PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()[0] - (int) 'A']);
            Debug.Log((int) PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()[0] - (int) 'A');
            playerInstance = PhotonNetwork.Instantiate(playerPrefab.name, startPositions[(int) PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()[0] - (int) 'A'], Quaternion.identity);

            PhotonView photonCube = PhotonView.Get(playerInstance);
            photonCube.RPC("ChangeTeam", RpcTarget.All, PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString());
            GameObject.Find("Fill").GetComponent<Image>().color = new Color(0.878f, 0.839f, 0.345f, 1f);
        } else {
            GameObject.Find("Fill").GetComponent<Image>().color = new Color(0.227f, 0.623f, 0.298f, 1f);
        }
    }

    void Update() {
        if ((int)timeStamp / timeInterval != (int)Time.timeSinceLevelLoad / timeInterval) {
            changeRole();
        }
        timeStamp = Time.timeSinceLevelLoad;
        slider.value = ((timeStamp) % 15) / 15;
        foreach (PhotonPlayer p in PhotonNetwork.PlayerList) {
            if (p.CustomProperties["Dead"] == null && 
                p.CustomProperties["Team"].ToString() != PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()) return;
        }
        EndingWin.SetActive(true);
        StartCoroutine(MainMenu());
    }

    void changeRole() {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        string role = cp["Role"].ToString();
        if (role == "Player1") {
            cp["Role"] = "Player2";
            GameObject.Find("Fill").GetComponent<Image>().color = new Color(0.227f, 0.623f, 0.298f, 1f);
        } else if (role == "Player2") {
            cp["Role"] = "Player1";
            GameObject.Find("Fill").GetComponent<Image>().color = new Color(0.878f, 0.839f, 0.345f, 1f);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(cp);
    }

    public void Lost() {
        EndingLose.SetActive(true);
        StartCoroutine(MainMenu());
    }

    public IEnumerator MainMenu() {
        yield return new WaitForSeconds(4);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable());
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}