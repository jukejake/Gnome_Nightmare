using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_001 : MonoBehaviour {

    public float m_Speed;
    public float m_JumpSpeed;
    public float m_RotationSpeed;
    public float m_MaxSpeed;
    private bool m_IsGrounded;
    private float m_Turn;


    private Rigidbody m_Rigidbody;
    private AudioSource m_audioSource;
    // Use this for initialization
    void Start () {
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        m_IsGrounded = true;
        m_audioSource = gameObject.GetComponent<AudioSource>();

        m_Speed = 3;
        m_JumpSpeed = 3;
        m_RotationSpeed = 3;
        m_MaxSpeed = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && m_IsGrounded)
        {
            m_Rigidbody.velocity += transform.up * m_JumpSpeed;
            m_IsGrounded = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            m_Rigidbody.velocity += transform.forward * m_Speed * Time.deltaTime;
            float l_y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_MaxSpeed);
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, l_y, m_Rigidbody.velocity.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_Rigidbody.velocity += -transform.forward * m_Speed * Time.deltaTime;
            float l_y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_MaxSpeed);
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, l_y, m_Rigidbody.velocity.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_Rigidbody.AddTorque(transform.up * m_RotationSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_Rigidbody.AddTorque(transform.up * -m_RotationSpeed);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            //m_Rigidbody.AddTorque(transform.up * 0.0f);
            //Debug.Log("Not turning");
        }
    }

    void FisedUpdate() {
        
    }
        

    void OnCollisionEnter(Collision collision) {
        if (collision.contacts.Length > 0) {
            if (Vector3.Dot(transform.up, collision.contacts[0].normal) > 0.5f) {
                m_IsGrounded = true;
            }
        }
    }
}
