using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Behaviour : MonoBehaviour {

    public float Damage = 1.0f;
    public float Range = 100.0f;
    public float FireRate = 1.0f;
    public float ImpactForce = 1.0f;
    public Camera FpsCamera;
    //public ParticleSystem MuzzleFlash;
    //public GameObject ImpactEffect;

    // Use this for initialization
    void Start () {
        FpsCamera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1")) { Shoot(); }
        if (Input.GetButtonDown("Fire2")) { Reload(); }
    }

    void Shoot() {
        RaycastHit hit;
        if (Physics.Raycast(FpsCamera.transform.position, FpsCamera.transform.forward, out hit, Range)) {
            Debug.Log(hit.transform.name);
        }
    }

    void Reload() {

    }
}
