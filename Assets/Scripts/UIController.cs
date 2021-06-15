using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //The health bar slider
    public Slider healthBar;
    //The score text box
    public Text scoreText;
    //The high score text box
    public Text highScoreText;
    public Text gameOverScoreText;

    //The Controls Menu game object
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

    //Game objects to activate when the Controls menu is closed
    public List<GameObject> gameObjects;
    //Game objects to deactivate when the Controls menu is closed
    public List<GameObject> menuObjects;

    //Calls the GameController.cs script
    private GameController gameController;
    private bool playerSelectedRetry = false;
    //The scene for the project
    private Scene main;

    void Start()
    {
        Time.timeScale = 1;

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

        if (Save.Contains("PlayerSelectedRetry") == false)
        {
            Save.SetBool("PlayerSelectedRetry", playerSelectedRetry);
        }
        else
        {
            playerSelectedRetry = Save.GetBool("PlayerSelectedRetry");

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
        gameOverScoreText.text = gameController.score.ToString();

        if (gameController.isDead)
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!gameController.isDead && mainMenu.activeSelf == false && controlsMenu.activeSelf == false)
        {
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
