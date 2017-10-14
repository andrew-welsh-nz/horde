using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {

    public Animator MenuAnimator;
    public GameObject Settings;
    public GameObject Info;
    public GameObject Main_Menu;

    int CurrentMenu = 0;

    // Use this for initialization
    void Start () {
        MenuAnimator.SetTrigger("OpenMenu");
        CurrentMenu = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Test()
    {
        Debug.Log("Test");
    }

    public void UI_Play()
    {
        Settings.SetActive(false);
    }

    public void UI_Back()
    {
        Settings.SetActive(false);
        Info.SetActive(false);
        Main_Menu.SetActive(true);
        CurrentMenu = 0;
    }

    public void UI_Settings()
    {
        Main_Menu.SetActive(false);
        Settings.SetActive(true);
        CurrentMenu = 2;
    }

    public void UI_Info()
    {
        Main_Menu.SetActive(false);
        Info.SetActive(true);
        CurrentMenu = 1;
    }
}
