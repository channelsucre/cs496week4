using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    float timer;
    int shootableMask;

    public Transform prefabBullet;


    void Awake() {
        shootableMask = LayerMask.GetMask("Shootable");
    }


    void Update() {
        timer += Time.deltaTime;

		if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) {
            Shoot ();
        }
    }

    void Shoot() {
        timer = 0f;

        Transform instanceBullet = Instantiate(prefabBullet, transform.position + Vector3.up, Quaternion.identity);

        Rigidbody bulletRigidbody = instanceBullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(transform.forward * 100, ForceMode.Impulse);
    }
}
