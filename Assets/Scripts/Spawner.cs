using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    //The rate at which the spawner should spawn enemies
    public float spawnDelay;
    public bool weaponUpgradeInScene = false;
    public List<GameObject> spawners;
    //A public variable for enemy prefabs to be spawned
    public List<GameObject> enemyToSpawn;
    public List<GameObject> weaponUpgradeToSpawn;

    //A variable for measuring time since the last enemy was spawned
    private float timePassed;
    //A variable for checking whether an enemy can be spawned
    private bool canSpawn;
    //A variable to call the Box Collider component
    private BoxCollider spawnBounds;
    private GameController gameController;
    private Weapon weapon;

    //The spawn bounds are the same as the box collider of the object this script is assigned to
    //At the start of the game, the enemy spawner cannot spawn enemies
    void Start()
    {
        canSpawn = false;
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        weapon = GameObject.Find("Weapon_M").GetComponent<Weapon>();
    }

    void Update()
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            spawnBounds = spawners[i].GetComponent<BoxCollider>();

            //If the enemy spawner can spawn enemies
            if (canSpawn)
            {
                //Run the SpawnEnemy function and disble the spawner's ability to spawn
                SpawnEnemy();
                canSpawn = false;
                timePassed = 0.0f;
            }

            //Count time until the enemy spawner can spawn enemies
            timePassed += Time.deltaTime;

            //If the amount of time passed is greater than or equal to the rate at which the enemy spawner is able to spawn enemies
            if (timePassed >= spawnDelay)
                //Allow the enemy spawner to spawn enemies
                canSpawn = true;
        }
    }

    //A function that handles the enemy spawner's ability to spawn enemies
    private void SpawnEnemy()
    {
        //Defines the random range within which enemies can spawn
        float xPos = Random.Range((spawnBounds.size.x * -0.5f), (spawnBounds.size.x * 0.5f)) + spawnBounds.gameObject.transform.position.x;
        float zPos = Random.Range((spawnBounds.size.z * -0.5f), (spawnBounds.size.z * 0.5f)) + spawnBounds.gameObject.transform.position.z;
        //Create a variable for an enemy's position
        Vector3 spawnPos = new Vector3(xPos, 0.0f, zPos);

        //Define the enemy's rotation variable as a Quaternion
        Quaternion spawnRot = new Quaternion();
        // Then use the Euler function to convert the enemy's rotation into a Vector3
        spawnRot = Quaternion.Euler(0f, 0f, 0f);

        int spawnProbability = Random.Range(0, 10);

        //Spawn the enemy with at the random position and rotation
        if (spawnProbability < 2 && gameController.score >= 1500)
            Instantiate(enemyToSpawn[1], spawnPos, spawnRot);
        else if (spawnProbability == 3 && gameController.score >= 5000)
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        else if (spawnProbability == 9 && weapon.allWeaponsActivated == false && weaponUpgradeInScene == false)
        {
            Instantiate(weaponUpgradeToSpawn[0], spawnPos, spawnRot);
            weaponUpgradeInScene = true;
        }
        else
            Instantiate(enemyToSpawn[0], spawnPos, spawnRot);
    }
}
