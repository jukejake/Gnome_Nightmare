using UnityEngine.EventSystems;
using UnityEngine;

public class Player_Movement : MonoBehaviour {

	public float moveSpeed = 10.0f;
    public float SensitivityXAxis = 15.0f;
    public float SensitivityYAxis = 10.0f;
    public Vector3 Jump = new Vector3(3.0f, 0.0f, 10.0f); //MinJump,JumpPressure, MaxJumpPressure
    
    private Rigidbody m_Rigidbody;
    //private AudioSource m_audioSource;

	private Vector3 moveDirection = Vector3.zero;
	private float rotX;
    private float rotY;
    private float RotationNeededForCamera;

    private bool m_IsGrounded = true;

    // Use this for initialization
    void Start () {
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        //m_audioSource = gameObject.GetComponent<AudioSource>();
        Camera.main.GetComponent<CameraFollow>().setPosition(this.gameObject.transform);
    }

    // Update is called once per frame
    void FixedUpdate() {
        //If the player is in a menu they wont move
        if (this.gameObject.GetComponent<PlayerManager>().MenuOpen) { return; }
        if (this.gameObject.GetComponent<PlayerStats>().isDead) { return; }
        
        //Get movement
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0.0f, Input.GetAxis("Vertical") * moveSpeed);

        //Get the Xaxis rotation
        if (Input.GetAxis("Right Joystick X") != 0.0f) { rotX = Input.GetAxis("Right Joystick X") * SensitivityXAxis; }
        else { rotX = Input.GetAxis("Mouse X") * SensitivityXAxis; }
        
        //Get the Yaxis rotation
        if (Input.GetAxis("Right Joystick X") != 0.0f) { rotY += Input.GetAxis("Right Joystick Y") * SensitivityYAxis; }
        else { rotY -= Input.GetAxis("Mouse Y") * SensitivityYAxis; }

        //Clamp the camera rotation in the Yaxis
        rotY = Mathf.Clamp(rotY, -60f, 70f);  //high, low
        //Holding jump button
        if (Input.GetButton("Jump") && m_IsGrounded) {
            if (Jump.y < Jump.z) { Jump.y += moveSpeed * Time.deltaTime; }
            else { Jump.y = Jump.z; }
        }
        //Not holding jump button
        else {
            if (Jump.y > 0.0f) {
                Jump.y = Jump.y + (Jump.x*2.0f);
                moveDirection.y = Jump.y;
                Jump.y -= Jump.x * 2.15f;
                //Jump.y = 0.0f;
            }
            else if (Jump.y < 0.0f) {
                Jump.y = 0.0f;
            }
        }

        //If the player is grounded do not apply additional gravity
        if (m_IsGrounded) { moveDirection.y -= 0.0f * Time.deltaTime; }
        else { moveDirection.y -= 30.0f * Time.deltaTime; }
        //Rotate player
        transform.Rotate(0, rotX, 0);
        //Move player
        moveDirection = transform.rotation * moveDirection;
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveDirection * Time.deltaTime);
        //Rotate camera
        RotationNeededForCamera += rotX;
        Camera.main.GetComponent<CameraFollow>().setRotation(new Vector2(RotationNeededForCamera, rotY));
    }

    void OnCollisionEnter(Collision collision) {
        //Needs to fix as it allows wall jumping
        if (collision.contacts.Length > 0) {
            if (Vector3.Dot(transform.up, collision.contacts[0].normal) > 0.5f) {
                m_IsGrounded = true;
            }
        }
    }
}
