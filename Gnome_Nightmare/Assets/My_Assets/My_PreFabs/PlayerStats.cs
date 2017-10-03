using UnityEngine;

public class PlayerStats : CharacterStats {

    public int PlayerLevel = 1;
    public int PlayerExperience = 0;
    public int MaxExperienceForLevel = 10;


    public void addExperience(int amount) {
        PlayerExperience += amount;
        if (PlayerExperience >= MaxExperienceForLevel) { PlayerLevelUp(); }
    }

    void PlayerLevelUp() {
        Damage.AddModifier(0.50f);
        Armor.AddModifier(0.50f);

        PlayerLevel += 1;
        PlayerExperience -= MaxExperienceForLevel;
        MaxExperienceForLevel = (PlayerLevel*PlayerLevel*4);

        if (PlayerExperience >= MaxExperienceForLevel) { PlayerLevelUp(); }
    }
}
