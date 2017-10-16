using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

    [SerializeField]
    GameObject MainSpawnControl;

    [SerializeField]
    GameObject Player;

    [SerializeField]
    GameObject ArrowBreakUI;

    [SerializeField]
    GameObject ArrowCounterUI;

    [SerializeField]
    GameObject ArrowCounter;

    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Arrow") {
            //Start Game
            MainSpawnControl.SetActive(true);

            Player.GetComponent<shooting>().ArrowCount = 10;
            Player.GetComponent<PlayerController>().HasStarted = true;

            //Makes game UI appear
            ArrowBreakUI.SetActive(true);
            ArrowCounterUI.SetActive(true);
            ArrowCounter.SetActive(true);

            Destroy(other.gameObject);
            Destroy(this.gameObject);
           
        }
    }
}
