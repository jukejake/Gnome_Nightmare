//Based off of a youtube video
//https://www.youtube.com/watch?v=THnivyG0Mvo
//

using UnityEngine;

public class Gun_Behaviour : MonoBehaviour {

    public float Damage = 1.0f;
    public float Range = 100.0f;
    public float FireRate = 3.0f;
    private float NextTimeToFire = 0.0f;
    public float ImpactForce = 100.0f;
    public Camera FpsCamera;
    public ParticleSystem MuzzleFlash;
    public GameObject ImpactEffect;

    private PlayerManager playerManager;

    // Use this for initialization
    void Start () {
        FpsCamera = Camera.main;
        playerManager = PlayerManager.instance;
    }
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f) && Time.time >= NextTimeToFire) {
            NextTimeToFire = Time.time + 1.0f / FireRate;
            Shoot();
        }
        if (Input.GetButtonDown("Fire2")) { Reload(); }
    }

    void Shoot() {
        if (playerManager.MenuOpen) { return; }

        MuzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(FpsCamera.transform.position, FpsCamera.transform.forward, out hit, Range)) {
            EnemyStats EnemyStat = hit.transform.GetComponent<EnemyStats>();
            if (EnemyStat != null) {
                if (EnemyStat.KillEnemy(Damage + playerManager.GetComponent<PlayerStats>().Damage.GetValue())) {
                    playerManager.GetComponent<PlayerStats>().addExperience(EnemyStat.Experience);
                    EnemyStat.OnDeath();
                }
            }
            if (hit.rigidbody != null) {
                //hit.rigidbody.AddForce(-hit.normal * ImpactForce);
            }

            GameObject ImpactAtHit = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactAtHit, 2.0f);
        }
    }

    void Reload() {

    }
}
