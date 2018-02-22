using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;


public class Event_Manager : MonoBehaviour
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
	public GameObject fire;
	public GameObject[] fireSpawns = new GameObject[5];
	public int nextEventRound = 0;  //	0 just for initialization
	public int fireCount = 10;	// current fire count for the barn fire
	public bool extinguisherHeld = false;	//	does a player have a fire extinguisher
	private int fireCountMax = 10;
	private int active;

	// Use this for initialization
	void Start()
	{
		createEvents();
		createAchievements();
		nextEventRound = genNextEventInfo(3, 4);

		Debug.Log("[ Name: " + ansiIt(getEventName(0, 0)) + " | Num in set " + getNumObjectives(0) + " ] [ X-" + getWaypointPos(0, 0, "x") + " | Y-" + getWaypointPos(0, 0, "y") + " | Z-" + getWaypointPos(0, 0, "z") + "]");
	}

	// Update is called once per frame
	void Update()
	{
		pos.x = this.transform.position.x;
		pos.y = this.transform.position.y;
		pos.z = this.transform.position.z;

		// check if the current round = anticpated event round
		if (GetComponent<EnemySpawners.Interface_SpawnTable>().CurrentLevel == nextEventRound)
		{
			
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
		setEventName(0, 1, "Put out the fire");
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
		if (!getEventStatus(0, 0) && isEventActive(0, 0))
		{
			// check if fire extinguisher is in any players' inventory
			if (extinguisherHeld)
			{
				moveOn(0, 0);
			}
		}
		else
		{
			if (!getEventStatus(0, 1) && isEventActive(0, 1))
			{
				// check if all of the fires have been put out
				if(fireCount == 0)
				{
					moveOn(0, 1);
				}
			}
		}
	}

	private void outageEvent()	//	handles outage event while it is active
	{
		if (!getEventStatus(1, 0) && isEventActive(1, 0))
		{

		}
		else
		{
			if (!getEventStatus(1, 1) && isEventActive(1, 1))
			{

			}
		}
	}

	private bool resetFireEvent()
	{
		fireCount = fireCountMax;
		if (!resetEvent(0))
		{
			return false;
		}
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