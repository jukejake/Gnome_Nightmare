using UnityEngine;

public class PlayerStats : CharacterStats {

    public int PlayerLevel = 1;
    public int PlayerExperience = 0;
    public int MaxExperienceForLevel = 10;

    public bool isDead = false;
    [Space]
    public float CurrentDamage;
    public float CurrentArmor;


    private void Update() {
        HealthBar();
    }

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
        FullHealth();
        //Armour.AddModifier(0.50f);

        //Incress player Level
        PlayerLevel += 1;
        PlayerExperience -= MaxExperienceForLevel;
        MaxExperienceForLevel = (PlayerLevel*PlayerLevel*4);

        //If the player can still level up, do so
        if (PlayerExperience >= MaxExperienceForLevel) { PlayerLevelUp(); }

        CurrentDamage = Damage.GetValue();
        CurrentArmor = Armour.GetValue();
    }

    private void HealthBar(){
        if (CurrentHealth <= 0.0f) { isDead = true; }
        if (this.gameObject.transform.Find("HealthBar")) {
            float v_Health = CurrentHealth / MaxHealth;
            v_Health = Mathf.Clamp(v_Health, 0.0f, 1.0f);
            Vector3 Health = new Vector3(v_Health, 1.0f, 1.0f);
            this.gameObject.transform.Find("HealthBar").GetChild(0).GetComponent<RectTransform>().transform.localScale = Health;
            Health = new Vector3(1.0f-(v_Health), 1.0f, 1.0f);
            this.gameObject.transform.Find("HealthBar").GetChild(1).GetComponent<RectTransform>().transform.localScale = Health;
        }
        if (this.gameObject.transform.Find("HealthBarTop")) {
            float v_Health = CurrentHealth / MaxHealth;
            v_Health = Mathf.Clamp(v_Health, 0.0f, 1.0f);
            Vector3 Health = new Vector3(v_Health, 1.0f, 1.0f);
            this.gameObject.transform.Find("HealthBarTop").GetChild(0).GetComponent<RectTransform>().transform.localScale = Health;
            Health = new Vector3(1.0f-(v_Health), 1.0f, 1.0f);
            this.gameObject.transform.Find("HealthBarTop").GetChild(1).GetComponent<RectTransform>().transform.localScale = Health;
        }
    }
}
