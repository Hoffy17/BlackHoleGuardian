using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //-----------------------------------------------------------------------------Public Variables (Value-Types)
    //Rate at which the player can fire projectiles
    public float refireRate;
    //Rate at which the player can fire projectiles with a rapid weapon upgrade
    public float refireRateRapid;
    //Checks whether the player is able to fire projectiles
    [HideInInspector] public bool canShoot;

    //-----------------------------------------------------------------------------Public Variables (Reference-Types)
    //List of player projectile types
    public List<GameObject> projectileObject;
    //List of extra weapons that are activated by the wide weapon upgrade
    public List<GameObject> weaponsToActivate;
    //Sound effect played when the player fires a projectile
    public AudioSource sfxWeaponShoot;

    //-----------------------------------------------------------------------------Private Variables (Value-Types)
    //Amount of time that is required before the player can again fire projectiles
    private float timePassed;

    //-----------------------------------------------------------------------------Private Variables (Reference-Types)
    //Calls the following scripts
    private GameController gameController;
    private UIController uiController;


    void Start()
    {
        //At the start of the game, the player can shoot projectiles
        canShoot = true;
        timePassed = 0.0f;

        //Find these scripts and updates their public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();
    }


    void Update()
    {
        //If the player presses Space and they are able to shoot projectiles, run the Shoot function
        if (Input.GetKey(KeyCode.Space) && (canShoot == true))
            Shoot();

        //Count time until the player can shoot again
        timePassed += Time.deltaTime;

        //If the amount of time passed is greater than or equal to the rate at which the player is able to shoot
        //Allow the player to shoot again
        if (timePassed >= refireRate)
            canShoot = true;

        //When this bool is true, activate the Guardian's extra weapons
        if (gameController.wideActivated)
            ActivateWeapons();

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
        //If the player doesn't have the large weapon upgrade, fire regular projectiles
        if (!gameController.largeActivated)
        {
            Instantiate(projectileObject[0], transform.position, transform.rotation);
            sfxWeaponShoot.Play();
        }
        //If the player has the large weapon upgrade, fire large projectiles
        else
        {
            Instantiate(projectileObject[1], transform.position, transform.rotation);
            sfxWeaponShoot.Play();
        }

        //Create a delay between shots fired
        canShoot = false;
        timePassed = 0.0f;
    }


    //For each weapon that hasn't been activated, activate it
    private void ActivateWeapons()
    {
        for (int i = 0; i < weaponsToActivate.Count; i++)
            weaponsToActivate[i].SetActive(true);
    }
}
