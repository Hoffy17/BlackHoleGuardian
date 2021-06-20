using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //The health bar slider
    public Slider healthBar;
    //The overheat bar slider;
    public Slider overheatBar;
    //The score text box
    public Text scoreText;
    //The high score text box
    public Text highScoreText;
    //This text box explains why the game ended
    public Text gameOverReasonText;
    //The text box displaying the player's final score
    public Text gameOverScoreText;

    //UI elements
    public GameObject mainMenu;
    public GameObject controlsMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public Button mainMenuArcadeButton;
    public Button mainMenuControlsButton;
    public Button mainMenuExitGameButton;
    public Button controlsMenuButton;
    public Button pauseRetryButton;
    public Button pauseQuitButton;
    public Button gameOverRetryButton;
    public Button gameOverQuitButton;

    //Game objects to activate when the game is started
    public List<GameObject> gameObjects;
    //UI objects to deactivate when the game is started
    public List<GameObject> menuObjects;

    //Calls the GameController.cs script
    private GameController gameController;
    //The scene for the project
    private Scene main;
    //Checks if the player selected the retry button when the scene is reloaded
    private bool playerSelectedRetry = false;

    void Start()
    {
        //Returns the speed of the game to normal after reloading the scene
        Time.timeScale = 1;

        //Defines the active scene as the main scene
        main = SceneManager.GetActiveScene();

        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        //Registers events for when each button is clicked
        mainMenuArcadeButton.onClick.AddListener(() => buttonCallBack(mainMenuArcadeButton));
        mainMenuControlsButton.onClick.AddListener(() => buttonCallBack(mainMenuControlsButton));
        mainMenuExitGameButton.onClick.AddListener(() => buttonCallBack(mainMenuExitGameButton));
        controlsMenuButton.onClick.AddListener(() => buttonCallBack(controlsMenuButton));
        pauseRetryButton.onClick.AddListener(() => buttonCallBack(pauseRetryButton));
        pauseQuitButton.onClick.AddListener(() => buttonCallBack(pauseQuitButton));
        gameOverRetryButton.onClick.AddListener(() => buttonCallBack(gameOverRetryButton));
        gameOverQuitButton.onClick.AddListener(() => buttonCallBack(gameOverQuitButton));

        //Checks for a saved boolean variable and creates it if it does not yet exist
        if (Save.Contains("PlayerSelectedRetry") == false)
        {
            Save.SetBool("PlayerSelectedRetry", playerSelectedRetry);
        }
        //If the boolean exists
        else
        {
            //Check the value of the boolean
            playerSelectedRetry = Save.GetBool("PlayerSelectedRetry");

            //If its true, the player has selected retry and the application loads Arcade mode
            if (playerSelectedRetry)
            {
                playerSelectedRetry = false;
                Save.SetBool("PlayerSelectedRetry", playerSelectedRetry);
                StartGame();
            }
        }
    }

    void Update()
    {
        //Keep the Score text box updated with the player's score
        scoreText.text = gameController.score.ToString();
        //Keep the High Score text box updated with the player's high score
        highScoreText.text = "HIGH: " + gameController.highScore.ToString();
        //Update the Game Over score text box with the player's final score
        gameOverScoreText.text = gameController.score.ToString();

        //If the game is over, freeze game time and display the game over menu
        if (gameController.gameIsOver)
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0;
        }
        //If the player is not dead, and the Main and Control menus are not active
        else if (!gameController.gameIsOver && mainMenu.activeSelf == false && controlsMenu.activeSelf == false)
        {
            //The player can press Escape to turn the Pause menu on and off
            //This also freezes and unfreezes game time
            if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == false)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == true)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }

        /*//If the player presses the Enter key while the Controls Menu is active
        if (Input.GetKey(KeyCode.Return) && controlsMenu == true)
        {
            //Run the Start Game function
            StartGame();
        }*/
    }

    //Controls what happens when each button is pressed
    void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == mainMenuArcadeButton)
        {
            StartGame();
        }

        if (buttonPressed == mainMenuControlsButton)
        {
            mainMenu.SetActive(false);
            controlsMenu.SetActive(true);
        }

        if (buttonPressed == mainMenuExitGameButton)
        {
            Application.Quit();
        }

        if (buttonPressed == controlsMenuButton)
        {
            controlsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        if (buttonPressed == pauseRetryButton)
        {
            playerSelectedRetry = true;
            Save.SetBool("PlayerSelectedRetry", playerSelectedRetry);

            SceneManager.LoadScene(main.name);
        }

        if (buttonPressed == pauseQuitButton)
        {
            SceneManager.LoadScene(main.name);
        }

        if (buttonPressed == gameOverRetryButton)
        {
            playerSelectedRetry = true;
            Save.SetBool("PlayerSelectedRetry", playerSelectedRetry);

            SceneManager.LoadScene(main.name);
        }

        if (buttonPressed == gameOverQuitButton)
        {
            SceneManager.LoadScene(main.name);
        }
    }

    //A function for activating game objects relevant to the game and deactivating menus
    void StartGame()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(true);
        }

        for (int i = 0; i < menuObjects.Count; i++)
        {
            menuObjects[i].SetActive(false);
        }
    }
}
