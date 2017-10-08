using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;
    void Awake() { instance = this; }
    public GameObject player;

    public bool MenuOpen = false;
    private float MenuTimer = 0.0f;
    private MenuManager menuManager;
    private CraftingManager craftingManager;

    private void Start() {
        menuManager = MenuManager.instance;
        craftingManager = CraftingManager.instance;
    }

    private void Update() {
        if (MenuTimer > 0.0f) {
            MenuTimer -= Time.deltaTime;
            Mathf.Clamp(MenuTimer, 0.0f, 30.0f);
        }
        else if (MenuTimer < 0.0f) { MenuTimer = 0.0f; }
        if ((Input.GetButton("E") || Input.GetButton("Tab")) && MenuTimer == 0.0f) {
            if (MenuOpen) { ExitMenus(); }
        }
            //

    }





    public void KillPlayer() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void OnTriggerStay(Collider other) {
        //If the player collides with an NPC and presses "E" to interact
        if ((Input.GetButton("E")|| Input.GetButton("Tab")) && MenuTimer == 0.0f) {
            MenuTimer = 0.3f;
            if (other.tag == "Crafting_Table") {
                if (other.gameObject.transform.GetChild(0).gameObject.activeSelf) { Debug.Log(other.gameObject.transform.GetChild(0).gameObject.name); }

                if (other.gameObject.transform.GetChild(0).gameObject.activeSelf) {
                    MenuOpen = false;
                    menuManager.EnableGraphicRaycaster(false);
                    other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                } else {
                    MenuOpen = true;
                    menuManager.EnableGraphicRaycaster(true);
                    other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            else if (other.tag == "NPC") {

            }
            else { }
        }
    }
    private void ExitMenus() {
        if (MenuOpen) {
            MenuOpen = false;
            menuManager.EnableGraphicRaycaster(false);
            craftingManager.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}