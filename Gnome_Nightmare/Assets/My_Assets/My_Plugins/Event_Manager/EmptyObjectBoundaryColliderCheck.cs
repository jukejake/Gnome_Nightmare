using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyObjectBoundaryColliderCheck : MonoBehaviour {
	public enum Area { None, BunkerEntrance, GeneratorRoom };
	public Area areaType;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Event_Manager.playerInBoundary = true;
			Event_Manager.boundaryType = areaType;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			Event_Manager.playerInBoundary = false;
			Event_Manager.boundaryType = Area.None;
		}
	}
}
