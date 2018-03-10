using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Gun_Behaviour : SerializedMonoBehaviour {

    #region Variables
    //public enum WeaponType { None, HitScan, Projectile, Melee };
    //public WeaponType weaponType;

    LayerMask layerMask;

    [TableList, HideInInspector]
    public OdinTables.WeaponStatsTable Stats = new OdinTables.WeaponStatsTable();

    [ToggleGroup("WeaponTypeHitScan", order: 1, groupTitle: "Hit Scan")]
    public bool WeaponTypeHitScan;
    [ToggleGroup("WeaponTypeProjectile", order: 2, groupTitle: "Projectile")]
    public bool WeaponTypeProjectile;

    [ToggleGroup("WeaponTypeHitScan"), ToggleGroup("WeaponTypeProjectile")]
    public Ammo_Types.Ammo TypeOfAmmo = Ammo_Types.Ammo.Basic;

    
    protected float NextTimeToFire = 0.0f;
    [HideInInspector]
    public Camera PlayerCamera;
    [ToggleGroup("WeaponTypeHitScan"), ToggleGroup("WeaponTypeProjectile")]
    public ParticleSystem MuzzleFlash;
    [ToggleGroup("WeaponTypeHitScan"), ToggleGroup("WeaponTypeProjectile")]
    public GameObject ImpactEffect;
    [ToggleGroup("WeaponTypeHitScan"), ToggleGroup("WeaponTypeProjectile")]
    public float HipFirePushback;
    [ToggleGroup("WeaponTypeHitScan"), ToggleGroup("WeaponTypeProjectile")]
    public float PushbackSpread;
    [ToggleGroup("WeaponTypeProjectile")]
    public GameObject s_Spawner;
    [ToggleGroup("WeaponTypeProjectile")]
    public GameObject s_bullet;
    [ToggleGroup("WeaponTypeProjectile")]
    private GameObject s_clone = null;
    [ToggleGroup("WeaponTypeProjectile")]
    public float s_BulletSpeed;
    [ToggleGroup("WeaponTypeProjectile")]
    public bool AimingHitscan = false;

    [BoxGroup("Sound")]
    public AudioSource FireSound;
    [BoxGroup("Sound")]
    public AudioSource ImpactSound;
    [BoxGroup("Sound")]
    public AudioSource ReloadSound;

    protected PlayerManager playerManager;
    protected MenuManager menuManager;
    protected GameObject AmountText;
    protected Transform EffectTab;
    #endregion

    // Use this for initialization
    private void Start() {
        Invoke("DelayedStart", 0.1f);
        layerMask = ~(1 << LayerMask.NameToLayer("Ignore Bullets") | 1 << LayerMask.NameToLayer("Ignore Invisable Walls"));
        PlayerCamera = Camera.main;
        playerManager = PlayerManager.instance;
        menuManager = MenuManager.instance;
        SetTextToAmount();
        EffectTab = GameObject.Find("EffectTab").transform;
    }
    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {  }

    private void Update() {
        if (playerManager.player.GetComponent<PlayerStats>().isDead) { return; }

        if (WeaponTypeHitScan) { HitScanWeapons_Update(); }
        else if (WeaponTypeProjectile) { ProjectileWeapons_Update(); }
    }
    

    #region HitScanWeapons
    //[ToggleGroup("WeaponTypeHitScan")]
    private void HitScanWeapons_Update() {
        //If the player presses LeftClick or Right Triggger
        if ((Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f) && Time.time >= NextTimeToFire) {
            NextTimeToFire = Time.time + (1.0f / this.Stats.FireRate.GetValue());
            if (TypeOfAmmo == Ammo_Types.Ammo.Extinguisher) { HitScanWeapons_Shoot(); }
            else {
                if (this.Stats.AmountCount.GetValue() <= 0) { Mathf.Clamp(this.Stats.AmountCount.GetValue(), 0, 100000); Reload(); }
                else { HitScanWeapons_Shoot(); }
            }
        }
        //If the player presses Right Click 
        if ((Input.GetButtonDown("Fire2") || Input.GetButton("CX")) && this.Stats.AmountCount.GetValue() < this.Stats.ClipSize.GetValue()) { Reload(); }
    }
    //[ToggleGroup("WeaponTypeHitScan")]
    private void HitScanWeapons_Shoot() {
        //If the player is in a Menu than return
        if (this.playerManager.MenuOpen) { return; }
        this.Stats.AmountCount.baseValue--;
        //Play a Particle System at end to the gun
        if (MuzzleFlash != null) { MuzzleFlash.Play(); }
        if (FireSound != null) { FireSound.Play(); }
        RaycastHit hit;
        //Fires a Raycast to find something to hit
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, this.Stats.Range.GetValue(), layerMask)) {
            Debug.DrawLine(this.PlayerCamera.transform.position, hit.point, Color.red, 5.0f);
            //Get enemies stats
            EnemyStats EnemyStat = hit.transform.GetComponent<EnemyStats>();
            //If what was hit has an EnemyStat
            if (EnemyStat != null) {
                //Applys damage to the enemy and if it kills it, destroy the enemie
                if (EnemyStat.DamageEnemy(this.Stats.Damage.GetValue() + this.playerManager.GetComponent<PlayerStats>().Damage.GetValue())) {
                    this.playerManager.GetComponent<PlayerStats>().AddExperience(EnemyStat.Experience);
                    this.playerManager.GetComponent<PlayerStats>().AddPoints(EnemyStat.Points);
                    this.playerManager.GetComponent<PlayerStats>().AddKills(1);
                    //Destroys the enemy
                    EnemyStat.OnDeath();
                }
            }
            //Apply force to the rigidbody 
            //if (hit.rigidbody != null) { hit.rigidbody.AddForce(-hit.normal * Stats.ImpactForce.GetValue()); }

            //Spawn a Particle System at where the Raycast hit
            GameObject ImpactAtHit = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            ImpactAtHit.transform.SetParent(EffectTab.transform);
            Destroy(ImpactAtHit, 2.0f);
        }
        else {
            Debug.DrawLine(this.PlayerCamera.transform.position, this.PlayerCamera.transform.position + (this.PlayerCamera.transform.forward * this.Stats.Range.GetValue()), Color.blue, 5.0f);
        }

        SetTextToAmount();
    }

    #endregion

    #region ProjectileWeapons
    // Update is called once per frame
    //[ToggleGroup("WeaponTypeProjectile")]
    private void ProjectileWeapons_Update() {

        if ((Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f) && Time.time >= NextTimeToFire) {
            //RaycastHit hit;
            //if (Physics.Raycast(this.PlayerCamera.transform.position, this.PlayerCamera.transform.forward, out hit, this.Stats.Range.GetValue(), layerMask)) {
            //    s_Spawner.transform.LookAt(hit.point);
            //    Debug.DrawLine(this.PlayerCamera.transform.position, hit.point, Color.red, 5.0f);
            //}
            //else {
                s_Spawner.transform.LookAt(this.PlayerCamera.transform.position+(this.PlayerCamera.transform.forward * this.Stats.Range.GetValue()));
                Debug.DrawLine(this.PlayerCamera.transform.position, this.PlayerCamera.transform.position+(this.PlayerCamera.transform.forward * this.Stats.Range.GetValue()), Color.blue, 5.0f);
            //}

            NextTimeToFire = Time.time + (1.0f / this.Stats.FireRate.GetValue());

            if (TypeOfAmmo == Ammo_Types.Ammo.Extinguisher) { ProjectileWeapons_Shoot(); }
            else {
                if (this.Stats.AmountCount.GetValue() <= 0) { Mathf.Clamp(this.Stats.AmountCount.GetValue(), 0, 100000); Reload(); }
                else { ProjectileWeapons_Shoot(); }
            }
        }

        //If the player presses Right Click 
        if ((Input.GetButtonDown("Fire2") || Input.GetButton("CX")) && this.Stats.AmountCount.GetValue() < this.Stats.ClipSize.GetValue()) { Reload(); }
        
    }
    //[ToggleGroup("WeaponTypeProjectile")]
    private void ProjectileWeapons_Shoot() {
        //If the player is in a Menu than return
        if (playerManager.MenuOpen) { return; }
        if (FireSound != null) { FireSound.Play(); }
        if (TypeOfAmmo != Ammo_Types.Ammo.Extinguisher) { this.Stats.AmountCount.baseValue--; }
        
        //spawn bullet
        s_clone = (GameObject)Instantiate(s_bullet, s_Spawner.transform.position, s_Spawner.transform.rotation);
        s_clone.transform.SetParent(EffectTab.transform);
        s_clone.GetComponent<Bullet_Benaviour>().TypeOfAmmo = TypeOfAmmo;
        s_clone.GetComponent<Bullet_Benaviour>().SetPlayerManager(playerManager);
        s_clone.GetComponent<Bullet_Benaviour>().SetDamage(this.Stats.Damage.GetValue());

        if (CameraControl.isAiming) { ProjectileWeapons_RaycastProjectile(); }
        //shoot it
        else { s_clone.GetComponent<Rigidbody>().AddForce(s_Spawner.transform.forward * s_BulletSpeed, ForceMode.Impulse); }
        if (s_clone != null) { Destroy(s_clone, 2); }

        SetTextToAmount();
    }

    // Shoots a raycast from the camera position and in the camera's forward direction then fires a bullet in the direction of the ray's hit point
    //[ToggleGroup("WeaponTypeProjectile")]
    private void ProjectileWeapons_RaycastProjectile() {
        RaycastHit hit;

        if (Physics.Raycast(this.PlayerCamera.transform.position, this.PlayerCamera.transform.forward, out hit, 1000.0f)) {
            s_Spawner.transform.LookAt(hit.point);
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
    

    private void Reload() {
        if (TypeOfAmmo == Ammo_Types.Ammo.Extinguisher) { return; }

        NextTimeToFire = Time.time + (this.Stats.ReloadTime.GetValue() / 1.0f);
        if (this.Stats.AmountCount.GetValue() >= this.Stats.ClipSize.GetValue()) { return; }
        else {
            for (int i = 0; i < menuManager.Ammo_Slot.transform.childCount; i++) {
                if (menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>()) {
                    if (menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().TypeOfAmmo == this.TypeOfAmmo) {
                        int AmountNeeded = (int)this.Stats.ClipSize.GetValue() - (int)this.Stats.AmountCount.GetValue();
                        int AmountGot = 0;
                        if (menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().Amount >= AmountNeeded) {
                            AmountGot = AmountNeeded;
                            menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().Amount -= AmountNeeded;
                        }
                        else {
                            AmountGot = menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().Amount;
                            menuManager.Ammo_Slot.transform.GetChild(i).GetComponent<Ammo_Types>().Amount = 0;
                        }
                        this.Stats.AmountCount.baseValue += AmountGot;
                        SetTextToAmount();
                        if (ReloadSound != null) { ReloadSound.Play(); }
                        return;
                    }
                }
            }
        }
        SetTextToAmount();
    }

    public void SetTextToAmount() {

        if (TypeOfAmmo == Ammo_Types.Ammo.Extinguisher) {
            if (menuManager.Weapon_Slot.transform.GetChild(0) && menuManager.Weapon_Slot.transform.GetChild(0).transform.Find("Amount_Text").gameObject) {
                //need to change if Add a Swap function (Already have one with controllers)
                if (AmountText == null) { AmountText = menuManager.Weapon_Slot.transform.GetChild(0).transform.Find("Amount_Text").gameObject; }
                AmountText.GetComponent<Text>().text = " ";
            }
        }
        else if (menuManager.Weapon_Slot.transform.GetChild(0) && menuManager.Weapon_Slot.transform.GetChild(0).transform.Find("Amount_Text").gameObject) {
            //need to change if Add a Swap function (Already have one with controllers)
            if (AmountText == null) { AmountText = menuManager.Weapon_Slot.transform.GetChild(0).transform.Find("Amount_Text").gameObject; }
            AmountText.GetComponent<Text>().text = Stats.AmountCount.GetValue().ToString();
        }
        else { AmountText = null; }
    }

}
