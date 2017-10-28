using UnityEngine;

public class Eyes_Follow_Cursor : MonoBehaviour {
	void Update () { this.transform.rotation = Camera.main.transform.rotation; }
}
