using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyObjectBoundaryColliderCheck : MonoBehaviour {
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "Player")
		{
			if (transform.name == "Entrance")
			{
				Event_Manager.playerInEntranceBoundary = true;
			}
			else if(transform.name == "Generator Room")
			{
				Event_Manager.playerInGRBoundary = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == "Player")
		{
			Event_Manager.playerInEntranceBoundary = false;
		}
	}
}
