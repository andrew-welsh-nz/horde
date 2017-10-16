using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstDrop : MonoBehaviour {

    [SerializeField]
    shooting game;

    [SerializeField]
    ActiveOnPlay firstEnemy;

	public void StartGame()
    {
        game.StartGame();
        firstEnemy.StartGame();
    }
}
