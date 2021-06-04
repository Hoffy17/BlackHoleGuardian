using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //A public variable for enemy prefabs to be spawned
    public GameObject enemyToSpawn;
    //The rate at which the spawner should spawn enemies
    public float spawnDelay;
    //The rotation of the spawner box
    public float spawnerBoxRotation;

    //A variable for measuring time since the last enemy was spawned
    private float timePassed;
    //A variable for checking whether an enemy can be spawned
    private bool canSpawn;
    //A variable to call the Box Collider component
    private BoxCollider spawnBounds;

    //At the start of the game, the enemy spawner can spawn enemies
    //The spawn bounds are the same as the box collider of the object this script is assigned to
    void Start()
    {
        canSpawn = true;
        spawnBounds = GetComponent<BoxCollider>();
    }

    void Update()
    {
        //If the enemy spawner can spawn enemies
        if (canSpawn)
        {
            //Run the SpawnEnemy function and disble the spawner's ability to spawn
            SpawnEnemy();
            canSpawn = false;
            timePassed = 0.0f;
        }

        //Count time until the enemy spawner can spawn enemies again
        timePassed += Time.deltaTime;

        //If the amount of time passed is greater than or equal to the rate at which the enemy spawner is able to spawn enemies
        if (timePassed >= spawnDelay)
            //Allow the enemy spawner to spawn enemies
            canSpawn = true;
    }

    //A function that handles the enemy spawner's ability to spawn enemies
    private void SpawnEnemy()
    {
        //Defines the random range within which enemies can spawn
        float xPos = Random.Range((spawnBounds.size.x * -0.5f), (spawnBounds.size.x * 0.5f)) + spawnBounds.gameObject.transform.position.x;
        float zPos = Random.Range((spawnBounds.size.z * -0.5f), (spawnBounds.size.z * 0.5f)) + spawnBounds.gameObject.transform.position.z;

        //Create a variable for an enemy's rotation
        Vector3 spawnPos = new Vector3(xPos, 0.0f, zPos);
        //Define the enemy's rotation variable as a Quaternion
        Quaternion spawnRot = new Quaternion();
        //Get a random angle betwen 0 and 180 degrees, and add the rotation of the spawner box
        float rotationAngle = Random.Range(0.0f, 180.0f) + spawnerBoxRotation;
        // Then use the Euler function to set that random rotation to the y-axis
        spawnRot = Quaternion.Euler(0f, rotationAngle, 0f);

        //Spawn the enemy with at the random position and rotation
        Instantiate(enemyToSpawn, spawnPos, spawnRot);
    }
}
