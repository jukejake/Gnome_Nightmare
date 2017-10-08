using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes_Follow_Cursor : MonoBehaviour {

	//CharacterController Player;
	public GameObject Eyes;
    public GameObject ThiredPerson;
    [Range(0.05f, 10.0f)]
    public float Sensitivity = 1.0f;
	private float rotX;
	private float rotY;
    private float rotY2;

    // Use this for initialization
    void Start () {
		//Player = gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		rotX = Input.GetAxis("Mouse X")*20.0f*Sensitivity;
		rotY -= Input.GetAxis("Mouse Y")*20.0f*Sensitivity;
		rotY = Mathf.Clamp (rotY, -60f, 50f);
		Eyes.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
        rotY2 = Mathf.Clamp(rotY, -0f, 50f);
        ThiredPerson.transform.localRotation = Quaternion.Euler(rotY2, 0, 0);
        transform.Rotate(0, rotX, 0);
	}
}
