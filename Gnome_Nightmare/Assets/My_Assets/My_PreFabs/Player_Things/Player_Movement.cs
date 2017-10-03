using UnityEngine.EventSystems;
using UnityEngine;

public class Player_Movement : MonoBehaviour {

	public float moveSpeed;
    public float SensitivityXAxis;
    public float SensitivityYAxis;
    public Vector3 Jump = new Vector3(3.0f, 0.0f, 10.0f); //MinJump,JumpPressure, MaxJumpPressure

    public GameObject Eyes;
    private Rigidbody m_Rigidbody;
    private AudioSource m_audioSource;

	private Vector3 moveDirection = Vector3.zero;
	private float rotX;
    private float rotY;
    private float RotationNeeded;

    private bool m_IsGrounded;

    // Use this for initialization
    void Start () {
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        m_IsGrounded = true;
        m_audioSource = gameObject.GetComponent<AudioSource>();
        moveSpeed = 10.0f;
        SensitivityXAxis = 15.0f;
        SensitivityYAxis = 5.0f;
        Camera.main.GetComponent<CameraFollow>().setPosition(this.gameObject.transform);
    }

    // Update is called once per frame
    void Update() {

        //if (EventSystem.current.IsPointerOverGameObject()) { return; }


        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0.0f, Input.GetAxis("Vertical") * moveSpeed);
        rotX = Input.GetAxis("Mouse X") * SensitivityXAxis;
        rotY -= Input.GetAxis("Mouse Y") * SensitivityYAxis;
        rotY = Mathf.Clamp(rotY, -60f, 50f);
        Eyes.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
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
        Camera.main.GetComponent<CameraFollow>().setRotation(new Vector2(RotationNeeded, rotY));
    }

    void OnCollisionEnter(Collision collision) {

        if (collision.contacts.Length > 0) {
            if (Vector3.Dot(transform.up, collision.contacts[0].normal) > 0.5f) {
                m_IsGrounded = true;
            }
        }
    }
}
