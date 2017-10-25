using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Benaviour : MonoBehaviour {


    public GameObject ImpactEffect;

    private PlayerManager playerManager;
    private float Damage = 0.0f;

    public void SetDamage(float damage) { Damage = damage; }
    public void SetPlayerManager(PlayerManager v_PlayerManager) { playerManager = v_PlayerManager; }

    private void OnTriggerEnter(Collider other) {
        if (playerManager == null) { return; }

        EnemyStats EnemyStat = other.transform.GetComponent<EnemyStats>();
        //If what was hit has an EnemyStat
        if (EnemyStat != null) {
            //Applys damage to the enemy and if it kills it, destroy the enemie
            if (EnemyStat.DamageEnemy(Damage + playerManager.GetComponent<PlayerStats>().Damage.GetValue())) {
                playerManager.GetComponent<PlayerStats>().AddExperience(EnemyStat.Experience);
                playerManager.GetComponent<PlayerStats>().AddPoints(EnemyStat.Points);
                //Destroys the enemy
                EnemyStat.OnDeath();
            }
        }
        //Spawn a Particle System at where the Raycast hit
        GameObject ImpactAtHit = Instantiate(ImpactEffect, this.transform.position, Quaternion.LookRotation(-this.transform.forward));
        Destroy(ImpactAtHit, 2.0f);
        Destroy(this.gameObject);
    }
}
