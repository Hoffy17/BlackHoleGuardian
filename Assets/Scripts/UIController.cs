using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //-----------------------------------------------------------------------------Public Variables (Reference-Types)
    //-------------------------------------------------------------Main Menu UI
    public GameObject mainMenu;
    public Button mainMenuArcadeButton;
    public Button mainMenuHowToPlayButton;
    public Button mainMenuOptionsButton;
    public Button mainMenuExitGameButton;
    //-------------------------------------------------------------How To Play UI
    public GameObject howToPlayMenu;
    public Button howToPlayMenuButton;
    //-------------------------------------------------------------Options UI
    public GameObject optionsMemu;
    public Button optionsMenuButton;
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
    //Sound effects
    public AudioSource selectSound;
    public AudioSource overheatAlarm;

    //-----------------------------------------------------------------------------Private Variables (Value-Types)
    //The scene for the project
    private Scene main;

    //-----------------------------------------------------------------------------Private Variables (Reference-Types)
    //Calls the following scripts
    private GameController gameController;
    //Checks the button that the player selected when the scene is reloaded
    private string playerRetryOrQuit = "";
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private string musicFreqCutoffParameter;


    void Start()
    {
        //Find these scripts and updates their public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        //Boot up the main menu
        mainMenu.SetActive(true);

        //Return the speed of the game to normal after reloading the scene
        Time.timeScale = 1;
        audioMixer.SetFloat(musicFreqCutoffParameter, 22000.0f);

        //Define the active scene as the main scene
        main = SceneManager.GetActiveScene();

        //Register events for when each UI button is clicked
        RegisterButtons();

        //Check for a saved string variable and create it if it does not yet exist
        if (Save.Contains("PlayerRetryOrQuit") == false)
            Save.SetString("PlayerRetryOrQuit", playerRetryOrQuit);
        //If the string exists
        else
        {
            //Check the string
            playerRetryOrQuit = Save.GetString("PlayerRetryOrQuit");

            //Depending on the selection, start the game
            if (playerRetryOrQuit == "Retry")
            {
                playerRetryOrQuit = "";
                Save.SetString("PlayerRetryOrQuit", playerRetryOrQuit);
                StartGame();

                selectSound.Play();
            }
            else if (playerRetryOrQuit == "Quit")
            {
                playerRetryOrQuit = "";
                Save.SetString("PlayerRetryOrQuit", playerRetryOrQuit);

                selectSound.Play();
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

        //If the player is not dead, and any of the menus are not active
        if (!gameController.isDead && mainMenu.activeSelf == false && howToPlayMenu.activeSelf == false && optionsMemu.activeSelf == false)
        {
            //The player can press Escape to turn the Pause menu on and off
            //This also freezes and unfreezes game time
            if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == false)
            {
                pauseMenu.SetActive(true);
                audioMixer.SetFloat(musicFreqCutoffParameter, 196.0f);
                selectSound.Play();
                overheatAlarm.Pause();
                Time.timeScale = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == true)
            {
                pauseMenu.SetActive(false);
                audioMixer.SetFloat(musicFreqCutoffParameter, 22000.0f);
                selectSound.Play();
                overheatAlarm.UnPause();
                Time.timeScale = 1;
            }
        }
    }


    private void RegisterButtons()
    {
        mainMenuArcadeButton.onClick.AddListener(() => ButtonEvent(mainMenuArcadeButton));
        mainMenuHowToPlayButton.onClick.AddListener(() => ButtonEvent(mainMenuHowToPlayButton));
        mainMenuOptionsButton.onClick.AddListener(() => ButtonEvent(mainMenuOptionsButton));
        mainMenuExitGameButton.onClick.AddListener(() => ButtonEvent(mainMenuExitGameButton));
        howToPlayMenuButton.onClick.AddListener(() => ButtonEvent(howToPlayMenuButton));
        optionsMenuButton.onClick.AddListener(() => ButtonEvent(optionsMenuButton));
        pauseRetryButton.onClick.AddListener(() => ButtonEvent(pauseRetryButton));
        pauseQuitButton.onClick.AddListener(() => ButtonEvent(pauseQuitButton));
        gameOverRetryButton.onClick.AddListener(() => ButtonEvent(gameOverRetryButton));
        gameOverQuitButton.onClick.AddListener(() => ButtonEvent(gameOverQuitButton));
    }


    //Controls what happens when each button is pressed
    private void ButtonEvent(Button buttonPressed)
    {
        if (buttonPressed == mainMenuArcadeButton)
        {
            StartGame();
            selectSound.Play();
        }

        if (buttonPressed == mainMenuHowToPlayButton)
        {
            mainMenu.SetActive(false);
            howToPlayMenu.SetActive(true);
            selectSound.Play();
        }

        if (buttonPressed == mainMenuOptionsButton)
        {
            mainMenu.SetActive(false);
            optionsMemu.SetActive(true);
            selectSound.Play();
        }

        if (buttonPressed == mainMenuExitGameButton)
            Application.Quit();

        if (buttonPressed == howToPlayMenuButton)
        {
            howToPlayMenu.SetActive(false);
            mainMenu.SetActive(true);
            selectSound.Play();
        }

        if (buttonPressed == optionsMenuButton)
        {
            optionsMemu.SetActive(false);
            mainMenu.SetActive(true);
            selectSound.Play();
        }

        if (buttonPressed == pauseRetryButton)
            Retry();

        if (buttonPressed == pauseQuitButton)
            Quit();

        if (buttonPressed == gameOverRetryButton)
            Retry();

        if (buttonPressed == gameOverQuitButton)
            Quit();
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
        playerRetryOrQuit = "Retry";
        Save.SetString("PlayerRetryOrQuit", playerRetryOrQuit);

        SceneManager.LoadScene(main.name);
    }


    private void Quit()
    {
        playerRetryOrQuit = "Quit";
        Save.SetString("PlayerRetryOrQuit", playerRetryOrQuit);

        SceneManager.LoadScene(main.name);
    }
}
