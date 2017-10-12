using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform playerTransform;
    public Vector2 playerRotation;
    public float height = 1.0f;
    public float depth = 0.0f;

    // Update is called once per frame
    void Update() {
        //If player is active, set the camera position and rotation
        if (playerTransform != null) {
            this.transform.position = playerTransform.position + new Vector3(0, height, 0) + playerTransform.transform.forward * depth;
            this.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0);
        }
    }
    //Set position of camera
    public void setPosition(Transform m_transform) {
        playerTransform = m_transform;
    }
    //Set rotation of camera
    public void setRotation(Vector2 m_rotation) {
        playerRotation = m_rotation;
    }
}