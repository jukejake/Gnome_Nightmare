using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour {

    public Stat Damage;
    public Stat Armor;
    public Stat FireRate;

    private void Start() {
        //temporary as you can just keep increassing the level by crafting
        Damage.AddModifier(Random.Range(1, 10) * 0.1f);
        Armor.AddModifier(Random.Range(1, 10) * 0.1f);
        FireRate.AddModifier(Random.Range(1, 10) * 0.1f);
    }

}
