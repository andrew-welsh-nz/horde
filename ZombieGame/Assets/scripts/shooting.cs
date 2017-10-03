using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shooting : MonoBehaviour {

    float timeCharging = 0.0f;
    [SerializeField]
    float fullChargeTime;
    [SerializeField]
    float charge = 0.0f;
    bool isCharging = false;

    [SerializeField]
    Slider PowerSlider;

    [SerializeField]
    float firstEffect;
    [SerializeField]
    float secondEffect;
    bool HasFirstEffectPlayed = false;
    bool HasSecondEffectPlayed = false;

    [SerializeField]
    ParticleSystem HeldChargeEffect; 
    [SerializeField]
    ParticleSystem HeldChargeEffectHalf;

    [SerializeField]
    Transform arrowSpawn;

    [SerializeField]
    Animator anim;

    [SerializeField]
    Arrow arrowPrefab;

    [SerializeField]
    Text ArrowCountText;

    [SerializeField]
    AudioClip chargeSound;
    [SerializeField]
    AudioClip holdSound;
    [SerializeField]
    AudioClip releaseSound;

    [SerializeField]
    Light PointLight;

    [SerializeField]
    GameObject ScreenFade;

    [SerializeField]
    float touchAreaBuffer;

    public Vector3 direction;
    public Vector3 hitPoint;

    //Assign BowRadial UI and Material
    public GameObject BowRadialDisplay;
    public Material BowDisplayMaterial;

    public int ArrowCount = 10;

    float ScreenDarkenTarget = 0;

    AudioSource arrowSounds;

	// Use this for initialization
	void Start () {
        arrowSounds = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        
       // Debug.Log(direction.magnitude);

        if (ArrowCount < 10)
        {
            PointLight.range = 20 + 4 * ArrowCount;
        }

        //Sets the fill amount for the BowRadial's Image component
        BowRadialDisplay.GetComponent<Image>().fillAmount = charge / 100;

        //Makes the BowDisplayColour variable, converts the charge to a byte and sets it
        Color32 BowDisplayColour = BowDisplayMaterial.GetColor("_Color");
        BowDisplayColour.a = (byte)charge;
        BowDisplayMaterial.SetColor("_Color", BowDisplayColour);

        ArrowCountText.text = ArrowCount.ToString();

        if (HasFirstEffectPlayed && !HasSecondEffectPlayed)
        {
            HeldChargeEffectHalf.Stop();
        }

		if(Input.GetMouseButtonDown(0) && ArrowCount > 0)
        {
            if(hitPoint.x >= this.transform.position.x - touchAreaBuffer && hitPoint.x <= this.transform.position.x + touchAreaBuffer && hitPoint.z >= this.transform.position.z - touchAreaBuffer && hitPoint.z <= this.transform.position.z + touchAreaBuffer)
            {
             //   Debug.Log("MouseDown");
                isCharging = true;
                //HasSecondEffectPlayed = false;
                anim.SetTrigger("Shoot");
                arrowSounds.clip = chargeSound;
                arrowSounds.loop = false;
                arrowSounds.Play();
            }
        }

        if(Input.GetMouseButtonUp(0) && ArrowCount > 0)   
        {
            if(isCharging)
            {
              //  Debug.Log("MouseUp");
                // Reset charging states
                isCharging = false;
                timeCharging = 0.0f;

                //Turn off Emmiter
                HeldChargeEffect.Stop();
                HasSecondEffectPlayed = false;
                HasFirstEffectPlayed = false;

                // Release arrow
                anim.ResetTrigger("Shoot");
                anim.SetTrigger("Release");
                ArrowCount--;

                // Stop audio
                arrowSounds.Stop();
                arrowSounds.clip = releaseSound;
                arrowSounds.loop = false;
                arrowSounds.Play();

                // Spawn an arrow at the arrow position
                Arrow newArrow = Instantiate(arrowPrefab);
             //   Debug.Log(direction.magnitude);
                newArrow.charge = direction.magnitude;
                newArrow.transform.position = arrowSpawn.transform.position;
                newArrow.transform.rotation = arrowSpawn.transform.rotation;

                // Reset charge
                //charge = 0.0f;
                //PowerSlider.value = charge;
            }
        }

        //if(isCharging)
        //{
        //    //Debug.Log("Charging");

        //    if(timeCharging <= fullChargeTime)
        //    {
        //        timeCharging += Time.deltaTime;
        //    }

        //    charge = (timeCharging / fullChargeTime) * 100;

        //    if(charge >= 48 && charge <= 52)
        //    {
        //        Debug.Log("Half");
        //        HeldChargeEffectHalf.Play();
        //        HasFirstEffectPlayed = true;
        //    }

        //    if(charge > 100)
        //    {
        //        charge = 100;
        //        if (arrowSounds.clip != holdSound)
        //        {
        //            arrowSounds.clip = holdSound;
        //            arrowSounds.loop = true;
        //            arrowSounds.Play();
        //        }
        //    }

        //    if (!HasSecondEffectPlayed && charge >= secondEffect) {
        //        HasSecondEffectPlayed = true;
        //        HeldChargeEffect.Play();
        //    }

        //    //PowerSlider.value = charge;
        //}


        //Screen Darken Image Fade
        var ScreenDarkenCurrent = ScreenFade.GetComponent<Image>().color;

        //Raise image alpha if less than 5 arrows
        if (ArrowCount <= 5)
        {
            //Set target
            ScreenDarkenTarget = ((5 - ArrowCount) * 0.2f);

            //Fade alpha to target 
            if (ScreenDarkenCurrent.a < ScreenDarkenTarget)
            {
                ScreenDarkenCurrent.a += 0.01f;
            }
            else if (ScreenDarkenCurrent.a > ScreenDarkenTarget)
            {
                ScreenDarkenCurrent.a -= 0.005f;
            }
        }
        //Fade to 0 alpha if more than 5 arrows
        else if (ScreenDarkenCurrent.a > ScreenDarkenTarget)
        {
            ScreenDarkenTarget = 0;

            ScreenDarkenCurrent.a -= 0.005f;
        }
        
        //Apply image alpha
        ScreenFade.GetComponent<Image>().color = ScreenDarkenCurrent;
    }
}
