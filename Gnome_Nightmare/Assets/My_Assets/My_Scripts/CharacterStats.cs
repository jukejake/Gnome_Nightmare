using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public float MaxHealth = 100.0f;
    public float CurrentHealth { get; private set; }

    public Stat Damage;
    public Stat Armour;

    private void Awake() {
        CurrentHealth = MaxHealth;
    }
    void Start() {
        CurrentHealth = MaxHealth;
    }


    void FixedUpdate () {
        //HealOverTime(0.0001f);
    }

    //Adds health to character over time
    public void HealOverTime(float HealthHealed) {
        if (CurrentHealth + HealthHealed < MaxHealth) {
            CurrentHealth += HealthHealed;
        }
    }

    //Applys damage to character
    public void TakeDamage(float DamageTaken) {
        //Debug.Log("Damage");

        DamageTaken -= Armour.GetValue();
        DamageTaken = Mathf.Clamp(DamageTaken, 0, float.MaxValue);

        CurrentHealth -= DamageTaken;
    }

    public void FullHealth() {
        CurrentHealth = MaxHealth;
    }
}