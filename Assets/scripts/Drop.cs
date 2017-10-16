using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

    [SerializeField]
    shooting shooter;

    public Material DropMaterial;

    public GameObject ExpArrow;
    public GameObject InstaKill;
    public GameObject Nuke;
    public GameObject MoreArrows;

    public int DropType = 0;
    public float DropAlpha = 255;

    public bool isStart = false;

    public float lifetime = 15;
    float life = 100;

    int dropMask;
    float camRayLength = 100.0f;

    // Use this for initialization
    void Start () {
        dropMask = LayerMask.GetMask("Drop");

        life = lifetime;

        //DropType = Random.Range(0, 4);
        DropType = 3;

        switch (DropType)
        {
            case 0:
                ExpArrow.SetActive(true);

                InstaKill.SetActive(false);
                Nuke.SetActive(false);
                MoreArrows.SetActive(false);
                break;
            case 1:
                InstaKill.SetActive(true);

                ExpArrow.SetActive(false);
                Nuke.SetActive(false);
                MoreArrows.SetActive(false);
                break;
            case 2:
                Nuke.SetActive(true);

                ExpArrow.SetActive(false);
                InstaKill.SetActive(false);
                MoreArrows.SetActive(false);
                break;

            case 3:
                MoreArrows.SetActive(true);

                Nuke.SetActive(false);
                ExpArrow.SetActive(false);
                InstaKill.SetActive(false);
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(!isStart)
        {
            if (life > 0)
            {
                life -= (1 * Time.deltaTime);
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        Color32 DropColour = DropMaterial.GetColor("_Color");
        DropColour.a = (byte)(DropAlpha);
        DropMaterial.SetColor("_Color", DropColour);

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorhit;

        if (Physics.Raycast(camRay, out floorhit, camRayLength, dropMask))
        {
            //Debug.Log("Drop tapped!");
            //Debug.Log(direction.magnitude);

            if(isStart)
            {
                shooter.ArrowCount = 10;
                GameObject.Destroy(gameObject);
            }
        }
    }
}
