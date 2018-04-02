using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour {
	public Text bPrompt;
	public static bool isActive = true;     //	false means that the generator is broken
	public static bool eventIsActive = false;
	public static float timer = 10.0f;

	// Update is called once per frame
	void Update () {
		if (isActive)
		{
			//	play generator sound
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name == "Player" && eventIsActive)
		{
			if(timer <= 10.0f && timer >= 0.0f)
			{
				timer -= Time.deltaTime;
				bPrompt.text = timer.ToString();
			}
			else
			{
				Event_Manager.gennyReplaced = true;
			}
		}
	}
}
