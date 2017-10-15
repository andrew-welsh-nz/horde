using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {

    public Animator MenuAnimator;
    public GameObject Settings;
    public GameObject Info;
    public GameObject Main_Menu;

    public Material MAT_Title;
    public Material MAT_UI;

    float titleAlpha = 0;
    float UIAlpha = 0;

    float titleDelay = 4.1f;
    float titleDelay_timer;
    float titleFadeOut = 0.5f;

    bool fadeUI = false;
    bool titleAppeared = false;
    bool intro = true;
    bool startGame = false;

    int CurrentMenu = 0;
    public float music = 0;
    public float sfx = 0;

    // Use this for initialization
    void Start () {
        //MenuAnimator.SetTrigger("OpenMenu");
        CurrentMenu = 0;
        intro = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Play Intro
        if (intro == true)
        {
            if (CurrentMenu == 0 && titleAppeared == false)
            {
                //Do the start fade
                if (titleDelay_timer <= titleDelay)
                {
                    titleDelay_timer += Time.deltaTime;
                }
                else
                {
                    //Once reached time, make title glow then start the fade
                    titleAlpha = 255;
                    titleAppeared = true;

                    MenuAnimator.SetTrigger("StartIntro");
                }
            }

            if (CurrentMenu == 0 && titleAppeared == true)
            {
                //Fade the title back to normal and fade the UI in
                fadeUI = true;

                if (titleAlpha >= 150)
                {
                    titleAlpha -= (1 * titleFadeOut);
                }
                else
                {
                    titleAlpha = 150;
                    intro = false;
                }
            }

            //Fade ui in
            if (fadeUI == true)
            {
                if (UIAlpha <= 255)
                {
                    UIAlpha += 1;
                }
                else
                {
                    UIAlpha = 255;
                    fadeUI = false;
                }
            }

            //Set materials
            Color32 TitleColour = MAT_Title.color;
            TitleColour.a = (byte)titleAlpha;
            MAT_Title.SetColor("_Color", TitleColour);

            Color32 UIColour = MAT_UI.color;
            UIColour.a = (byte)UIAlpha;
            MAT_UI.SetColor("_Color", UIColour);
        }
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    public void UI_Play()
    {
        gameObject.SetActive(false);
        startGame = true;
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

    public void musicSlider(float musicSlider)
    {
        music = musicSlider;
    }

    public void sfxSlider(float sfxSlider)
    {
        sfx = sfxSlider;
    }
}
