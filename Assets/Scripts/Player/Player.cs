using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Player : MonoBehaviourPunCallbacks {
    int groundMask;
	float camRayLength = 200f;
    PhotonView playerPhotonView;
    string team;
    void Awake() {
        groundMask = LayerMask.GetMask("Ground");
        playerPhotonView = PhotonView.Get(this);
        team = PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString();

        // if (playerPhotonView.gameObject.GetComponent<PlayerActions>().team == team) {
        //     Debug.Log("Hi");
        //     Camera.main.GetComponent<CameraFollow>().target = playerPhotonView.gameObject.transform;
        // }
    }

    void Update() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool mouse0Input = Input.GetKeyDown(KeyCode.Mouse0);
        bool mouse1Input = Input.GetKeyDown(KeyCode.Mouse1);
        bool keyRInput = Input.GetKeyDown(KeyCode.R);
        
        Vector3 playerToMouse;
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundHit;
        if (Physics.Raycast(camRay, out groundHit, camRayLength, groundMask)) {
            playerToMouse = groundHit.point - transform.position;
            playerToMouse.y = 0f;
        } else {
            playerToMouse = Vector3.zero;
        }
        
        if (playerPhotonView.gameObject.GetComponent<PlayerActions>().team == team) {
            if (PhotonNetwork.LocalPlayer.CustomProperties["Role"].ToString() == "Player1") {
                playerPhotonView.RPC("GetPlayer1Values", RpcTarget.All, horizontalInput, verticalInput, keyRInput, team);
            } else {
                playerPhotonView.RPC("GetPlayer2Values", RpcTarget.All, mouse0Input, mouse1Input, playerToMouse, team);
            }
        }
    }
}
