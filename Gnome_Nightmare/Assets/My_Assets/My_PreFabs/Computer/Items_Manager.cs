using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Items_Manager : SerializedMonoBehaviour {

    private int CurrentlySelectedTab = 0;
    private int CurrentlySelectedItem = 0;

    public OdinTables.OnlineTable OnlineTable = new OdinTables.OnlineTable();

    // Update is called once per frame
    void Update () {
		
	}

    public int  GetCurrentlySelectedTab() { return CurrentlySelectedTab; }
    public void SetCurrentlySelectedTab( int currentlySelectedTab) { CurrentlySelectedTab = currentlySelectedTab; }
    public int  GetCurrentlySelectedItem() { return CurrentlySelectedItem; }
    public void SetCurrentlySelectedItem(int currentlySelectedItem) { CurrentlySelectedItem = currentlySelectedItem; }
}

