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

    //Checks whether the game is over
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public bool wideActivated;
    [HideInInspector]
    public bool rapidActivated;
    [HideInInspector]
    public bool largeActivated;

    void Start()
    {
        //At the start of the game, the player is alive
        isDead = false;
        wideActivated = false;
        rapidActivated = false;
        largeActivated = false;

        //If a HighScore does not exist, set to 0, if it does, get it
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
        //If the player dies, reload the scene
        if (isDead)
        {
            //If the player's high score exceeds the saved high score, overwrite it
            if (highScore > Save.GetInt("HighScore"))
            {
                Save.SetInt("HighScore", highScore);
            }
        }
    }
}
