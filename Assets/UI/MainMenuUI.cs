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

    [SerializeField]
    FirstDrop startDrop;

    // Use this for initialization
    void Start () {
        CurrentMenu = 0;
        intro = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
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
                    //Trigger animator to move UI around, make title glow
                    titleAlpha = 255;
                    titleAppeared = true;

                    MenuAnimator.SetTrigger("StartIntro");
                }
            }

            if (CurrentMenu == 0 && titleAppeared == true)
            {
                //Trigger UI to fade in
                fadeUI = true;

                //Fade title back to normal
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

            //Fade UI
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
        startDrop.gameObject.SetActive(true);
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
