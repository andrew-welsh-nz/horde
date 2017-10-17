using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

    [SerializeField]
    Transform unusedTarget;

    [SerializeField]
    Transform currentTarget;

    [SerializeField]
    GameObject[] sets;

    [SerializeField]
    float movespeed = 0.5f;

    [SerializeField]
    int currentSet = 0;

    [SerializeField]
    LocalNavMeshBuilder NavMeshBuilder;

    private bool LayoutChangeComplete = false;

	// Use this for initialization
	void Start () {
        sets[currentSet].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {

        sets[currentSet].SetActive(true);

        for (int i = 0; i < sets.Length; i++)
        {
            if (i == currentSet)
            {
                if(sets[i].transform.position.y <= currentTarget.position.y)
                {
                    sets[i].transform.position = new Vector3(0.0f, sets[i].transform.position.y + movespeed * Time.deltaTime, 0.0f);
                }
                else if(sets[i].transform.position.y != currentTarget.position.y)
                {
                    sets[i].transform.position = currentTarget.position;

                    if (LayoutChangeComplete == false) {
                        LayoutChangeComplete = true;
                        NavMeshBuilder.UpdateNewNavMesh = true;
                        Debug.Log("Layout change complete");
                    }

                }
            }
            else
            {
                if (sets[i].transform.position.y >= unusedTarget.position.y)
                {
                    sets[i].transform.position = new Vector3(0.0f, sets[i].transform.position.y - movespeed * Time.deltaTime, 0.0f);
                }
                else
                {
                    sets[i].SetActive(false);
                }
            }
        }
	}

    public void ChangeSet()
    {
        Debug.Log("Changing Sets");
        //currentSet = Random.Range(0, sets.Length);
        currentSet = 1;
        LayoutChangeComplete = false;
    }
}
