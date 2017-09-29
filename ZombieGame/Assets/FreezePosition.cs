using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePosition : MonoBehaviour {

    private Vector3 OriginalPosition;
    private Quaternion OriginalRotation;
	// Use this for initialization
	void Awake () {
        OriginalPosition = this.transform.position;
        OriginalRotation = this.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = OriginalPosition;
        this.transform.rotation = OriginalRotation;
    }
}
