using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

    [SerializeField]
    Transform unusedTarget;

    [SerializeField]
    Transform currentTarget;

    //[SerializeField]
    //GameObject[] scenes;

    [SerializeField]
    Theme[] themes;

    [SerializeField]
    GameObject[] Lights;

    public Light[] PointLights;

    [SerializeField]
    float movespeed = 0.5f;

    [SerializeField]
    float LightTransitionSpeed = 0.5f;

    [SerializeField]
    int currentTheme = 0;

    [SerializeField]
    LocalNavMeshBuilder NavMeshBuilder;

    [SerializeField]
    CameraShake MainCamera;

    private bool LayoutChangeComplete = false;
    private bool UpdateNavMesh = false;

    // Use this for initialization
    void Start () {
        themes[currentTheme].gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {

        themes[currentTheme].gameObject.SetActive(true);
        Lights[currentTheme].gameObject.SetActive(true);

        int NavMeshCounter = 0;
        for (int i = 0; i < themes.Length; i++)
        {
            if (i == currentTheme)
            {
                //Theme Objects
                if(themes[i].transform.position.y <= currentTarget.position.y)
                {
                    themes[i].transform.position = new Vector3(0.0f, themes[i].transform.position.y + movespeed * Time.deltaTime, 0.0f);
                }
                else if(themes[i].transform.position.y != currentTarget.position.y)
                {
                    themes[i].transform.position = currentTarget.position;

                }

                //Lights
                if (Lights[i].gameObject.GetComponentInChildren<Light>().intensity < 3)
                {
                    Light[] allLights = Lights[i].gameObject.GetComponentsInChildren<Light>();
                    foreach (Light light in allLights) {
                        light.gameObject.GetComponentInChildren<Light>().intensity += LightTransitionSpeed * Time.deltaTime;
                    }
                    
                   
                }
                else {
                    Light[] allLights = Lights[i].gameObject.GetComponentsInChildren<Light>();
                    foreach (Light light in allLights)
                    {
                        light.gameObject.GetComponentInChildren<Light>().intensity = 3;
                    }
                }


            }
            else
            {
                //Game Objects
                if (themes[i].transform.position.y >= unusedTarget.position.y)
                {
                    themes[i].transform.position = new Vector3(0.0f, themes[i].transform.position.y - movespeed * Time.deltaTime, 0.0f);
                }
                else
                {
                    themes[i].gameObject.SetActive(false);
                }

                //Lights
                if (Lights[i].gameObject.GetComponentInChildren<Light>().intensity > 0)
                {
                    Light[] allLights = Lights[i].gameObject.GetComponentsInChildren<Light>();
                    foreach (Light light in allLights)
                    {
                        light.gameObject.GetComponentInChildren<Light>().intensity -= LightTransitionSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    Light[] allLights = Lights[i].gameObject.GetComponentsInChildren<Light>();
                    foreach (Light light in allLights)
                    {
                        light.gameObject.GetComponentInChildren<Light>().intensity = 0; 
                    }
                    Lights[i].SetActive(false);
                }
            }


            if (i != currentTheme) {
                if (themes[i].gameObject.activeSelf == false)
                    NavMeshCounter++;
            }


        }

        //Checks if the layout has finished changing based on the amount of inactive themes (as they tend to deactivate last)
        if (NavMeshCounter >= 2) {
            if (LayoutChangeComplete == false)
            {
                LayoutChangeComplete = true;
                UpdateNavMesh = true;
            }
        }

        if (UpdateNavMesh == true) {
            UpdateNavMesh = false;
            NavMeshBuilder.UpdateNewNavMesh = true;
            Debug.Log("Layout change complete");
        }

	}

    public void ChangeSet()
    {
        Debug.Log("Changing Sets");
        int oldTheme = currentTheme;
        while(currentTheme == oldTheme)
        {
            currentTheme = Random.Range(0, themes.Length);
        }
        themes[currentTheme].GetNewActive();
        //currentTheme = 1;
        MainCamera.enabled = true;
        LayoutChangeComplete = false;
    }
}
