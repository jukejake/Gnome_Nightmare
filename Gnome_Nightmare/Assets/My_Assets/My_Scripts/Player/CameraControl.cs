using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public GameObject player;   // Player object   
    public GameObject endPoint; // The point where the camera translates to for A.D.S.
    public Transform lookAt;    // Player transform
    public Transform camTransform;  // Camera's transform
    public static Vector3 currentPos = new Vector3();   // Camera's current position (really for other classes to access)
    

    public float adsSpeed = 100.0f; // How fast the camera translation is
    public float distance = 90.0f;  // Distance from player
    public float currentX = 0.0f;   
    public float currentY = 0.0f;
    public static bool isAiming = false;    // Is the player currently aiming

    private Vector3 originalPos = new Vector3();    // Camera position before A.D.S.
    private Quaternion originalPlayerRot = new Quaternion();    // Player rotation before A.D.S.
    private const float Y_ANGLE_MIN = -5.0f;    // Minimum camera angle
    private const float Y_ANGLE_MAX = 35.0f;    // Maximum camera angle
    //private bool lookForward = true;
   

    void Start()
    {
        camTransform = this.transform;
        originalPos = camTransform.position;
    }

    //current values
    void Update()
    {
        float t = 0.0f;

        currentX += Input.GetAxis("Mouse X");
        currentY -= Input.GetAxis("Mouse Y");
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        currentPos = this.transform.position;

        //Movement of ads
        if (isAiming)
        {
            t = adsSpeed * Time.deltaTime;

            // set rotation and move to end position
            this.transform.rotation = Quaternion.Lerp(transform.rotation, endPoint.transform.rotation, t);
            this.transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position, t);

            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            player.transform.rotation = rotation;
        }
        else
        {
            t = adsSpeed * Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, originalPos, t);          
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log(t);
            originalPos = this.transform.position;
            originalPlayerRot = player.transform.rotation;
            isAiming = true;         
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            t = 0;
            player.transform.rotation = originalPlayerRot;
        }
    }

    //when the camera moves with player
    void LateUpdate()
    {
        if (!isAiming)
        {
            //put camera in the center then got angle where player is looking and throw camera behind that angle
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            camTransform.position = lookAt.position + rotation * dir;
            camTransform.LookAt(lookAt.position);
            originalPos = this.transform.position;
        }
        else
        {
            camTransform.forward = player.transform.forward;
        }
    }
}
