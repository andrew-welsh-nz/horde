using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme : MonoBehaviour {

    [SerializeField]
    GameObject[] layouts;

    int currentActive;
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetNewActive()
    {
        // Randomize a new current scene and set the others to inactive
        currentActive = Random.Range(0, layouts.Length);

        for(int i = 0; i < layouts.Length; i++)
        {
            if(i == currentActive)
            {
                layouts[i].SetActive(true);
            }
            else
            {
                layouts[i].SetActive(false);
            }
        }
    }
}
