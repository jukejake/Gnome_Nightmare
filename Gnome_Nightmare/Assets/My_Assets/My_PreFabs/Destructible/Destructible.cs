using UnityEngine;

//Based off of a youtube video
//https://www.youtube.com/watch?v=EgNV0PWVaS8&list=PLdnBuK2UbqBQnM2LZcraHyLGu3heJVO7F&index=5
//

public class Destructible : MonoBehaviour {

    public GameObject destroyedVersion;
    public float MaxVelocity = 10.0f;
    private bool Destroyed = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Kill() {
        GameObject v_destroyedVersion = Instantiate(destroyedVersion, this.transform.position, this.transform.rotation);
        Destroy(v_destroyedVersion, 6.0f);
        Destroy(this.gameObject);
        Destroyed = true;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.relativeVelocity.magnitude > MaxVelocity && Destroyed == false) {
            Instantiate(destroyedVersion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
            Destroyed = true;
        }
    }

    private void OnMouseDown() {
        Instantiate(destroyedVersion, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
        Destroyed = true;
    }
}
