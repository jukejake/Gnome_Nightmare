using UnityEngine;

public class PlayerStats : CharacterStats {

    public int PlayerLevel = 1;
    public int PlayerExperience = 0;
    public int MaxExperienceForLevel = 10;

    public float PlayerDamage;
    public float PlayerArmor;

    //Adds Experience to the player
    //If the player Experience is full, level up
    public void AddExperience(int amount) {
        PlayerExperience += amount;
        if (PlayerExperience >= MaxExperienceForLevel) { PlayerLevelUp(); }
    }

    //When the player levels up
    void PlayerLevelUp() {
        //Add player modifiers
        Damage.AddModifier(0.50f);
        Armour.AddModifier(0.50f);

        //Incress player Level
        PlayerLevel += 1;
        PlayerExperience -= MaxExperienceForLevel;
        MaxExperienceForLevel = (PlayerLevel*PlayerLevel*4);

        //If the player can still level up, do so
        if (PlayerExperience >= MaxExperienceForLevel) { PlayerLevelUp(); }

        PlayerDamage = Damage.GetValue();
        PlayerArmor = Armour.GetValue();
    }
}
