using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //-----------------------------------------------------------------------------Public Variables (Value-Types)
    //Variable rates for which spawners should spawn game objects
    public List<float> spawnDelayList;
    //Scores that the player must reach before the spawn delay decreases
    public List<float> levelControllers;
    //Scores that the player must reach before faster enemies are spawned instead of slower ones
    public List<float> difficultyControllers;
    //Checks whether a weapon upgrade is in the scene
    [HideInInspector]
    public bool weaponUpgradeInScene;

    //-----------------------------------------------------------------------------Public Variables (Reference-Types)
    //A list of spawner objects in the scene
    public List<GameObject> spawners;
    //A list of enemy prefabs to be spawned
    public List<GameObject> enemyToSpawn;
    //A list of weapon upgrade prefabs to be spawned
    public List<GameObject> weaponUpgradeToSpawn;

    //-----------------------------------------------------------------------------Private Variables (Value-Types)
    //The current rate at which spawners should spawn game objects
    private float spawnDelay;
    //Measures time since the last enemy was spawned
    private float timePassed;
    //Checks whether a spawner can spawn
    private bool canSpawn;

    //-----------------------------------------------------------------------------Private Variables (Reference-Types)
    //Calls the Box Collider component for spawners in the scene
    private BoxCollider spawnBounds;
    //Calls the GameController.cs script
    private GameController gameController;


    void Start()
    {
        //At the start of the game, the enemy spawner cannot spawn enemies
        canSpawn = false;
        //And there is not a weapon upgrade in the scene
        weaponUpgradeInScene = false;

        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }


    void Update()
    {
        //Changes the rate at which spawners can spawn, as the player's score increases
        LevelController();

        //Runs through each spawner in the scene and spawns enemies and weapon upgrades
        for (int i = 0; i < spawners.Count; i++)
        {
            //Calls the box collider component for a spawner and gets its size
            spawnBounds = spawners[i].GetComponent<BoxCollider>();

            //Checks if the spawner can spawn
            if (canSpawn)
                //Handles the spawner's ability to spawn
                Spawn();

            //Count time until the spawner can spawn again
            timePassed += Time.deltaTime;

            //If the amount of time passed is greater than or equal to the rate at which the spawner is able to spawn
            if (timePassed >= spawnDelay)
            {
                //Allow the spawner to spawn again
                canSpawn = true;
            }
        }
    }


    private void Spawn()
    {
        //Defines the random range within which game objects can spawn, as the bounds of the box collider
        float xPos = Random.Range((spawnBounds.size.x * -0.5f), (spawnBounds.size.x * 0.5f)) + spawnBounds.gameObject.transform.position.x;
        float zPos = Random.Range((spawnBounds.size.z * -0.5f), (spawnBounds.size.z * 0.5f)) + spawnBounds.gameObject.transform.position.z;
        //Create a Vector3 for a game object's spawn position
        Vector3 spawnPos = new Vector3(xPos, 0.0f, zPos);

        //Define the game object's rotation variable as a Quaternion
        Quaternion spawnRot = new Quaternion();
        //Then the Euler function converts the spawned object's rotation into a Vector3
        spawnRot = Quaternion.Euler(0f, 0f, 0f);

        //Grab a random number with a range
        int spawnChance = Random.Range(0, 10);

        //Based on the player's score, each random int spawns an enemy variant
        if (spawnChance < 2 && gameController.score >= difficultyControllers[0])
            Instantiate(enemyToSpawn[1], spawnPos, spawnRot);
        else if (spawnChance == 3 && gameController.score >= difficultyControllers[1])
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        else if (spawnChance == 4 && gameController.score >= difficultyControllers[2])
            Instantiate(enemyToSpawn[1], spawnPos, spawnRot);
        else if (spawnChance == 5 && gameController.score >= difficultyControllers[3])
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        else if (spawnChance == 6 && gameController.score >= difficultyControllers[4])
            Instantiate(enemyToSpawn[1], spawnPos, spawnRot);
        else if (spawnChance == 7 && gameController.score >= difficultyControllers[5])
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        else if (spawnChance == 8 && gameController.score >= difficultyControllers[6])
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        //If a weapon upgrade is not currently in the scene, there's a 10% chance of spawning a weapon upgrade
        else if (spawnChance == 9 && weaponUpgradeInScene == false && gameController.score >= difficultyControllers[0])
        {
            //Another random int is rolled
            int spawnChanceWU = Random.Range(0, 3);

            //Each random int spawns a different weapon upgrade, provided the player doesn't already have that upgrade
            if (spawnChanceWU == 0 && gameController.wideActivated == false)
                Instantiate(weaponUpgradeToSpawn[0], spawnPos, spawnRot);
            else if (spawnChanceWU == 1 && gameController.rapidActivated == false)
                Instantiate(weaponUpgradeToSpawn[1], spawnPos, spawnRot);
            else if (spawnChanceWU == 2 && gameController.largeActivated == false)
                Instantiate(weaponUpgradeToSpawn[2], spawnPos, spawnRot);
            //If none of the above conditions are met, an enemy is spawned
            else
                Instantiate(enemyToSpawn[1], spawnPos, spawnRot);

            //When a weapon upgrade is spawned, set this boolean to true
            weaponUpgradeInScene = true;
        }
        //If none of the above conditions are met, the slowest enemy type is spawned
        else
            Instantiate(enemyToSpawn[0], spawnPos, spawnRot);

        //Disable the spawner's ability to spawn
        canSpawn = false;
        //Start the timer until the spawner can spawn again
        timePassed = 0.0f;
    }


    private void LevelController()
    {
        if (gameController.score < levelControllers[0])
            spawnDelay = spawnDelayList[0];
        else if (gameController.score >= levelControllers[0] && gameController.score < levelControllers[1])
            spawnDelay = spawnDelayList[1];
        else if (gameController.score >= levelControllers[1] && gameController.score < levelControllers[2])
            spawnDelay = spawnDelayList[2];
        else if (gameController.score >= levelControllers[2] && gameController.score < levelControllers[3])
            spawnDelay = spawnDelayList[3];
        else if (gameController.score >= levelControllers[3] && gameController.score < levelControllers[4])
            spawnDelay = spawnDelayList[4];
        else if (gameController.score >= levelControllers[4] && gameController.score < levelControllers[5])
            spawnDelay = spawnDelayList[5];
        else if (gameController.score >= levelControllers[5] && gameController.score < levelControllers[6])
            spawnDelay = spawnDelayList[6];
        else if (gameController.score >= levelControllers[6] && gameController.score < levelControllers[7])
            spawnDelay = spawnDelayList[7];
        else if (gameController.score >= levelControllers[7] && gameController.score < levelControllers[8])
            spawnDelay = spawnDelayList[8];
        else if (gameController.score >= levelControllers[8])
            spawnDelay = spawnDelayList[9];
    }
}
