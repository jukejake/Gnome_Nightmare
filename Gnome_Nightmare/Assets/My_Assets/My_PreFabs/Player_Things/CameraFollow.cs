using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform playerTransform;
    public Vector2 playerRotation;
    public float height = 1.0f;
    public float depth = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            this.transform.position = playerTransform.position + new Vector3(0, height, 0) + playerTransform.transform.forward * depth;
            //this.transform.rotation = Quaternion.Euler(playerRotation.y, this.transform.rotation.x, 0.0f);
            //this.transform.Rotate(0, playerRotation.x*100.0f, 0);
            this.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0);
        }
    }

    public void setPosition(Transform m_transform)
    {
        playerTransform = m_transform;
    }
    public void setRotation(Vector2 m_rotation)
    {
        playerRotation = m_rotation;
    }
}