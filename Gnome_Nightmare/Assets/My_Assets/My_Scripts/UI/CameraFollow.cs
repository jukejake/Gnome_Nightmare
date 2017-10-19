using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform playerTransform;
    public Vector2 playerRotation;
    [Space]
    private bool is3rdPerson = false;
    public bool Switch3rdPerson = false;
    public float Height3rdPerson = 3.0f;
    public float Depth3rdPerson = -4.0f;
    public float Rot3rdPerson = 45.0f;
    public float CameraSpeed = 8.0f;
    private float Height = 0.50f;
    private float Depth = 0.0f;

    void Start() { }

    // Update is called once per frame
    void Update() {
        if (is3rdPerson == Switch3rdPerson) { SwitchPerspective(); }

        //If player is active, set the camera position and rotation
        if (is3rdPerson) { LerpCamera(); }
        else { WarpCamera(); }
    }
    //Set position of camera
    public void SetPosition(Transform m_transform) {
        playerTransform = m_transform;
    }
    //Set rotation of camera
    public void SetRotation(Vector2 m_rotation) {
        playerRotation = m_rotation;
    }

    public void SwitchPerspective() {
        if (is3rdPerson) {
            is3rdPerson = false;
            Switch3rdPerson = true;
            Height = 0.0f;
            Depth = 0.5f;
        }
        else {
            is3rdPerson = true;
            Switch3rdPerson = false;
            Height = Height3rdPerson;
            Depth = Depth3rdPerson;
            WarpCamera();
        }
    }

    public void WarpCamera() {
        if (playerTransform != null) {
            this.transform.position = playerTransform.position + new Vector3(0, Height, 0) + playerTransform.transform.forward * Depth;
            this.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0);
        }
    }
    public void LerpCamera() {
        if (playerTransform != null) {
            float tempFloatY;
            if (playerRotation.y > -10.0f) { tempFloatY = Mathf.Clamp(playerRotation.y * 0.1f, -(Height3rdPerson * 0.9f), (Height3rdPerson * 0.2f))-1.0f; }
            else { tempFloatY = Mathf.Clamp(playerRotation.y * 0.1f, -(Height3rdPerson * 0.9f), (Height3rdPerson * 0.5f)) - 1.0f; }
            float tempFloatX;
            if (playerRotation.y > 10.0f) { tempFloatX = Mathf.Clamp(playerRotation.y * 0.1f, (Depth3rdPerson * 0.75f), (-Depth3rdPerson) - 1.0f) + 1.0f; }
            else { tempFloatX = Mathf.Clamp(playerRotation.y * 0.1f, (Depth3rdPerson * 0.75f), 0.50f) + 1.0f; }

            if (tempFloatX > 0.0f) { tempFloatX  = -tempFloatX; }
            //Debug.Log("[" + playerRotation.y + "] [" + tempFloatY + "] [" + tempFloatX + "]");
            this.transform.position = Vector3.Lerp(this.transform.position,  (playerTransform.position + new Vector3(0, (Height + tempFloatY), 0) + playerTransform.transform.forward * (Depth-tempFloatX)), Time.deltaTime * CameraSpeed);
            this.transform.rotation = Quaternion.Euler(playerRotation.y, playerRotation.x, 0);
            //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(playerRotation.y, playerRotation.x, 0), Time.deltaTime * CameraSpeed);
        }
    }
}