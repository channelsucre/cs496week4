using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using PhotonPlayer = Photon.Realtime.Player;

public class PlayerBulletCollision : MonoBehaviour
{
    public int health;
    Slider healthBar;
    GameObject lid;
    private GameObject EndingLose;

    void Awake() {
        health = 20;
        healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();
        lid = GameObject.Find("Lid");
        EndingLose = GameObject.Find("EndingLose");
    }

    void Start() {
        UpdateHealthBar();
    }

    void Update() {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        if (health <= 0 && gameObject.GetComponent<PlayerActions>().team == cp["Team"].ToString()) {
            cp["Dead"] = "Dead";
            foreach (PhotonPlayer p in PhotonNetwork.PlayerList) {
                if (p.CustomProperties["Team"].ToString() == cp["Team"].ToString()) {
                    Hashtable op = p.CustomProperties;
                    op["Dead"] = "Dead";
                    p.SetCustomProperties(op);
                }
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(cp);
        }
        if (health <= 0 && gameObject.GetComponent<PhotonView>().IsMine) {
            PhotonNetwork.Destroy(gameObject);
        }
        if (cp["Dead"] != null) {
            GameObject.Find("Launcher").GetComponent<Launcher>().Lost();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Bullet" && gameObject.GetComponent<PhotonView>().IsMine) {
            gameObject.GetComponent<PhotonView>().RPC("GotHit", RpcTarget.All, PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString());
        }
    }

    void UpdateHealthBar() {
        healthBar.value = (health / 20f);
    }

    [PunRPC]
    void GotHit(string team) {
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() == team) {
            StartCoroutine(Alert());
            health--;
            UpdateHealthBar();
        }
    }

    IEnumerator Alert() {
        for (int i = 0; i < 2; ++i) {
            lid.GetComponent<Lid>().Blink();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
