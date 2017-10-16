using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shooting : MonoBehaviour {

    [SerializeField]
    float chargeSpeed = 0.5f;

    float timeCharging = 0.0f;
    [SerializeField]
    float fullChargeTime;
    [SerializeField]
    float charge = 0.0f;
    bool isCharging = false;

    [SerializeField]
    float aimRadius;

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

    [SerializeField]
    AudioSource dangerMusic;

    public Vector3 direction;
    public Vector3 hitPoint;

    //Assign BowRadial UI and Material
    public GameObject BowRadialDisplay;
    public Material BowDisplayMaterial;

    public GameObject ArrowCounter;
    float ArrowAmount;

    //Arrow Line Stuff
    public GameObject ArrowLineL;
    public GameObject ArrowLineR;
    float ArrowLineDistTarg = 0.75f;
    float ArrowLineDist = 1.5f;
    float ArrowLineWidth = 1;
    float ArrowLineAlpha = 0;
    float ArrowLineLength = 100f;

    public Material ArrowLineMat;

    Vector3 ArrowLineStart;
    Vector3 ArrowLineEnd;

    public GameObject ArrowLineGroup;
    public Material PullIndicatorMaterial;
    public GameObject PullIndicatorHolder;
    public GameObject PullIndicator;
    public GameObject PullIndicatorBacking;
    float PullIndicatorAlpha = 0.0f;

    public int ArrowCount = 10;

    float ScreenDarkenTarget = 0;

    AudioSource arrowSounds;

    bool hasBegun = false;

	// Use this for initialization
	void Start () {
        arrowSounds = GetComponent<AudioSource>();
        ArrowLineGroup.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (hasBegun)
        {
            // Debug.Log(direction.magnitude);

            if (ArrowCount < 10)
            {
                PointLight.range = 20 + 4 * ArrowCount;
            }

            if (ArrowCount < 5 && dangerMusic.volume < 0.2f)
            {
                dangerMusic.volume += 0.005f;
            }
            else if (ArrowCount < 5 && dangerMusic.volume > 0.2f)
            {
                dangerMusic.volume = 0.2f;
            }

            if (ArrowCount > 4 && dangerMusic.volume > 0.0f)
            {
                dangerMusic.volume -= 0.02f;
            }
            else if (ArrowCount > 4 && dangerMusic.volume < 0.0f)
            {
                dangerMusic.volume = 0.0f;
            }

            ArrowCountText.text = ArrowCount.ToString();

            ArrowAmount = ArrowCount;

            ArrowCounter.GetComponent<Image>().fillAmount = (ArrowAmount / 10);

            if (HasFirstEffectPlayed && !HasSecondEffectPlayed)
            {
                HeldChargeEffectHalf.Stop();
            }

            if (Input.GetMouseButtonDown(0) && ArrowCount > 0)
            {
                if (hitPoint.x >= this.transform.position.x - touchAreaBuffer && hitPoint.x <= this.transform.position.x + touchAreaBuffer && hitPoint.z >= this.transform.position.z - touchAreaBuffer && hitPoint.z <= this.transform.position.z + touchAreaBuffer)
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

            if (Input.GetMouseButtonUp(0) && ArrowCount > 0)
            {
                if (isCharging)
                {
                    //  Debug.Log("MouseUp");
                    // Reset charging states
                    isCharging = false;
                    timeCharging = 0.0f;

                    charge *= 100;

                    //Turn off Emmiter
                    HeldChargeEffect.Stop();
                    HasSecondEffectPlayed = false;
                    HasFirstEffectPlayed = false;

                    // Release arrow
                    anim.ResetTrigger("Shoot");
                    anim.SetTrigger("Release");
                    ArrowCount--;

                    //Arrow Counter Flash
                    ArrowCountFlash();

                    // Stop audio
                    arrowSounds.Stop();
                    arrowSounds.clip = releaseSound;
                    arrowSounds.loop = false;
                    arrowSounds.Play();

                    // Spawn an arrow at the arrow position
                    Arrow newArrow = Instantiate(arrowPrefab);
                    //   Debug.Log(direction.magnitude);
                    newArrow.charge = charge;
                    newArrow.transform.position = arrowSpawn.transform.position;
                    newArrow.transform.rotation = arrowSpawn.transform.rotation;

                    // Reset charge
                    //charge = 0.0f;
                    //PowerSlider.value = charge;
                }
            }

            if (isCharging)
            {
                ArrowLineGroup.SetActive(true);

                if (charge < direction.magnitude / aimRadius)
                {
                    charge += chargeSpeed;
                }
                else
                {
                    charge = direction.magnitude / aimRadius;
                }
                //charge = direction.magnitude / aimRadius;

                if (charge > 1.0f)
                {
                    charge = 1.0f;
                }

                anim.Play("Shooting", 0, charge);

                //Arrow Display

                ArrowLineL.SetActive(true);
                ArrowLineR.SetActive(true);

                if (charge <= 0.95)
                {
                    ArrowLineAlpha = charge * 0.5f;

                    if (charge <= 0.5f)
                    {
                        //If charge is less than 0.5, use charge to determine the distance from the centre the two lines are, and control the width

                        ArrowLineDist = ArrowLineDistTarg - ((charge * 2) * ArrowLineDistTarg);
                        ArrowLineWidth = charge * 1f;
                    }
                    else
                    {
                        //If charge is over 0.5, keep lines at centre, while buff up the scale rate

                        ArrowLineDist = 0.0f;
                        ArrowLineWidth = (charge * 3f);
                    }
                }
                else
                {
                    //Make the line width huge above 0.95 charge
                    ArrowLineAlpha = 1;
                    ArrowLineWidth = 5.0f;
                }

                //Raycast to get distance for the line

                RaycastHit ArrowLineInfo;

                if (Physics.Raycast(new Vector3(transform.position.x, 1.75f, transform.position.z), transform.forward, out ArrowLineInfo))
                {
                    ArrowLineLength = ArrowLineInfo.distance;
                }
                else
                {
                    ArrowLineLength = 100;
                }

                //Setting the line's dynamic actions

                //Position
                ArrowLineL.GetComponent<RectTransform>().localPosition = new Vector3(ArrowLineDist, 0, 0);
                ArrowLineR.GetComponent<RectTransform>().localPosition = new Vector3(-ArrowLineDist, 0, 0);

                //Scale
                ArrowLineL.GetComponent<RectTransform>().localScale = new Vector3(ArrowLineWidth, 1, 1);
                ArrowLineR.GetComponent<RectTransform>().localScale = new Vector3(ArrowLineWidth, 1, 1);

                //Length
                ArrowLineL.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ArrowLineLength);
                ArrowLineR.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ArrowLineLength);

                //Colour
                ArrowLineMat.color = new Color(0, 0, 0, ArrowLineAlpha);

                Quaternion newRotation = Quaternion.LookRotation(direction);
                GetComponent<Rigidbody>().MoveRotation(newRotation);

                //Pull Indicator

                PullIndicatorAlpha = (charge * 255);
                PullIndicatorBacking.SetActive(true);
                PullIndicatorBacking.GetComponent<Image>().fillAmount = direction.magnitude / aimRadius;
                PullIndicator.GetComponent<Image>().fillAmount = charge;

                //Apply Colour

                Color32 PullIndicatorColour = PullIndicatorMaterial.GetColor("_Color");
                PullIndicatorColour.a = (byte)(PullIndicatorAlpha);
                PullIndicatorMaterial.SetColor("_Color", PullIndicatorColour);

                //Debug.Log("Charging");

                //if (timeCharging <= fullChargeTime)
                //{
                //    timeCharging += Time.deltaTime;
                //}

                //charge = (timeCharging / fullChargeTime) * 100;

                //if (charge >= 48 && charge <= 52)
                //{
                //    Debug.Log("Half");
                //    HeldChargeEffectHalf.Play();
                //    HasFirstEffectPlayed = true;
                //}

                //if (charge > 100)
                //{
                //    charge = 100;
                //    if (arrowSounds.clip != holdSound)
                //    {
                //        arrowSounds.clip = holdSound;
                //        arrowSounds.loop = true;
                //        arrowSounds.Play();
                //    }
                //}

                //if (!HasSecondEffectPlayed && charge >= secondEffect)
                //{
                //    HasSecondEffectPlayed = true;
                //    HeldChargeEffect.Play();
                //}

                //PowerSlider.value = charge;
            }
            else
            {
                //Disables the shooting displays

                ArrowLineL.SetActive(false);
                ArrowLineR.SetActive(false);
                PullIndicatorBacking.SetActive(false);

                Color32 BowDisplayColour = BowDisplayMaterial.GetColor("_Color");
                BowDisplayColour.a = (byte)(0);
                BowDisplayMaterial.SetColor("_Color", BowDisplayColour);

                Color32 PullIndicatorColour = PullIndicatorMaterial.GetColor("_Color");
                PullIndicatorColour.a = (byte)(0);
                PullIndicatorMaterial.SetColor("_Color", PullIndicatorColour);
            }

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

    public void ArrowCountFlash()
    {
        ArrowCounter.GetComponent<ArrowCounter>().ArrowCounterFlash();
    }

    public void StartGame()
    {
        hasBegun = true;
    }
}
