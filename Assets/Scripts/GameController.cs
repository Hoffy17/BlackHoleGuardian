using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int health = 10;
    public int score = 0;
    public int highScore;

    //A boolean for checking whether the game is over
    [HideInInspector]
    public bool isDead;

    private Scene arcade;

    void Start()
    {
        arcade = SceneManager.GetActiveScene();

        isDead = false;
    }

    void Update()
    {
        if (isDead)
            SceneManager.LoadScene(arcade.name);
    }
}
