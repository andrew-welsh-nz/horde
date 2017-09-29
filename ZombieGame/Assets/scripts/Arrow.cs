using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public float charge;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Move in the forward direction at speed
        transform.position += transform.forward * Time.deltaTime * charge;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Environment")
        {
            this.gameObject.GetComponent<Arrow>().charge = 0;

            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.GetComponent<Rigidbody>().detectCollisions = false;


            this.gameObject.transform.SetParent(other.gameObject.transform);
            // collision.transform.position += new Vector3(0.0f, 1.8f, 0.0f);
            this.transform.position += this.gameObject.transform.forward * 1.0f;
        }
    }
}
