using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour {
    public float timeRemaining = 3f;

    void Update() {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            Destroy(gameObject);
        }
    }
}
