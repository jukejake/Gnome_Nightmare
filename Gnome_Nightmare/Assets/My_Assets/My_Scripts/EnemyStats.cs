using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {

    public int Experience = 1;

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
}
