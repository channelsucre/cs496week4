using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
// [RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class PlayerActions : MonoBehaviour {
	public float speed = 15f;
	public float dashDistance = 12f;
	public float dashCooldown = 2.5f;
	public float shootCooldown = 0.3f;
	public int maxAmmoCount = 20;
	public Transform prefabBullet;
	float dashTimer;
	float shootTimer;
	int ammoCount;
	public static int mode;

	float horizontalInput;
	// float horizontalInput = Input.GetAxisRaw("Horizontal");
	float verticalInput;
	// float verticalInput = Input.GetAxisRaw("Vertical");
	bool mouse0Input;
	// bool mouse0Input = Input.GetKeyDown(KeyCode.Mouse0);
	bool mouse1Input;
	// bool mouse1Input = Input.GetKeyDown(KeyCode.Mouse1);
	bool keyRInput;
	Vector3 playerToMouse;
	// Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
	// RaycastHit groundHit;
	// if (Physics.Raycast(camRay, out groundHit, camRayLength, groundMask)) {
	// 	Vector3 playerToMouse = groundHit.point - transform.position;
	// 	playerToMouse.y = 0f;
	// } else {
	// 	playerToMouse = Vector3.zero;
	// }

	Vector3 movement;
	// Animator animator;
	Rigidbody playerRigidbody;

	public string team;
	

	void Awake() {
		// animator = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();
		dashTimer = dashCooldown;
		shootTimer = shootCooldown;
		ammoCount = maxAmmoCount;
		mode = 0;
	}

	void Update() {
		if (mouse0Input && shootTimer >= shootCooldown && ammoCount > 0) {
			if (team == PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString() && PhotonNetwork.LocalPlayer.CustomProperties["Role"].ToString() == "Player1")
				gameObject.GetComponent<PhotonView>().RPC("UseBullet", RpcTarget.All);
			switch (mode) {
				case 0:
					Shoot();
					break;
				case 1:
					Shoot();
					break;
				case 2:
					ThreeWayShoot();
					break;
				default:
					break;
			}
		}

		if (mouse1Input && dashTimer >= dashCooldown) {
			Dash(playerToMouse);
		}

		if (keyRInput && ammoCount <= 0) {
			gameObject.GetComponent<PhotonView>().RPC("Reload", RpcTarget.All);
		}

		dashTimer += Time.deltaTime;
		shootTimer += Time.deltaTime;
	}

	void FixedUpdate() {
		Move(horizontalInput, verticalInput);
		Turn(playerToMouse);
		// Animating(h, v);
	}

	[PunRPC]
	void ChangeTeam(string t) {
		team = t;
		if (team == PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()) {
			Camera.main.gameObject.AddComponent<CameraFollow>().target = gameObject.transform;
		}
	}

	[PunRPC]
	void GetPlayer1Values(float p0, float p1, bool p2, string t) {
		if (team == t) {
			horizontalInput = p0;
			verticalInput = p1;
			keyRInput = p2;
		}
	}

	[PunRPC]
	void GetPlayer2Values(bool p3, bool p4, Vector3 p5, string t) {
		if (team == t) {
			if (!mouse0Input) {
				mouse0Input = p3;
			}
			if (!mouse1Input && dashTimer >= dashCooldown) {
				mouse1Input = p4;
			}
			playerToMouse = p5;
		}
	}

	void Move(float horizontalInput, float verticalInput) {
		if (this.GetComponent<PhotonView>().IsMine) {
			movement.Set(horizontalInput + verticalInput, 0f, -horizontalInput + verticalInput);
			movement = movement.normalized * speed * Time.deltaTime;

			playerRigidbody.MovePosition(transform.position + movement);
		}
	}

	[PunRPC]
	void Reload() {
		ammoCount = 20;
		if (team == PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()) {
			GameObject.Find("Ammo Bar").GetComponent<Slider>().value = ammoCount / 20f;
			mouse0Input = false;
		}
	}

	[PunRPC]
	void UseBullet() {
		ammoCount--;
		if (team == PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()) {
			GameObject.Find("Ammo Bar").GetComponent<Slider>().value = ammoCount / 20f;
		}
	}

	void Shoot() {
		Transform instanceBullet = Instantiate(prefabBullet, transform.position + Vector3.up, Quaternion.identity);
		Physics.IgnoreCollision(instanceBullet.GetComponent<Collider>(), GetComponent<Collider>());

		Rigidbody bulletRigidbody = instanceBullet.GetComponent<Rigidbody>();
		bulletRigidbody.AddForce(transform.forward * 40, ForceMode.Impulse);

		mouse0Input = false;
		shootTimer = 0f;

		if (team == PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()) {
			instanceBullet.gameObject.GetComponent<Renderer>().material.color = new Color(0.1f, 0.1f, 0.8f);
		} else {
			instanceBullet.gameObject.GetComponent<Renderer>().material.color = new Color(0.8f, 0.1f, 0.1f);
		}
    }

	void FourWayShoot() {
		Vector3[] ways = {transform.forward, transform.right, -transform.forward, -transform.right};
		Transform[] bullets = new Transform[4];

		for (int i = 0; i < 4; ++i) {
			bullets[i] = Instantiate(prefabBullet, transform.position + Vector3.up, Quaternion.identity);
			Physics.IgnoreCollision(bullets[i].GetComponent<Collider>(), GetComponent<Collider>());
			bullets[i].GetComponent<Rigidbody>().AddForce(ways[i] * 30, ForceMode.Impulse);

			if (team == PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()) {
				bullets[i].gameObject.GetComponent<Renderer>().material.color = new Color(0.1f, 0.1f, 0.8f);
			} else {
				bullets[i].gameObject.GetComponent<Renderer>().material.color = new Color(0.8f, 0.1f, 0.1f);
			}
		}

		mouse0Input = false;
		shootTimer = 0f;
	}

	void ThreeWayShoot() {
		Vector3[] ways = {3 * transform.forward, 2 * transform.forward + transform.right, 2 * transform.forward + (-transform.right)};
		Transform[] bullets = new Transform[3];

		for (int i = 0; i < 3; ++i) {
			bullets[i] = Instantiate(prefabBullet, transform.position + Vector3.up, Quaternion.identity);
			Physics.IgnoreCollision(bullets[i].GetComponent<Collider>(), GetComponent<Collider>());
			bullets[i].GetComponent<Rigidbody>().AddForce(ways[i] * 10, ForceMode.Impulse);

			if (team == PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString()) {
				bullets[i].gameObject.GetComponent<Renderer>().material.color = new Color(0.1f, 0.1f, 0.8f);
			} else {
				bullets[i].gameObject.GetComponent<Renderer>().material.color = new Color(0.8f, 0.1f, 0.1f);
			}
		}

		mouse0Input = false;
		shootTimer = 0f;
	}

	void Dash(Vector3 playerToMouse) {
		playerRigidbody.MovePosition(transform.position + dashDistance * playerToMouse.normalized);
		
		mouse1Input = false;
		dashTimer = 0f;
	}

	void Turn(Vector3 playerToMouse) {
		if (playerToMouse.magnitude > 0f) {
			Quaternion rotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(rotation);
		}
	}

	// void Animating(float h, float v) {
	// 	bool walking = (h != 0f || v != 0f);
	// 	animator.SetBool("IsWalking", walking);
	// }

	private void OnEnable() {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable() {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}