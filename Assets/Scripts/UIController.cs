using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //-----------------------------------------------------------------------------Public Variables (Reference-Types)
    //-------------------------------------------------------------Main Menu UI
    public GameObject mainMenu;
    public Button mainMenuArcadeButton;
    public Button mainMenuControlsButton;
    public Button mainMenuExitGameButton;
    //-------------------------------------------------------------How To Play UI
    public GameObject controlsMenu;
    public Button controlsMenuButton;
    //-------------------------------------------------------------Arcade UI
    //The health bar slider
    public Slider healthBar;
    //The overheat bar slider;
    public Slider overheatBar;
    //The score text box
    public Text scoreText;
    //The high score text box
    public Text highScoreText;
    //-------------------------------------------------------------Pause UI
    public GameObject pauseMenu;
    public Button pauseRetryButton;
    public Button pauseQuitButton;
    //-------------------------------------------------------------Game Over UI
    public GameObject gameOverMenu;
    //This text box explains why the game ended
    public Text gameOverReasonText;
    //The text box displaying the player's final score
    public Text gameOverScoreText;
    public Button gameOverRetryButton;
    public Button gameOverQuitButton;
    //-------------------------------------------------------------Other
    //Game objects to activate when the game is started
    public List<GameObject> gameObjects;
    //UI objects to deactivate when the game is started
    public List<GameObject> menuObjects;
    //The background music
    public AudioSource BGM;
    //Menu selection sound effect
    public AudioSource selectSound;

    //-----------------------------------------------------------------------------Private Variables (Value-Types)
    //The scene for the project
    private Scene main;
    //Checks if the player selected the retry button when the scene is reloaded
    private bool playerSelectedRetry = false;

    //-----------------------------------------------------------------------------Private Variables (Reference-Types)
    //Calls the GameController.cs script
    private GameController gameController;


    void Start()
    {
        //Returns the speed of the game to normal after reloading the scene
        Time.timeScale = 1;

        //Defines the active scene as the main scene
        main = SceneManager.GetActiveScene();

        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        //Registers events for when each UI button is clicked
        RegisterButtons();

        //Checks for a saved boolean variable and creates it if it does not yet exist
        if (Save.Contains("PlayerSelectedRetry") == false)
            Save.SetBool("PlayerSelectedRetry", playerSelectedRetry);
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
                BGM.Pause();
                selectSound.Play(0);
                Time.timeScale = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == true)
            {
                pauseMenu.SetActive(false);
                BGM.UnPause();
                selectSound.Play(0);
                Time.timeScale = 1;
            }
        }
    }


    private void RegisterButtons()
    {
        mainMenuArcadeButton.onClick.AddListener(() => buttonCallBack(mainMenuArcadeButton));
        mainMenuControlsButton.onClick.AddListener(() => buttonCallBack(mainMenuControlsButton));
        mainMenuExitGameButton.onClick.AddListener(() => buttonCallBack(mainMenuExitGameButton));
        controlsMenuButton.onClick.AddListener(() => buttonCallBack(controlsMenuButton));
        pauseRetryButton.onClick.AddListener(() => buttonCallBack(pauseRetryButton));
        pauseQuitButton.onClick.AddListener(() => buttonCallBack(pauseQuitButton));
        gameOverRetryButton.onClick.AddListener(() => buttonCallBack(gameOverRetryButton));
        gameOverQuitButton.onClick.AddListener(() => buttonCallBack(gameOverQuitButton));
    }


    //Controls what happens when each button is pressed
    void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == mainMenuArcadeButton)
        {
            StartGame();
            selectSound.Play(0);
        }

        if (buttonPressed == mainMenuControlsButton)
        {
            mainMenu.SetActive(false);
            controlsMenu.SetActive(true);
            selectSound.Play(0);
        }

        if (buttonPressed == mainMenuExitGameButton)
            Application.Quit();

        if (buttonPressed == controlsMenuButton)
        {
            controlsMenu.SetActive(false);
            mainMenu.SetActive(true);
            selectSound.Play(0);
        }

        if (buttonPressed == pauseRetryButton)
            Retry();

        if (buttonPressed == pauseQuitButton)
            SceneManager.LoadScene(main.name);

        if (buttonPressed == gameOverRetryButton)
            Retry();

        if (buttonPressed == gameOverQuitButton)
            SceneManager.LoadScene(main.name);
    }


    //A function for activating game objects relevant to the game and deactivating menus
    private void StartGame()
    {
        for (int i = 0; i < gameObjects.Count; i++)
            gameObjects[i].SetActive(true);

        for (int i = 0; i < menuObjects.Count; i++)
            menuObjects[i].SetActive(false);
    }


    private void Retry()
    {
        playerSelectedRetry = true;
        Save.SetBool("PlayerSelectedRetry", playerSelectedRetry);

        SceneManager.LoadScene(main.name);
    }
}
