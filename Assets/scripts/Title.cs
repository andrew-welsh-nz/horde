using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour {

    //SpriteRenderer sprite;

    //Assign BowRadial UI and Material
    public Material TitleMaterial;

    [SerializeField]
    float alpha = 255;

    [SerializeField]
    GameObject Player;

    float startOffset;
    float startTime = 4.1f;

    bool appear = false;

	// Use this for initialization
	void Start () {
        //sprite = GetComponent<SpriteRenderer>();
        Color32 TitleColour = TitleMaterial.GetColor("_Color");
        TitleColour.a = (byte)0;
        TitleMaterial.SetColor("_Color", TitleColour);
    }
	
	// Update is called once per frame
	void Update () {

        //Kills itself if game has started
        if (Player.GetComponent<PlayerController>().HasStarted == true)
        {
            Destroy(this.gameObject);
        }

        //Appear only once, setting alpha to 255, then fade to 0 and destroy object.
        if (startOffset >= startTime)
        {
            if (appear == false)
            {
                alpha = 255f;

                Color32 TitleColour = TitleMaterial.GetColor("_Color");
                TitleColour.a = (byte)alpha;
                TitleMaterial.SetColor("_Color", TitleColour);

                appear = true;
            }
            else if (alpha > 0)
            {
                alpha -= 75f * Time.deltaTime;

                Color32 TitleColour = TitleMaterial.GetColor("_Color");
                TitleColour.a = (byte)alpha;
                TitleMaterial.SetColor("_Color", TitleColour);

                if (alpha <= 10)
                {
                    alpha = 0;

                    //Need to destroy because it comes back sometimes, not sure why
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            startOffset += Time.deltaTime;
        }


    }
}
