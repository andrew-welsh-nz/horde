using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

    public Material DropMaterial;

    public GameObject ExpArrow;
    public GameObject InstaKill;
    public GameObject Nuke;
    public GameObject MoreArrows;

    public int DropType = 0;

    public float lifetime = 15;
    float life = 100;

	// Use this for initialization
	void Start () {
        life = lifetime;

        DropType = Random.Range(0, 4);

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
        if (life > 0)
        {
            life -= (1 * Time.deltaTime);
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
}
