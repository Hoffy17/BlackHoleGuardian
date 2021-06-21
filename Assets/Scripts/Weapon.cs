using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    //Checks whether the player is able to fire projectiles
    public bool canShoot;
    //The rate at which the player can fire projectiles
    public float refireRate;
    //The rate at which the player can fire projectiles with a rapid weapon upgrade
    public float refireRateRapid;

    //A list of player projectile types
    public List<GameObject> projectileObject;
    //A list of extra weapons that are activated by a weapon upgrade
    public List<GameObject> weaponsToActivate;

    //The sound effect played when the player fires a projectile
    public AudioSource shootSound;

    //The amount of time that is required before the player can again fire projectiles
    private float timePassed;

    //Calls the GameController.cs script
    private GameController gameController;
    //Calls the UIController.cs script
    private UIController uiController;

    //At the start of the game, the player can shoot projectiles
    void Start()
    {
        canShoot = true;
        timePassed = 0.0f;

        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        //Finds the UI Controller and updates its public variables
        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();
    }

    void Update()
    {
        //If the player presses Space and they are able to shoot projectiles
        if (Input.GetKey(KeyCode.Space) && (canShoot == true))
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
        if (gameController.wideActivated)
        {
            for (int i = 0; i < weaponsToActivate.Count; i++)
            {
                weaponsToActivate[i].SetActive(true);
            }
        }

        //When this bool is true, shorten the refire rate
        if (gameController.rapidActivated)
            refireRate = refireRateRapid;

        //When the player is dead, or the pause menu is active, the player cannot shoot
        if (gameController.isDead || uiController.pauseMenu.activeSelf == true)
            canShoot = false;
    }

    //Handles the player's ability to fire projectiles
    //Instantiate projectiles at the player's location, in the direction the weapon is facing
    private void Shoot()
    {
        //If the player doesn't have the large weapon upgrade, fire regular projects
        if (!gameController.largeActivated)
        {
            Instantiate(projectileObject[0], transform.position, transform.rotation);
            shootSound.Play(0);
        }
        //If the player has the large weapon upgrade, fire large projects
        else
        {
            Instantiate(projectileObject[1], transform.position, transform.rotation);
            shootSound.Play(0);
        }
    }
}
