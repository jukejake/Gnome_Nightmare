using UnityEngine;

public class Load_Menu : MonoBehaviour {

    GameObject EmptyWorld;
    public GameObject Menu_Prefab;
    public GameObject Items_Prefab;
    public GameObject Player_Prefab;
    //GameObject Inventory = (GameObject)Instantiate(Resources.Load("MyPrefab"));


    // Use this for initialization
    void Start () {
        EmptyWorld = new GameObject("World");
        GameObject Menu = (GameObject)Instantiate(Menu_Prefab);
        Menu.transform.SetParent(EmptyWorld.transform);
        Menu.name = "Menu";
        GameObject Items = (GameObject)Instantiate(Items_Prefab);
        Items.transform.SetParent(EmptyWorld.transform);
        Items.name = "Items";
        GameObject Player = (GameObject)Instantiate(Player_Prefab);
        Player.transform.SetParent(EmptyWorld.transform);
        Player.name = "Player";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
