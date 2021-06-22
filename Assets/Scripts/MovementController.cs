﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //-----------------------------------------------------------------------------Public Variables (Value-Types)
    //Axis at which the player rotates around the Black Hole
    public Vector3 rotationAxis = Vector3.up;
    //Speed at which that rotation occurs
    public float rotationSpeed;
    //Rate at which the overheat meter increases
    public float overheatRate;
    //Rate at which the overheat meter decreases
    public float coolRate;
    //Checks whether to play particles when the player collects an upgrade
    [HideInInspector]
    public bool playUpgradePS;
    //Checks whether the Black Hole has started to collapse
    [HideInInspector]
    public bool blackHoleCollapsed;

    //-----------------------------------------------------------------------------Public Variables (Reference-Types)
    //Game Objects in the scene
    public GameObject guardian;
    public GameObject blackHole;
    //Toggles on and off when the player warps
    public GameObject warpPoint;
    //Particles when the player overheats
    public GameObject explosion;
    public ParticleSystem playerExplodePS;
    //Particles when the player collects an upgrade
    public GameObject getUpgradePS;
    //Animator controlling the Black Hole collapsing
    public Animator blackHoleAnimator;
    //Sound effect when the player overheats
    public AudioSource playerExplode;
    //Sound effect when the Black Hole collapses
    public AudioSource blackHoleCollapse;

    //-----------------------------------------------------------------------------Private Variables (Value-Types)
    //Counts time betweem Black Hole collapsing and Game Over menu displaying
    private float expandTime;
    //Checks if the player died because they overheated
    private bool overheated;
    //Checks if the player died because the Black Hole collapsed
    private bool swallowed;

    //-----------------------------------------------------------------------------Private Variables (Reference-Types)
    //Calls the following scripts
    private GameController gameController;
    private UIController uiController;


    void Start()
    {
        //Find these scripts and updates their public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();

        expandTime = 0.0f;
        blackHoleCollapsed = false;
        overheated = false;
        swallowed = false;
    }


    void Update()
    {
        //Checks if the player is alive
        if (!gameController.isDead)
            Movement();

        //When the player collects a weapon upgrade, a particle system is activated
        if (gameController.wideActivated && playUpgradePS)
            UpgradeParticles();
        if (gameController.rapidActivated && playUpgradePS)
            UpgradeParticles();
        if (gameController.largeActivated && playUpgradePS)
            UpgradeParticles();

        //When the player tries to fire their weapon, the overheat bar increases
        if (Input.GetKey(KeyCode.Space) && !overheated)
            Heat();
        //When the player is not firing their weapon, the overheat bar decreases
        else if (!Input.GetKey(KeyCode.Space) && !overheated)
            Cool();

        //Controls the timed sequence of the Black Hole collapsing
        CollapseBlackHole();

        //After the explosion particle system has been deactivated, the Game Over menu displays
        if (explosion.activeSelf == false && gameController.isDead == true && overheated)
            GameOver_Overheated();
        //After the Black Hole animation has finished, the Game Over menu displays
        if (expandTime >= 2.0f && gameController.isDead == true && swallowed == true)
            GameOver_Swallowed();
    }


    private void Movement()
    {
        //The player uses the D and A keys to rotate around the Black Hole
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(rotationAxis * Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(-rotationAxis * Time.deltaTime * rotationSpeed);

        //The player uses the Q key to warp 180 degrees around the Black Hole
        //On key down, a "ghost" appears displaying where the player will warp to
        if (Input.GetKeyDown(KeyCode.Q))
            warpPoint.SetActive(true);
        //On key up, the player warps and the "ghost" disappears
        if (Input.GetKeyUp(KeyCode.Q))
        {
            transform.Rotate(0, 180, 0);
            warpPoint.SetActive(false);
        }
    }


    private void Heat()
    {
        //Increase the overheat bar
        uiController.overheatBar.value += overheatRate;

        //If the overheat bar reaches the max value, the player overheats
        if (uiController.overheatBar.value >= uiController.overheatBar.maxValue)
        {
            overheated = true;
            gameController.isDead = true;

            //Turn off the player character and play the explosion particles
            guardian.SetActive(false);
            explosion.SetActive(true);
            Instantiate(playerExplodePS, guardian.transform.position + new Vector3(0, 1f, 0), guardian.transform.rotation);

            //Play the sound effect
            playerExplode.Play(0);
        }
    }


    private void Cool()
    {
        uiController.overheatBar.value -= coolRate;
    }


    private void UpgradeParticles()
    {
        getUpgradePS.SetActive(true);
        playUpgradePS = false;
    }


    private void CollapseBlackHole()
    {
        if (blackHoleCollapsed)
        {
            //Turn off the Black Hole's sphere collider
            blackHole.GetComponent<SphereCollider>().enabled = false;
            //Play the Black Hole's expanding animation
            blackHoleAnimator.SetBool("Black Hole Expand", true);
            //Play the sound effect
            blackHoleCollapse.Play(0);

            //Start the timer until Game Over menu is displayed
            expandTime = 0.0f;
            //Set this boolean to true to continue Game Over sequence
            swallowed = true;

            //Turn this boolean off so expandTime doesn't reset
            blackHoleCollapsed = false;
        }

        //Count time until Game Over
        expandTime += Time.deltaTime;
    }


    private void GameOver_Overheated()
    {
        //Set the reason that is shown on the Game Over screen
        if (swallowed)
            uiController.gameOverReasonText.text = "You overheated and the Black Hole collapsed!";
        else
            uiController.gameOverReasonText.text = "You overheated!";

        //Display the Game Over screen
        gameController.gameIsOver = true;
    }


    private void GameOver_Swallowed()
    {
        //Set the reason that is shown on the Game Over screen
        if (overheated)
            uiController.gameOverReasonText.text = "You overheated and the Black Hole collapsed!";
        else
            uiController.gameOverReasonText.text = "The Black Hole collapsed!";

        //Display the Game Over screen
        gameController.gameIsOver = true;
    }
}