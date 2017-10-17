using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {

    public int Experience = 1;

    private void Update() {
       HealthBar();
    }

    //Applys damage to the enemy 
    //if it kills it return true
    public bool DamageEnemy(float amount) {
        TakeDamage(amount);
        if (CurrentHealth <= 0.0f) { return true; }
        else { return false; }
    }

    //On enemy death destory the enemy
    public void OnDeath() {
        //If the enemy is a Destructible object, destroy through destruction
        Destructible isDestructible = this.GetComponent<Destructible>();
        if (isDestructible != null) { isDestructible.Kill(); }
        //Else just destroy the gameObject
        else { Destroy(this.gameObject, 0.2f); }
    }

    private void HealthBar(){
        if (this.gameObject.transform.Find("HealthBar")) {
            float v_Health = CurrentHealth / MaxHealth;
            v_Health = Mathf.Clamp(v_Health, 0.0f, 1.0f);
            Vector3 Health = new Vector3(v_Health, 1.0f, 1.0f);
            this.gameObject.transform.Find("HealthBar").GetChild(0).GetComponent<RectTransform>().transform.localScale = Health;
            Health = new Vector3(1.0f-(v_Health), 1.0f, 1.0f);
            this.gameObject.transform.Find("HealthBar").GetChild(1).GetComponent<RectTransform>().transform.localScale = Health;
        }
    }
}
