using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour {
	public Text bPrompt;
	public GameObject smokeParticles;
	public bool isNew;
	public static bool isActive = true;     //	false means that the generator is broken
	public static bool eventIsActive = false;
	public bool carrying = false;
	public static float timer = 10.0f;
	private bool smokeSpawned = false;
	private bool promptSpawned = false;

	// Update is called once per frame
	void Update () {
		if (!isNew && isActive)
		{
			//	play generator sound
		}
		else if(!isNew && !isActive)
		{
			if (!smokeSpawned)
			{
				Instantiate(smokeParticles, transform);
				smokeSpawned = true;
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Player" && eventIsActive)
		{
			if(!isNew && !carrying)
			{
				if (!promptSpawned)
				{
					bPrompt.text = "This Generator is Broken, Find The Replacement...";
					promptSpawned = true;
					ButtonPrompt.promptActive = true;
				}
			}
			else if (!isNew && carrying)
			{
				Event_Manager.instance.newGennyPlaced = true;
			}
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name == "Player" && eventIsActive)
		{
			if (!isNew && Event_Manager.isEventActive(2, 2))
			{
				if (timer <= 10.0f && timer >= 0.0f)
				{
					timer -= Time.deltaTime;
					bPrompt.text = timer.ToString();
				}
				else if (timer < 0.0f)
				{
					Event_Manager.gennyReplaced = true;
					Destroy(smokeParticles);
					smokeSpawned = false;
				}
			}
			else
			{
				if (!promptSpawned && !ButtonPrompt.promptActive && !carrying)
				{
					bPrompt.text = "Press 'E' to Carry";
					promptSpawned = true;
					ButtonPrompt.promptActive = false;
				}

				if (Input.GetKeyDown(KeyCode.E))
				{
					bPrompt.text = "";
					promptSpawned = false;
					ButtonPrompt.promptActive = false;
					carrying = true;
				}
			}
		}
	}
}
