using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamageInfo : MonoBehaviour {
    public int damage = 1;

   // private int numOfContacts = 0;

    void OnCollisionEnter(Collision col) {
        Debug.Log("Hit");
        
        if (transform.GetComponentInParent<Shoot>().weaponSwung) {
            string objectHit = col.collider.name;
            Debug.Log(gameObject.name + " hit " + objectHit);

            col.collider.GetComponent<EnemyStats>().DamageEnemy(damage);
        }
    }
}
