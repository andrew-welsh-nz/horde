using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCounter : MonoBehaviour {

    public Animator Animator;
    public GameObject Player;

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void ArrowCounterFlash()
    {
        if (Player.GetComponent<PlayerController>().HasStarted == true)
        {
            Animator.SetTrigger("flash");
        }
    }
}
