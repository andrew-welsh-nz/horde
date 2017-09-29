using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    // Use this for initialization

    int floorMask;
    float camRayLength = 100.0f;
    public float CurrentScore = 0.0f;
    public int DisplayScore = 0;
    public bool HasStarted = false;

    private bool GameOverHasBeenCalled = false;

    [SerializeField]
    GameObject GameOverText;

    [SerializeField]
    GameObject ScoreText;

    void Awake () {
        floorMask = LayerMask.GetMask("Floor");
        CurrentScore = 0.0f;
        CurrentScore = 0.0f;
        GameOverHasBeenCalled = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Turning();
	}

    private void Update(){
        if (HasStarted)
        {
            CurrentScore += Time.deltaTime;
            DisplayScore = (int)CurrentScore;
        }
        else {
            this.gameObject.GetComponent<shooting>().ArrowCount = 10;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void Turning() {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorhit;

        if (Physics.Raycast(camRay, out floorhit, camRayLength, floorMask)) {
            Vector3 playerToMouse = floorhit.point - transform.position;
            playerToMouse.y = 0.0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            GetComponent<Rigidbody>().MoveRotation(newRotation);
        }
    }


    public IEnumerator GameOver() {
        if (!GameOverHasBeenCalled)
        {
            GameOverHasBeenCalled = true;
            ScoreText.GetComponent<Text>().text = "Score: " + DisplayScore.ToString();
            GameOverText.SetActive(true);
            ScoreText.SetActive(true);
            yield return new WaitForSeconds(5.0f);
            SceneManager.LoadScene("main");
        }
    }
}
