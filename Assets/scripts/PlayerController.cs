using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerController : MonoBehaviour {

    // Use this for initialization

    int floorMask;
    float camRayLength = 100.0f;
    public float CurrentScore = 0.0f;
    public int DisplayScore = 0;
    public bool HasStarted = false;

    public Vector3 direction;

    private bool GameOverHasBeenCalled = false;

    [SerializeField]
    GameObject GameOverText;

    [SerializeField]
    GameObject ScoreText;

    [SerializeField]
    GameObject HighScoreText;

    shooting Shooting;

    void Awake () {
        floorMask = LayerMask.GetMask("Floor");
        CurrentScore = 0.0f;
        CurrentScore = 0.0f;
        GameOverHasBeenCalled = false;

        Shooting = GetComponent<shooting>();
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
            Vector3 direction = -(floorhit.point - transform.position);
            direction.y = 0.0f;

            Shooting.direction = direction;
            Shooting.hitPoint = floorhit.point;
            //Debug.Log(direction.magnitude);
        }
    }

    void CheckNewHighScore(int Score) {
        //Get High Scores
        StreamReader reader = new StreamReader("Assets/Text/HighScores.txt");
        string HighScore = reader.ReadLine();
        reader.Close();
        int CurrentHightScore =  int.Parse(HighScore);

        //Check if it's a high score
        if (CurrentHightScore < Score) {
            CurrentHightScore = Score;

            StreamWriter writer = new StreamWriter("Assets/Text/HighScores.txt", false);
            writer.WriteLine(CurrentHightScore.ToString());
            writer.Close();
        }
    }

    void DisplayHighScores() {
        //Get High Scores
        StreamReader reader = new StreamReader("Assets/Text/HighScores.txt");
        string HighScore = reader.ReadLine();
        reader.Close();
        int CurrentHightScore = int.Parse(HighScore);
        HighScoreText.GetComponent<Text>().text = "HighScore: " + CurrentHightScore.ToString();
    }

    public IEnumerator GameOver() {
        if (!GameOverHasBeenCalled)
        {
            GameOverHasBeenCalled = true;
            ScoreText.GetComponent<Text>().text = "Score: " + DisplayScore.ToString();
            CheckNewHighScore(DisplayScore);
            DisplayHighScores();
            GameOverText.SetActive(true);
            ScoreText.SetActive(true);
            HighScoreText.SetActive(true);
            yield return new WaitForSeconds(5.0f);
            SceneManager.LoadScene("main");
        }
    }
}
