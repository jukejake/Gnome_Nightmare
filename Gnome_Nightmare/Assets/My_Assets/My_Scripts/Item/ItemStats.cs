using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour {
    
    public Stat Damage;
    public Stat Armour;
    public Stat FireRate;

    private void Start() {
        //temporary as you can just keep increassing the level by crafting
        Damage.AddModifier(Random.Range(1, 10) * 0.1f);
        Armour.AddModifier(Random.Range(1, 10) * 0.1f);
        FireRate.AddModifier(Random.Range(1, 10) * 0.1f);
    }

    public void AddAllModifiers(ItemStats AddStats) {
        Damage.AddModifier(AddStats.Damage.GetModifierValue());
        Armour.AddModifier(AddStats.Armour.GetModifierValue());
        FireRate.AddModifier(AddStats.FireRate.GetModifierValue());
    }
    public void AddAllModifiersByPercent(ItemStats AddStats, float percent) {
        Mathf.Clamp(percent, 0.0f, 1.0f);
        Damage.AddModifier(AddStats.Damage.GetModifierValue() * percent);
        Armour.AddModifier(AddStats.Armour.GetModifierValue() * percent);
        FireRate.AddModifier(AddStats.FireRate.GetModifierValue() * percent);
    }

    public void SquishAllModifierValues() {
        Damage.SquishModifierValue();
        Armour.SquishModifierValue();
        FireRate.SquishModifierValue();
    }
    public void SquishAllValuesToBase() {
        Damage.SquishValueToBase();
        Armour.SquishValueToBase();
        FireRate.SquishValueToBase();
    }
}
