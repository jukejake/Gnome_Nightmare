using UnityEngine;

public class Bullet_Benaviour : MonoBehaviour {


    public GameObject ImpactEffect;
    public int maxBulletHoleCount = 15;
    private static int bulletHoleCount = 0;
    public GameObject bulletHole;

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
        else if (bulletHoleCount < maxBulletHoleCount) {
            RaycastHit hit;
            if (Physics.Raycast((transform.position - (transform.forward * 1.0f )), transform.forward, out hit)) {
                Debug.Log("Point of contact: " + hit.point);
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Vector3 bulletHoleSpawnPos = new Vector3(hit.point.x, hit.point.y, hit.point.z) + (hit.normal * 0.05f);
                GameObject s_bulletHole = Instantiate(bulletHole, bulletHoleSpawnPos, rot);
                bulletHoleCount++;
                Destroy(s_bulletHole, 5.0f);
            }
        }
        //Spawn a Particle System at where the Raycast hit
        GameObject ImpactAtHit = Instantiate(ImpactEffect, this.transform.position, Quaternion.LookRotation(-this.transform.forward));
        Destroy(ImpactAtHit, 2.0f);
        Destroy(this.gameObject);
    }
    private void OnDestroy() {
        bulletHoleCount--;
    }
}
