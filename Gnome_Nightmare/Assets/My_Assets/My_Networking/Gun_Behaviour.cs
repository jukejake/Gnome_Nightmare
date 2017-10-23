//Based off of a youtube video
//https://www.youtube.com/watch?v=THnivyG0Mvo
//

using UnityEngine;
using UnityEngine.Networking;

public class Network_Gun_Behaviour : NetworkBehaviour {

    public float Damage = 1.0f;
    public float Range = 100.0f;
    public float FireRate = 3.0f;
    private float NextTimeToFire = 0.0f;
    public float ImpactForce = 100.0f;
    public Camera PlayerCamera;
    public ParticleSystem MuzzleFlash;
    public GameObject ImpactEffect;

    private PlayerManager playerManager;

    // Use this for initialization
    void Start () {
        Invoke("DelayedStart", 0.1f);
    }
    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        PlayerCamera = Camera.main;
        playerManager = PlayerManager.instance;
    }

    // Update is called once per frame
    void Update () {
        //If the player presses LeftClick or Right Triggger
        if ((Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f) && Time.time >= NextTimeToFire) {
            NextTimeToFire = Time.time + 1.0f / FireRate;
            CmdShoot();
        }
        //If the player presses Right Click 
        if (Input.GetButtonDown("Fire2")) { Reload(); }
    }

    [Command]
    void CmdShoot() {
        //If the player is in a Menu than return
        if (playerManager.MenuOpen) { return; }

        //Play a Particle System at end to the gun
        if (MuzzleFlash != null) { MuzzleFlash.Play(); }
        RaycastHit hit;
        //Fires a Raycast to find something to hit
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, Range)) {
            //Get enemies stats
            EnemyStats EnemyStat = hit.transform.GetComponent<EnemyStats>();
            //If what was hit has an EnemyStat
            if (EnemyStat != null) {
                //Applys damage to the enemy and if it kills it, destroy the enemie
                if (EnemyStat.DamageEnemy(Damage + playerManager.GetComponent<PlayerStats>().Damage.GetValue())) {
                    playerManager.GetComponent<PlayerStats>().AddExperience(EnemyStat.Experience);
                    //Destroys the enemy
                    EnemyStat.OnDeath();
                }
            }
            //Apply force to the rigidbody 
            //if (hit.rigidbody != null) { hit.rigidbody.AddForce(-hit.normal * ImpactForce); }

            //Spawn a Particle System at where the Raycast hit
            GameObject ImpactAtHit = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactAtHit, 2.0f);
        }
    }

    void Reload() {

    }

    void raycast() {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, 100.0f)) { Debug.Log(hit.transform.name); }
        else { Debug.Log("Out of range."); }
    }
}
