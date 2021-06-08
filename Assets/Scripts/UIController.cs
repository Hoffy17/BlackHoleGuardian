using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider healthBar;
    public Text scoreText;
    public Text highScoreText;
    public Button controlsOK;

    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDeactivate;

    private GameController gameController;
    private GameObject controlsMenu;

    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        controlsMenu = GameObject.Find("Controls");

        Button btn = controlsOK.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void Update()
    {
        scoreText.text = gameController.score.ToString();
        highScoreText.text = "HIGH: " + gameController.highScore.ToString();

        if (Input.GetKey(KeyCode.Return) && controlsMenu == true)
        {
            StartGame();
        }
    }

    void TaskOnClick()
    {
        StartGame();
    }

    void StartGame()
    {
        for (int i = 0; i < objectsToActivate.Count; i++)
        {
            objectsToActivate[i].SetActive(true);
        }

        for (int i = 0; i < objectsToDeactivate.Count; i++)
        {
            objectsToDeactivate[i].SetActive(false);
        }
    }
}
