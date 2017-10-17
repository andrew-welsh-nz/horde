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
    float movespeed = 0.5f;

    [SerializeField]
    int currentTheme = 0;

    [SerializeField]
    LocalNavMeshBuilder NavMeshBuilder;

    private bool LayoutChangeComplete = false;

	// Use this for initialization
	void Start () {
        themes[currentTheme].gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {

        themes[currentTheme].gameObject.SetActive(true);

        for (int i = 0; i < themes.Length; i++)
        {
            if (i == currentTheme)
            {
                if(themes[i].transform.position.y <= currentTarget.position.y)
                {
                    themes[i].transform.position = new Vector3(0.0f, themes[i].transform.position.y + movespeed * Time.deltaTime, 0.0f);
                }
                else if(themes[i].transform.position.y != currentTarget.position.y)
                {
                    themes[i].transform.position = currentTarget.position;

                    if (LayoutChangeComplete == false) {
                        LayoutChangeComplete = true;
                        NavMeshBuilder.UpdateNewNavMesh = true;
                        Debug.Log("Layout change complete");
                    }

                }
            }
            else
            {
                if (themes[i].transform.position.y >= unusedTarget.position.y)
                {
                    themes[i].transform.position = new Vector3(0.0f, themes[i].transform.position.y - movespeed * Time.deltaTime, 0.0f);
                }
                else
                {
                    themes[i].gameObject.SetActive(false);
                }
            }
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
        LayoutChangeComplete = false;
    }
}
