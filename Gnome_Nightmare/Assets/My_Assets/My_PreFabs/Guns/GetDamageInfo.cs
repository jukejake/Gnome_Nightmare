using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamageInfo : MonoBehaviour {
    public GameObject holder;
    public static string objectHit; // contains the name of the object hit
    public int damage;

   // private int numOfContacts = 0;

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Hit");

        if (transform.GetComponentInParent<Shoot>().weaponSwung)
        {
            objectHit = col.collider.name;
            Debug.Log(gameObject.name + " hit " + objectHit);

            col.collider.GetComponent<EnemyStats>().DamageEnemy(damage);
        }
    }
}
