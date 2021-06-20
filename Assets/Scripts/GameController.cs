using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //The player's health before the game ends
    public int health = 10;
    //The player's score when enemies are destroyed
    public int score = 0;
    //The player's high score
    public int highScore;

    [HideInInspector]
    //Checks whether the player can move
    public bool isDead;
    [HideInInspector]
    public bool overheated;
    [HideInInspector]
    public bool blackHoleCollapsed;
    [HideInInspector]
    //Checks whether the game is over
    public bool gameIsOver;
    [HideInInspector]
    //Checks whether the player has acquired the wide weapon upgrade
    public bool wideActivated;
    [HideInInspector]
    //Checks whether the player has acquired the rapid weapon upgrade
    public bool rapidActivated;
    [HideInInspector]
    //Checks whether the player has acquired the large weapon upgrade
    public bool largeActivated;

    void Start()
    {
        //At the start of the game, the player is alive and all weapon upgrades are not active
        isDead = false;
        gameIsOver = false;
        overheated = false;
        blackHoleCollapsed = false;
        wideActivated = false;
        rapidActivated = false;
        largeActivated = false;

        //If a High Score does not exist, set it to 0, if it does, get it
        if (Save.Contains("HighScore") == false)
        {
            Save.SetInt("HighScore", highScore);
        }
        else
        {
            highScore = Save.GetInt("HighScore");
        }
    }

    void Update()
    {
        //If the current score exceeds the high score, update the high score
        if (score > highScore)
        {
            highScore = score;
        }

        //If the player dies and their high score exceeds the saved high score, overwrite it
        if (gameIsOver && (highScore > Save.GetInt("HighScore")))
        {
            Save.SetInt("HighScore", highScore);
        }
    }
}
