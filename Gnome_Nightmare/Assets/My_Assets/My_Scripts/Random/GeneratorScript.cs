using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour {
	public Text bPrompt;
	public GameObject lights;
	public GameObject computer;
	public static bool isActive = true;
	private float timer = 0.0f;
	private bool lightsAreOff = false;

	private void Update()
	{
		if (!isActive && !lightsAreOff)
		{
			computer.GetComponent<UnHide>().Hide();
			lights.GetComponent<UnHide>().Hide();
			lightsAreOff = true;
		}
		else if(isActive && lightsAreOff)
		{
			computer.GetComponent<UnHide>().View();
			lights.GetComponent<UnHide>().View();
			lightsAreOff = false;
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.name == "Player")
		{
			if (Event_Manager.isEventActive(1, 2))
			{
				if (!ButtonPrompt.promptActive)
				{
					bPrompt.text = "Press 'E' To Turn On";
					ButtonPrompt.promptActive = true;
				}
			}
		}
	}

	private void OnCollisionStay(Collision other)
	{
		if (other.gameObject.name == "Player")
		{
			if (Event_Manager.isEventActive(1, 2))
			{
				if (Input.GetKeyDown(KeyCode.E))
				{
					isActive = true;
				}
				else if (Input.GetKeyUp(KeyCode.E))
				{
					timer = 0.0f;
				}
			}
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.name == "Player")
		{
			if (ButtonPrompt.promptActive)
			{
				bPrompt.text = "";
				ButtonPrompt.promptActive = false;
			}
		}
	}
}
