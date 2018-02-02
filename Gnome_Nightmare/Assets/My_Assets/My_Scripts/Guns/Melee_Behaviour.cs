using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Melee_Behaviour : SerializedMonoBehaviour {

    [TableList]
    public OdinTables.WeaponStatsTable Stats = new OdinTables.WeaponStatsTable();

    protected PlayerManager playerManager;
    protected MenuManager menuManager;
    protected Camera PlayerCamera;
    protected float NextTimeToFire = 0.0f;


    // Use this for initialization
    void Start () {
        Invoke("DelayedStart", 0.1f);
        PlayerCamera = Camera.main;
        playerManager = PlayerManager.instance;
        menuManager = MenuManager.instance;
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() { }

    // Update is called once per frame
    void Update ()
    {
        if (this.playerManager.MenuOpen) { return; }
        if (this.playerManager.player.GetComponent<PlayerStats>().isDead) { return; }
        MeleeWeapons_Update();
    }

    #region MeleeWeapons
    private bool weaponSwung = false;
    private int swingAnimCount = 0;
    private bool DownSwing = true;
    void MeleeWeapons_Update()
    {
        if (weaponSwung == false && (Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f) && Time.time >= NextTimeToFire) {
            NextTimeToFire = Time.time + (1.0f / this.Stats.FireRate.GetValue());
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
}
