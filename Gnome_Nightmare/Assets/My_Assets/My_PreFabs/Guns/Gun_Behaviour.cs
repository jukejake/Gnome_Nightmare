//Based off of a youtube video
//https://www.youtube.com/watch?v=THnivyG0Mvo
//

using UnityEngine;
using UnityEngine.UI;

public class Gun_Behaviour : MonoBehaviour {

    //public LayerMask myLayerMask;
    //[Space]

    public Ammo_Types.Ammo TypeOfAmmo = Ammo_Types.Ammo.Basic;
    public int ClipSize = 5;
    public int AmountCount = 0;
    [Space]

    public float Damage = 1.0f;
    public float Range = 100.0f;
    public float FireRate = 3.0f;
    public float ReloadTime = 2.0f;
    private float NextTimeToFire = 0.0f;
    public float ImpactForce = 100.0f;
    [Space]
    public Camera PlayerCamera;
    public ParticleSystem MuzzleFlash;
    public GameObject ImpactEffect;

    private PlayerManager playerManager;
    private MenuManager menuManager;
    private GameObject AmountText;

    // Use this for initialization
    private void Start () {
        Invoke("DelayedStart", 0.1f);
    }
    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        PlayerCamera = Camera.main;
        playerManager = PlayerManager.instance;
        menuManager = MenuManager.instance;
    }

    // Update is called once per frame
    private void Update () {
        //If the player presses LeftClick or Right Triggger
        if ((Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f) && Time.time >= NextTimeToFire) {
            NextTimeToFire = Time.time + (1.0f / FireRate);
            if (AmountCount <= 0) { Mathf.Clamp(AmountCount, 0, 100000); Reload(); }
            else { Shoot(); }
        }
        //If the player presses Right Click 
        if (Input.GetButtonDown("Fire2") && AmountCount < ClipSize) { Reload(); }
    }

    private void Shoot() {
        //If the player is in a Menu than return
        if (playerManager.MenuOpen) { return; }
        AmountCount--;
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

        SetTextToAmount();
    }

    private void Reload() {
        NextTimeToFire = Time.time + (ReloadTime / 1.0f);
        if (AmountCount >= ClipSize) { return; }
        else {
            for (int i = 0; i < menuManager.Ammo_Slot.transform.childCount; i++) {
                if (menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>()) {
                    if (menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().TypeOfAmmo == this.TypeOfAmmo){
                        int AmountNeeded = ClipSize - AmountCount;
                        int AmountGot = 0;
                        if (menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().Amount >= AmountNeeded) {
                            AmountGot = AmountNeeded;
                            menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().Amount -= AmountNeeded;
                        }
                        else {
                            AmountGot = menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().Amount;
                            menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().Amount = 0;
                        }
                        AmountCount += AmountGot;
                        SetTextToAmount();
                        return;
                    }
                }
            }
        }
        SetTextToAmount();
    }

    private void raycast() {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, 100.0f)) { Debug.Log(hit.transform.name); }
        else { Debug.Log("Out of range."); }
    }

    public void SetTextToAmount() {
        if (!menuManager.Weapon_Slot.transform.GetChild(0)) { AmountText = null; return; }
        if (!menuManager.Weapon_Slot.transform.GetChild(0).transform.Find("Amount_Text").gameObject) { AmountText = null; return; }
        //need to change if Add a Swap function (Already have one with controllers)
        if (AmountText == null) { AmountText = menuManager.Weapon_Slot.transform.GetChild(0).transform.Find("Amount_Text").gameObject; }
        AmountText.GetComponent<Text>().text = AmountCount.ToString();
    }
}
