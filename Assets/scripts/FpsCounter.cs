using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour {

    int FPSCount = 0;

    float CooldownCounter = 0.0f;

	// Use this for initialization
	void Start () {
        FPSCount = 0;

    }
	
	// Update is called once per frame
	void Update () {
        CooldownCounter += Time.deltaTime;

        //Every fifth of a second update the fps
        if (CooldownCounter >= 0.2f)
        {
            FPSCount = (int) (1.0f / Time.deltaTime);
            GetComponent<Text>().text = "Fps: " + FPSCount.ToString();
            CooldownCounter = 0.0f;
        }
	}
}
