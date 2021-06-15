using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    //Creates a variable GameObject for player projectiles
    public GameObject projectileObject;
    //The rate at which the player can fire projectiles
    public float refireRate;
    //A list of extra weapons that are activated by a weapon upgrade
    public List<GameObject> weaponsToActivate;
    //Checks whether all weapons have been set to active
    [HideInInspector]
    public bool allWeaponsActivated;

    //The amount of time that is required before the player can again fire projectiles
    private float timePassed;
    //Checks whether the player is able to fire projectiles
    [HideInInspector]
    public bool canShoot;

    //Calls the GameController.cs script
    private GameController gameController;
    //Calls the UIController.cs script
    private UIController uiController;

    //At the start of the game, the player can shoot projectiles and extra weapons are not active
    void Start()
    {
        canShoot = true;
        allWeaponsActivated = false;
        timePassed = 0.0f;

        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        //Finds the UI Controller and updates its public variables
        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();
    }

    void Update()
    {
        //If the player presses Space and they are able to shoot projectiles
        if ((Input.GetKey(KeyCode.Space)) && (canShoot == true))
        {
            //Run the Shoot function and disable the player's ability to shoot
            Shoot();
            canShoot = false;
            timePassed = 0.0f;
        }

        //Count time until the player can shoot again
        timePassed += Time.deltaTime;

        //If the amount of time passed is greater than or equal to the rate at which the player is able to shoot
        if (timePassed >= refireRate)
        {
            //Allow the player to shoot
            canShoot = true;
        }

        //When this bool is true, activate the Guardian's extra weapons
        if (allWeaponsActivated)
        {
            for (int i = 0; i < weaponsToActivate.Count; i++)
            {
                weaponsToActivate[i].SetActive(true);
            }
        }

        if (gameController.isDead || uiController.pauseMenu.activeSelf == true)
            canShoot = false;
    }

    //Handles the player's ability to fire projectiles
    private void Shoot()
    {
        //Instantiate projectiles at the player's location, in the direction the weapon is facing
        Instantiate(projectileObject, transform.position, transform.rotation);
    }
}
