using UnityEngine;
using UnityEngine.Networking;

public class Network_Player_Movement : NetworkBehaviour {

    public float RotationNeeded;

    public float moveSpeed;
	public float SensitivityXAxis;
    public float SensitivityYAxis;
    public Vector3 Jump = new Vector3(3.0f, 0.0f, 10.0f); //MinJump,JumpPressure, MaxJumpPressure

    private Rigidbody m_Rigidbody;
    //private AudioSource m_audioSource;

	private Vector3 moveDirection = Vector3.zero;
	private float rotX;
    private float rotY;

    private bool m_IsGrounded;

    // Use this for initialization
    public override void OnStartLocalPlayer() {
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        m_IsGrounded = true;
        //m_audioSource = gameObject.GetComponent<AudioSource>();
        moveSpeed = 10.0f;
        SensitivityXAxis = 20.0f;
        SensitivityYAxis = 5.0f;

        this.GetComponent<MeshRenderer>().material.color = Color.blue;
        Camera.main.GetComponent<CameraFollow>().SetPosition(this.gameObject.transform);
        
    }

    // Update is called once per frame
    void Update() {

        if (!isLocalPlayer) { return; }

        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0.0f, Input.GetAxis("Vertical") * moveSpeed);
        rotX = Input.GetAxis("Mouse X") * SensitivityXAxis;
        rotY -= Input.GetAxis("Mouse Y") * SensitivityYAxis;
        rotY = Mathf.Clamp(rotY, -60f, 50f);
        //Camera.main.GetComponent<CameraFollow>().setRotation(new Vector2(transform.rotation.y, rotY));
        //holding jump button
        if (Input.GetButton("Jump") && m_IsGrounded) {
            if (Jump.y < Jump.z) { Jump.y += moveSpeed * Time.deltaTime; }
            else { Jump.y = Jump.z; }
        }
        //not holding jump button
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

        moveDirection.y -= 30.0f * Time.deltaTime;
        transform.Rotate(0, rotX, 0);
        moveDirection = transform.rotation * moveDirection;
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveDirection * Time.deltaTime);
        RotationNeeded += rotX;
        Camera.main.GetComponent<CameraFollow>().SetRotation(new Vector2(RotationNeeded, rotY));

    }
	
	void FixedUpdate () {
		
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.contacts.Length > 0) {
            if (Vector3.Dot(transform.up, collision.contacts[0].normal) > 0.5f) {
                m_IsGrounded = true;
            }
        }
    }
}
