using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //The axis at which the player rotates around the Black Hole
    public Vector3 rotationAxis = Vector3.up;
    //The speed at which that rotation occurs
    public float rotationSpeed;
    public float wallRotation;

    //Calls the GameController.cs script
    private GameController gameController;
    private Wall wall;

    private CharacterController playerController;
    private Vector3 directionVector = new Vector3();
    private Vector3 previousRotationDirection = Vector3.forward;

    void Start()
    {
        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        wall = GameObject.Find("Wall").GetComponent<Wall>();

        playerController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        //If the player is alive, they can use the D and A keys to rotate around the Black Hole
        if (!gameController.isDead)
        {
            if (Input.GetKey(KeyCode.D))
            {
                //if ((transform.rotation.y * Mathf.Rad2Deg) < wallRotation)
                //{
                    Debug.Log(transform.rotation.y * Mathf.Rad2Deg);
                    transform.Rotate(rotationAxis * Time.deltaTime * rotationSpeed);
                //}
            }

            if (Input.GetKey(KeyCode.A))
            {
                //if ((transform.rotation.y * Mathf.Rad2Deg) > -wallRotation)
                //{
                    Debug.Log(transform.rotation.y * Mathf.Rad2Deg);
                    transform.Rotate(-rotationAxis * Time.deltaTime * rotationSpeed);
                //}
            }

            //The player can also use the Q key to warp 180 degrees around the Black Hole
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.Rotate(0, 180, 0);
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
                gameController.isDead = true;

            /*float rotX = Input.GetAxis("Horizontal");
            float rotZ = Input.GetAxis("Vertical");
            directionVector = new Vector3(rotX, 0.0f, rotZ);

            if (directionVector.magnitude < 0.1f)
            {
                directionVector = previousRotationDirection;
            }

            directionVector = directionVector.normalized;
            previousRotationDirection = directionVector;

            //LookRotation grabs a Vector3, and this line outputs a rotation
            transform.rotation = Quaternion.LookRotation(directionVector);
            //playerController.Move(rotationAxis * Time.deltaTime * rotationSpeed);*/
        }
    }
}
