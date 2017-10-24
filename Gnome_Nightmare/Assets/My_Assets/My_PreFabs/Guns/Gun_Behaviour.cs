//Based off of a youtube video
//https://www.youtube.com/watch?v=THnivyG0Mvo
//

using UnityEngine;
using UnityEngine.UI;

public class Gun_Behaviour : MonoBehaviour {
    #region Variables
    public enum WeaponType { None, HitScan, Projectile, Melee };
    public WeaponType weaponType = WeaponType.None;
    [Space]
    public Ammo_Types.Ammo TypeOfAmmo = Ammo_Types.Ammo.Basic;
    public int ClipSize = 5;
    public int AmountCount = 0;
    [Space]
    public float Damage = 1.0f;
    public float Range = 100.0f;
    public float FireRate = 3.0f;
    public float ReloadTime = 2.0f;
    protected float NextTimeToFire = 0.0f;
    public float ImpactForce = 100.0f;
    [Space]
    public Camera PlayerCamera;
    public ParticleSystem MuzzleFlash;
    public GameObject ImpactEffect;
    [Space]
    public GameObject s_Spawner;
    public GameObject s_bullet;
    private GameObject s_clone = null;
    public float s_BulletSpeed;
    [Space]

    protected PlayerManager playerManager;
    protected MenuManager menuManager;
    protected GameObject AmountText;
    #endregion


    // Use this for initialization
    private void Start() {
        Invoke("DelayedStart", 0.1f);
    }
    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        PlayerCamera = Camera.main;
        playerManager = PlayerManager.instance;
        menuManager = MenuManager.instance;
    }

    private void Update() {
        if (weaponType == WeaponType.None) { }
        else if (weaponType == WeaponType.HitScan) { HitScanWeapons_Update(); }
        else if (weaponType == WeaponType.Projectile) { ProjectileWeapons_Update(); }
        else if (weaponType == WeaponType.Melee) { MeleeWeapons_Update(); }
    }


    #region HitScanWeapons
    private void HitScanWeapons_Update() {
        //If the player presses LeftClick or Right Triggger
        if ((Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f) && Time.time >= NextTimeToFire) {
            NextTimeToFire = Time.time + (1.0f / FireRate);
            if (AmountCount <= 0) { Mathf.Clamp(AmountCount, 0, 100000); Reload(); }
            else { HitScanWeapons_Shoot(); }
        }
        //If the player presses Right Click 
        if (Input.GetButtonDown("Fire2") && AmountCount < ClipSize) { Reload(); }
    }

    private void HitScanWeapons_Shoot() {
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

    #endregion

    #region ProjectileWeapons
    // Update is called once per frame
    private void ProjectileWeapons_Update() {
        if ((Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f) && Time.time >= NextTimeToFire) {
            NextTimeToFire = Time.time + (1.0f / FireRate);
            if (AmountCount <= 0) { Mathf.Clamp(AmountCount, 0, 100000); Reload(); }
            else { ProjectileWeapons_Shoot(); }
        }
        //If the player presses Right Click 
        if (Input.GetButtonDown("Fire2") && AmountCount < ClipSize) { Reload(); }
    }

    private void ProjectileWeapons_Shoot() {
        //If the player is in a Menu than return
        if (playerManager.MenuOpen) { return; }
        AmountCount--;
        //spawn bullet
        s_clone = (GameObject)Instantiate(s_bullet, s_Spawner.transform.position, s_Spawner.transform.rotation);
        s_clone.GetComponent<Bullet_Benaviour>().SetPlayerManager(playerManager);
        s_clone.GetComponent<Bullet_Benaviour>().SetDamage(Damage);
        if (CameraControl.isAiming) { ProjectileWeapons_RaycastProjectile(); }
        //shoot it
        else { s_clone.GetComponent<Rigidbody>().AddForce(s_Spawner.transform.forward * s_BulletSpeed, ForceMode.Impulse); }
        if (s_clone != null) { Destroy(s_clone, 2); }

        SetTextToAmount();
    }

    // Shoots a raycast from the camera position and in the camera's forward direction then fires a bullet in the direction of the ray's hit point
    private void ProjectileWeapons_RaycastProjectile() {
        RaycastHit hit;

        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, 1000.0f)) {
            s_Spawner.transform.LookAt(hit.transform);
            Debug.Log(hit.transform.name + " " + hit.point);
            Vector3 bulletDir = (hit.point - s_Spawner.transform.position).normalized;
            //shoot it
            s_clone.GetComponent<Rigidbody>().AddForce(bulletDir * s_BulletSpeed, ForceMode.Impulse); 
        }
        else {
            Debug.Log("Projectile Raycast Failed!");
            //shoot it
            s_clone.GetComponent<Rigidbody>().AddForce(s_Spawner.transform.forward * s_BulletSpeed, ForceMode.Impulse); 
        }
        s_Spawner.transform.rotation = Quaternion.identity;
    }
    #endregion

    #region MeleeWeapons
    private bool weaponSwung = false;
    private int swingAnimCount = 0;
    private bool DownSwing = true;

    void MeleeWeapons_Update() {
        if (weaponSwung == false && (Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f) && Time.time >= NextTimeToFire) {
            NextTimeToFire = Time.time + (1.0f / FireRate);
            weaponSwung = true;
            swingAnimCount = 0;
        }
        else if (weaponSwung) {
            // Swing weapon
            if (DownSwing && swingAnimCount < 7) {
                this.transform.Rotate(10, 0, 0);
                swingAnimCount++;
                if (swingAnimCount >= 7) { DownSwing = false; }
                
            }
            else if (!DownSwing && swingAnimCount >= 0) {
                this.transform.Rotate(-10, 0, 0);
                swingAnimCount--;
                if (swingAnimCount <= 0) { DownSwing = true; weaponSwung = false; }
            }
        }
    }

    #endregion

    private void Reload() {
        NextTimeToFire = Time.time + (ReloadTime / 1.0f);
        if (AmountCount >= ClipSize) { return; }
        else {
            for (int i = 0; i < menuManager.Ammo_Slot.transform.childCount; i++) {
                if (menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>()) {
                    if (menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().TypeOfAmmo == this.TypeOfAmmo) {
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

    public void SetTextToAmount() {
        if (!menuManager.Weapon_Slot.transform.GetChild(0)) { AmountText = null; return; }
        if (!menuManager.Weapon_Slot.transform.GetChild(0).transform.Find("Amount_Text").gameObject) { AmountText = null; return; }
        //need to change if Add a Swap function (Already have one with controllers)
        if (AmountText == null) { AmountText = menuManager.Weapon_Slot.transform.GetChild(0).transform.Find("Amount_Text").gameObject; }
        AmountText.GetComponent<Text>().text = AmountCount.ToString();
    }
}
