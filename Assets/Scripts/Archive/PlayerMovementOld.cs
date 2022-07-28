using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerMovementOld : MonoBehaviour, IOnEventCallback {
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    public bool grounded;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void OnEnable() {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable() {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent) {
        switch (photonEvent.Code) {
            case 0:
                object[] data = (object[]) photonEvent.CustomData;

                horizontalInput = (float) data[0];
                verticalInput = (float) data[1];
                
                moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
                break;
            case 1:
                if (grounded) {
                    Jump();
                }
                break;
            default:
                break;
        }
    }

    void Update() {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        SpeedControl();

        // Handle drag
        rb.drag = grounded ? groundDrag : 0;
    }

    void FixedUpdate() {
        MovePlayer();
    }

    private void MovePlayer() {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On ground
        if (grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // In air
        else if (!grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        if (flatVel.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump() {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}