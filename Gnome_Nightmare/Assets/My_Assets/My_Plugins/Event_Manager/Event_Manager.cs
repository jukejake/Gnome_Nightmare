using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;


public class Event_Manager : SerializedMonoBehaviour
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Vec3
	{
		public float x, y, z;
	}
	[DllImport("EventManager")]
	public static extern void DestroyManager();
	[DllImport("EventManager")]
	public static extern void initAccess();    // this must be called once to initialize the event manager access pointer
	[DllImport("EventManager")]
	public static extern void createEventSet();
	[DllImport("EventManager")]
	public static extern void newEvent(int eventSetIndex);
	[DllImport("EventManager")]
	public static extern void moveOn(int eventSetIndex, int eventIndex);       // move to the next MyEvent in the hierarchy
	[DllImport("EventManager")]
	public static extern void addWaypoint(int eventSetIndex, Vec3 waypoint);   // add a waypoint to the waypoint vector
	[DllImport("EventManager")]
	public static extern void addWaypointInd(int eventSetIndex, float x, float y, float z);    // same as addWaypoint except pass individual values
	[DllImport("EventManager")]
	public static extern void addAchievement(string s);
	[DllImport("EventManager")]
	public static extern bool checkRadius(int eventSetIndex, int eventIndex, Vec3 pos, int waypointIndex, float radius);   // check the radius relative to a waypoint
	[DllImport("EventManager")]
	public static extern int genNextEventInfo(int min, int max);
	[DllImport("EventManager")]
	public static extern bool resetEvent(int index);
	[DllImport("EventManager")]
	public static extern void setActiveEventSet(int index, bool status);
	[DllImport("EventManager")]
	public static extern void setEventSetName(int index, string n);
	[DllImport("EventManager")]
	public static extern void setEventName(int eventSetIndex, int eventIndex, string n);
	[DllImport("EventManager")]
	public static extern void setParent(int eventSetIndex, int eventIndex, int parentEventIndex);  // set parent to the current MyEvent
	[DllImport("EventManager")]
	public static extern void setImmediateChild(int eventSetIndex, int eventIndex, int parentEventIndex);  // set the immediate (next) MyEvent in the hierarchy
	[DllImport("EventManager")]
	public static extern void setIconFilepath(int eventSetIndex, int eventIndex, char f);
	[DllImport("EventManager")]
	public static extern void setLast(int eventSetIndex, int eventIndex);  // sets the MyEvent given as the last in its hierarchy
	[DllImport("EventManager")]
	public static extern void setAchievementStatusSearch(string name, bool stat);
	[DllImport("EventManager")]
	public static extern void setAchievementStatus(int index, bool stat);
	[DllImport("EventManager")]
	public static extern void setAchievementDesc(int index, string desc);
	[DllImport("EventManager")]
	public static extern void setLastRoundEventHappened(int r);
	[DllImport("EventManager")]
	public static extern bool getEventStatus(int eventSetIndex, int eventIndex);       // returns whether the MyEvent has been completed or not
	[DllImport("EventManager")]
	public static extern bool getAchievementStatus(string name);
	[DllImport("EventManager")]
	public static extern string getAchievementDesc(int index);
	[DllImport("EventManager")]
	public static extern IntPtr getAchievementName(int achievementNum);
	[DllImport("EventManager")]
	public static extern IntPtr getEventName(int eventSetIndex, int eventIndex);
	[DllImport("EventManager")]
	public static extern bool isEventActive(int eventSetIndex, int eventIndex);
	//public static extern int getActiveEventSet();
	[DllImport("EventManager")]
	public static extern IntPtr getIconFilePath(int eventSetIndex, int eventIndex);
	[DllImport("EventManager")]
	public static extern float getWaypointPos(int eventSetIndex, int eventIndex, string whichAxis);
	[DllImport("EventManager")]
	public static extern int getNumObjectives(int eventSetIndex);   //returns number of objectives in a particular event set
	[DllImport("EventManager")]
	public static extern int getLastRoundEventHappened();
	[DllImport("EventManager")]
	public static extern int getNextEvent();
	[DllImport("EventManager")]
	public static extern int getLastEvent();

	Vec3 pos;
	public Text prompt;
	public MenuManager menu;
	public GameObject firePrefab;
	private static int fireCountMax = 10;
	public static int fireCount = 0;   // current fire count for the barn fire
	public static int nextEventRound = 0;  //	0 just for initialization
	public static int active = 0;   // 100 for null (essentially, not literally)
	public static int eventRoundProgress = 0;	// how many rounds passed since event started
	public static int eventDuration = 2;	// max 2 rounds to complete and event
	private static bool fireSpawned = false;
	private GameObject fire;

	// Use this for initialization
	void Start()
	{
		createEvents();
		Debug.Log("Events created successfully!");
		createAchievements();
		Debug.Log("Achievements created successfully!");
		nextEventRound = 1;

		//Debug.Log("[ Name: " + ansiIt(getEventName(0, 0)) + " | Num in set " + getNumObjectives(0) + " ] [ X-" + getWaypointPos(0, 0, "x") + " | Y-" + getWaypointPos(0, 0, "y") + " | Z-" + getWaypointPos(0, 0, "z") + "]");
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log("Next Event Round: " + nextEventRound);
		Debug.Log("Current Event:0: " + ansiIt(getEventName(0, 0)));
		Debug.Log("Num of events in set 1: " + getNumObjectives(0));

		// check if the current round = anticpated event round
		if (EnemySpawners.Interface_SpawnTable.instance.CurrentLevel == nextEventRound)
		{
			nextEventRound = getNextEvent();
			//active = getNextEvent();
		}

		// if fire event is active
		if (active == 0)
		{
			fireEvent();
		}

		// if outage event is active
		if(active == 1)
		{
			outageEvent();
		}

        if (menu == null) {
            menu = GameObject.FindObjectOfType(typeof(MenuManager)) as MenuManager;
        }
	}

	[Button]
	public void GetSecond() {

		Debug.Log("Current Event:1: " + ansiIt(getEventName(0, 0)));
		Debug.Log("Current Event:2: " + ansiIt(getEventName(1, 0)));
	}

	private void OnDestroy()
	{
		DestroyManager();
	}

	private string ansiIt(IntPtr str)
	{
		return Marshal.PtrToStringAnsi(str);
	}

	private void createEvents()
	{
		initAccess();

		// barn fire event
		createEventSet();
		setEventSetName(0, "Barn Fire");
		newEvent(0);
		setEventName(0, 0, "Find the fire extinguisher");
		setParent(0, 0, -1);
		newEvent(0);
		setEventName(0, 1, "Put out all the fires");
		setParent(0, 1, 0);

		// outage event
		createEventSet();
		setEventSetName(1, "Outage");
		newEvent(1);
		setEventName(1, 0, "Get to the bunker!");
		setParent(1, 0, -1);
		newEvent(1);
		setEventName(1, 1, "Find the generator");
		setParent(1, 1, 0);
		newEvent(1);
		setEventName(1, 2, "Turn on the generator");
		setParent(1, 2, 1);
	}

	private void createAchievements()
	{
		addAchievement("Up and Coming");
		setAchievementDesc(0, "Complete the Tutorial");

		//if (!checkForAchieveList()) {
		//	for (int i = 0; i < getNumAchievements(); i++)
		//	{
		//		setAchievementStatus(i, false);
		//	}
		//}
	}

	private void fireEvent()	//	handles fire event while it is active
	{
		if (eventRoundProgress < 2)
		{
			if (!fireSpawned)
			{
				for (int i = 0; i < fireCountMax; i++)
				{
					fire = Instantiate(firePrefab, transform.GetChild(0).transform.GetChild(i).transform);
				}
				setActiveEventSet(0, true);
				fireSpawned = true;
			}

			if (!getEventStatus(0, 0) && isEventActive(0, 0))
			{
				//prompt.text = ansiIt(getEventName(0, 0));
				prompt.text = "Find The Fire Extinguisher";

                Component[] Slots;
                if (menu != null) { 
                    // check if fire extinguisher is in any players' inventory	
                    Slots = menu.Inventory_Slot.GetComponentsInChildren<Drag_Inventory>();	
                    foreach (Drag_Inventory slots in Slots) {
                        if (slots.typeOfItem == Drag_Inventory.Slot.Extinguisher) { moveOn(0, 0); }
                    }
                    // check if fire extinguisher is in any players' inventory		
                    Slots = menu.Weapon_Slot.GetComponentsInChildren<Drag_Inventory>();
                    foreach (Drag_Inventory slots in Slots) {
                        if (slots.typeOfItem == Drag_Inventory.Slot.Extinguisher) { moveOn(0, 0); }
                    }
                }
			}
			else if (!getEventStatus(0, 1) && isEventActive(0, 1))
			{
				prompt.text = ansiIt(getEventName(0, 1));

				// check if all of the fires have been put out
				if (fireCount < 1)
				{
					nextEventRound = genNextEventInfo(2, 5);
					moveOn(0, 1);
				}
			}
		}
	}

	private void outageEvent()	//	handles outage event while it is active
	{
		if (!getEventStatus(1, 0) && !isEventActive(1, 0))
		{
			setActiveEventSet(0, true);
		}
		else if (!getEventStatus(1, 0) && isEventActive(1, 0))
		{
			// check if a player is in the bunker space

		}
		else if (!getEventStatus(1, 1) && isEventActive(1, 1))
		{
			// check if a player has reached the generator room

		}
		else if (!getEventStatus(1, 2) && isEventActive(1,2)) { 
			// check the generator status

		}
	}

	private bool resetFireEvent()
	{
		fireCount = fireCountMax;
		if (!resetEvent(0))
		{
			return false;
		}
		fireSpawned = false;
		return true;	// returns true if there are no errors
	}

	private bool resetOutageEvent()
	{
		if (!resetEvent(1))
		{
			return false;
		}
		return true;    // returns true if there are no errors
	}
}