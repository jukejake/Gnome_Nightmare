using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;


public class Event_Manager : MonoBehaviour {
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec3 {
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
    public static extern void setAchievementStatus(string name, bool stat);
    [DllImport("EventManager")]
    public static extern bool getEventStatus(int eventSetIndex, int eventIndex);       // returns whether the MyEvent has been completed or not
    [DllImport("EventManager")]
    public static extern bool getAchievementStatus(string name);
    [DllImport("EventManager")]
    public static extern IntPtr getAchievementName(int achievementNum);
    [DllImport("EventManager")]
    public static extern IntPtr getEventName(int eventSetIndex, int eventIndex);
    [DllImport("EventManager")]
    public static extern IntPtr getIconFilePath(int eventSetIndex, int eventIndex);
    [DllImport("EventManager")]
    public static extern float getWaypointPos(int eventSetIndex, int eventIndex, string whichAxis);
    [DllImport("EventManager")]
    public static extern int getNumObjectives(int eventSetIndex);	//returns number of objectives in a particular event set


    Vec3 pos;
    // Use this for initialization
    void Start () {
        //DestroyManager();
        initAccess();
        createEventSet();
        newEvent(0);
        setEventName(0, 0, "The first event");
        setLast(0, 0);
        setParent(0, 0, -1);
        addWaypointInd(0, 10.0f, 0.01f, 2.0f);
        addAchievement("Achievement Achievement Achievement");
        setAchievementStatus(Marshal.PtrToStringAnsi(getAchievementName(0)), true);
        Debug.Log("[ Name: " + Marshal.PtrToStringAnsi(getEventName(0, 0)) + " | Num in set " + getNumObjectives(0) + " ] [ X-" + getWaypointPos(0, 0, "x") + " | Y-" + getWaypointPos(0, 0, "y") + " | Z-" + getWaypointPos(0, 0, "z") + "]");
    }
	
	// Update is called once per frame
	void Update () {
        pos.x = this.transform.position.x;
        pos.y = this.transform.position.y;
        pos.z = this.transform.position.z;
        if (checkRadius(0,0, pos, 0,3)) { Debug.Log("Within Range"); }
        if (getAchievementStatus(Marshal.PtrToStringAnsi(getAchievementName(0)))) { Debug.Log("Achievement: " + Marshal.PtrToStringAnsi(getAchievementName(0))); }
	}
    private void OnDestroy() {
        DestroyManager();
    }
}
