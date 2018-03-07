using UnityEngine;
using UnityEngine.UI;

public class Ammo_Types : MonoBehaviour {

    public enum Ammo { None, Basic, Paintball, Staples, Arrow, Potato, Scrap, Fuel, Extinguisher };
    public Ammo TypeOfAmmo = Ammo.None;

    //[SerializeField]
    public int Amount;
    private GameObject AmountText;
    public float Damage;

    private void Start() {
        AmountText = this.transform.Find("Amount_Text").gameObject;
    }

    public void SetTextToAmount() {
        if (Amount <= 0) { Destroy(this.gameObject); }
        AmountText.GetComponent<Text>().text = Amount.ToString();
    }

}
