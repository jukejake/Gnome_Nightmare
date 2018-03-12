using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour {
	public GameObject lights;
	public static bool isActive = true;
	private float timer = 0.0f;
	private bool promptActive = false;

	private void Update()
	{
		if (!isActive)
		{
			// turn off all lights in map

		}	
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			if (Event_Manager.isEventActive(1, 2))
			{
				if (!promptActive)
				{
					GetComponent<ButtonPrompt>().prompt.text = "Hold 'E' To Turn On";
					promptActive = true;
				}
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			if (Event_Manager.isEventActive(1, 2))
			{
				if (Input.GetKey(KeyCode.E))
				{
					if (timer < 1.5f)
					{
						timer += Time.deltaTime;
					}
					else
					{
						isActive = true;
					}
				}
				else if (Input.GetKeyUp(KeyCode.E))
				{
					timer = 0.0f;
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (Event_Manager.isEventActive(1, 2) && promptActive)
			{
				GetComponent<ButtonPrompt>().prompt.text = "";
			}
		}
	}
}
