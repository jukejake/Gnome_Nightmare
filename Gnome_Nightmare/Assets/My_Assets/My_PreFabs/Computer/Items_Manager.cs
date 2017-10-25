using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems; //used for drag and drop

public class Items_Manager : SerializedMonoBehaviour  {
    public static Items_Manager instance;
    void Awake() { instance = this; }

    [HorizontalGroup("Menus"), LabelWidth(100)]
    public GameObject DisplayContent;
    [HorizontalGroup("Menus"), LabelWidth(100)]
    public GameObject Details;

    private int CurrentlySelectedTab = 0;
    private int CurrentlySelectedItem = 0;
    private Vector3 vec30 = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 vec31 = new Vector3(0.9f, 0.9f, 0.9f); //Vector3(1.0f, 1.0f, 1.0f);

    public OdinTables.OnlineTable OnlineTable = new OdinTables.OnlineTable();

    // Update is called once per frame
    void Update() { }

    public int GetCurrentlySelectedTab() { return CurrentlySelectedTab; }
    public void SetCurrentlySelectedTab(int currentlySelectedTab) { CurrentlySelectedTab = currentlySelectedTab; SetDisplayedContent(); }
    public int GetCurrentlySelectedItem() { return CurrentlySelectedItem; }
    public void SetCurrentlySelectedItem(int currentlySelectedItem) { CurrentlySelectedItem = currentlySelectedItem; }


    private void SetDisplayedContent(){
        DestroyChildren();
        if (CurrentlySelectedTab == 0) {
            for (int i = 0; i < OnlineTable.Ammo.Count; i++) {
                GameObject Temp = Instantiate<GameObject>(OnlineTable.Ammo[i].Item);
                Temp.name = OnlineTable.Ammo[i].Item.name;
                Temp.transform.SetParent(DisplayContent.transform);
                Temp.GetComponent<RectTransform>().localPosition = vec30;
                Temp.GetComponent<RectTransform>().localScale = vec31;
            }
            Vector2 Temp2 = DisplayContent.GetComponent<RectTransform>().sizeDelta;
            Temp2.y = (210.0f+(34.0f*(Mathf.Clamp(OnlineTable.Ammo.Count-3, 0, 100000))));
            DisplayContent.GetComponent<RectTransform>().sizeDelta = Temp2;
        }
        else if (CurrentlySelectedTab == 1) { 
            for (int i = 0; i < OnlineTable.Parts.Count; i++) {
                GameObject Temp = Instantiate<GameObject>(OnlineTable.Parts[i].Item);
                Temp.name = OnlineTable.Parts[i].Item.name;
                Temp.transform.SetParent(DisplayContent.transform);
                Temp.GetComponent<RectTransform>().localPosition = vec30;
                Temp.GetComponent<RectTransform>().localScale = vec31;
            }
            Vector2 Temp2 = DisplayContent.GetComponent<RectTransform>().sizeDelta;
            Temp2.y = (210.0f + (34.0f * (Mathf.Clamp(OnlineTable.Ammo.Count - 3, 0, 100000))));
            DisplayContent.GetComponent<RectTransform>().sizeDelta = Temp2;
        }
        else if (CurrentlySelectedTab == 2) { 
            for (int i = 0; i < OnlineTable.Misc.Count; i++) {
                GameObject Temp = Instantiate<GameObject>(OnlineTable.Misc[i].Item);
                Temp.name = OnlineTable.Misc[i].Item.name;
                Temp.transform.SetParent(DisplayContent.transform);
                Temp.GetComponent<RectTransform>().localPosition = vec30;
                Temp.GetComponent<RectTransform>().localScale = vec31;
            }
            Vector2 Temp2 = DisplayContent.GetComponent<RectTransform>().sizeDelta;
            Temp2.y = (210.0f + (34.0f * (Mathf.Clamp(OnlineTable.Ammo.Count - 3, 0, 100000))));
            DisplayContent.GetComponent<RectTransform>().sizeDelta = Temp2;
        }
    }

    private void DestroyChildren() {
        foreach (Transform child in DisplayContent.transform) { Destroy(child.gameObject); }
    }


    public void SetInfo(int childIndex) {
        if (GetCurrentlySelectedTab() == 0) {
            Details.transform.GetChild(0).GetComponent<Text>().text = OnlineTable.Ammo[childIndex].Item.name;
            Details.transform.GetChild(1).GetComponent<Text>().text = OnlineTable.Ammo[childIndex].Summary;
            Details.transform.GetChild(2).GetComponent<Text>().text = OnlineTable.Ammo[childIndex].Price.ToString();
        }
        else if (GetCurrentlySelectedTab() == 1) {
            Details.transform.GetChild(0).GetComponent<Text>().text = OnlineTable.Parts[childIndex].Item.name;
            Details.transform.GetChild(1).GetComponent<Text>().text = OnlineTable.Parts[childIndex].Summary;
            Details.transform.GetChild(2).GetComponent<Text>().text = OnlineTable.Parts[childIndex].Price.ToString();
        }
        else if (GetCurrentlySelectedTab() == 2) { 
            Details.transform.GetChild(0).GetComponent<Text>().text = OnlineTable.Misc[childIndex].Item.name;
            Details.transform.GetChild(1).GetComponent<Text>().text = OnlineTable.Misc[childIndex].Summary;
            Details.transform.GetChild(2).GetComponent<Text>().text = OnlineTable.Misc[childIndex].Price.ToString();
        }
    }
}

