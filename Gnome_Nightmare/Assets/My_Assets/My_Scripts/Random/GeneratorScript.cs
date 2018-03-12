using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour {
	public Text bPrompt;
	public GameObject lights;
	public static bool isActive = true;
	private float timer = 0.0f;
	private bool lightsAreOff = false;

	private void Update()
	{
		if (!isActive && !lightsAreOff)
		{
			// turn off all lights in map
			lights.GetComponent<UnHide>().Hide();
			lightsAreOff = true;
		}
		else if(isActive && lightsAreOff)
		{
			lights.GetComponent<UnHide>().View();
			lightsAreOff = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			if (Event_Manager.isEventActive(1, 2))
			{
				if (!ButtonPrompt.promptActive)
				{
					bPrompt.text = "Hold 'E' To Turn On";
					ButtonPrompt.promptActive = true;
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
			if (Event_Manager.isEventActive(1, 2) && ButtonPrompt.promptActive)
			{
				GetComponent<ButtonPrompt>().prompt.text = "";
			}
		}
	}
}
