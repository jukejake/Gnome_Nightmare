// line render learned from: https://www.youtube.com/watch?v=ZT8eutqzW5A&t=984s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSword : MonoBehaviour {
    LineRenderer lineRend;
    public Transform startPos;
    public Transform endPos;
    public float ignitionSpeed = 3.0f;
    public static bool isIgnited = false;

    private float intensity = 21.66f;
    private Vector3 endPosition;

	// Use this for initialization
	void Start () {
        lineRend = transform.GetComponent<LineRenderer>();
        endPosition = endPos.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        // sets start and end positions of the line render
        // these 2 lines must be at top to update properly
        lineRend.SetPosition(0, startPos.localPosition);
        lineRend.SetPosition(1, endPos.localPosition);

        if (isIgnited)
        {
            endPos.localPosition = Vector3.Lerp(endPos.localPosition, endPosition, Time.deltaTime * ignitionSpeed);
        }
        else
        {
            endPos.localPosition = Vector3.Lerp(endPos.localPosition, startPos.localPosition, Time.deltaTime * ignitionSpeed);

            // update end position vector
            endPosition = new Vector3(startPos.transform.localPosition.x + 1.1f, startPos.transform.localPosition.y + 25.0f, startPos.transform.localPosition.z + 1.0f);
         }
    }
}
