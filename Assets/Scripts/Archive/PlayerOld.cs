using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerOld : MonoBehaviourPunCallbacks
// public class Player : MonoBehaviourPunCallbacks, IOnEventCallback
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            // This is the local client
            float x = Input.GetAxis("Horizontal") * 10f * Time.deltaTime;
            float z = Input.GetAxis("Vertical") * 10f * Time.deltaTime;
            bool jump = Input.GetKeyDown(KeyCode.Space);
            if (x != 0 || z != 0)
            {
                byte evCode = 0;
                object[] content = new object[] { x, z };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, SendOptions.SendReliable);
            }

            if (jump)
            {
                byte evCode = 1;
                object[] content = new object[] {};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, SendOptions.SendReliable);
            }
        }
    }

    // private void OnEnable()
    // {
    //     PhotonNetwork.AddCallbackTarget(this);
    // }

    // private void OnDisable()
    // {
    //     PhotonNetwork.RemoveCallbackTarget(this);
    // }

    // public void OnEvent(EventData photonEvent)
    // {
    //     byte eventCode = photonEvent.Code;
    //     Debug.Log("OnEvent");
    // }
}
