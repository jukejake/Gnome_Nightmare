using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random121 : MonoBehaviour {
    
    [Range(-100.0f, 100.0f)]
    public float Weight = 0f;


    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            System.Array values = System.Enum.GetValues(typeof(KeyCode));
            foreach (KeyCode code in values)
            {
                if (Input.GetKeyDown(code)) { print(System.Enum.GetName(typeof(KeyCode), code)); }
            }
        }
    }

    void FixedUpdate() {

    }
}
