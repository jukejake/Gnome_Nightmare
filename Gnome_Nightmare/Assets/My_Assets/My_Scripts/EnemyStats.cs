using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {

    public int Experience = 1;

    public bool KillEnemy(float amount) {
        TakeDamage(amount);
        if (CurrentHealth <= 0.0f) { return true; }
        else { return false; }
    }

    public void OnDeath() {
        Destructible isDestructible = this.GetComponent<Destructible>();
        if (isDestructible != null) { isDestructible.Kill(); }
        else { Destroy(this.gameObject, 0.2f); }
        Debug.Log("Dead");
    }
}
