using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //The axis at which the player rotates around the Black Hole
    public Vector3 rotationAxis = Vector3.up;
    //The speed at which that rotation occurs
    public float rotationSpeed;
    //The rate at which the weapons heat up when the player is trying to fire them
    public float overheatRate;
    //The rate at which the weapons cool down when the player is not trying to fire them
    public float coolRate;
    //public float wallRotation;

    public GameObject guardian;
    public GameObject blackHole;
    public GameObject explosion;
    public Animator blackHoleAnimator;

    private float expandTime;
    private bool swallowed;

    //Calls the GameController.cs script
    private GameController gameController;
    //Calls the UIController.cs script
    private UIController uiController;
    //private Wall wall;

    //private CharacterController playerController;
    //private Vector3 directionVector = new Vector3();
    //private Vector3 previousRotationDirection = Vector3.forward;

    void Start()
    {
        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        //Finds the UI Controller and updates its public variables
        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();
        //wall = GameObject.Find("Wall").GetComponent<Wall>();

        expandTime = 0.0f;
        swallowed = false;

        //playerController = gameObject.GetComponent<CharacterController>();
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
                    //Debug.Log(transform.rotation.y * Mathf.Rad2Deg);
                    transform.Rotate(rotationAxis * Time.deltaTime * rotationSpeed);
                //}
            }

            if (Input.GetKey(KeyCode.A))
            {
                //if ((transform.rotation.y * Mathf.Rad2Deg) > -wallRotation)
                //{
                    //Debug.Log(transform.rotation.y * Mathf.Rad2Deg);
                    transform.Rotate(-rotationAxis * Time.deltaTime * rotationSpeed);
                //}
            }

            //The player can also use the Q key to warp 180 degrees around the Black Hole
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.Rotate(0, 180, 0);
            }

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

        //When the player tries to fire their weapon, the overheat bar increases
        if (Input.GetKey(KeyCode.Space) && !gameController.overheated)
        {
            uiController.overheatBar.value += overheatRate;

            //If the overheat bar reaches the max value, the player overheats
            if (uiController.overheatBar.value >= uiController.overheatBar.maxValue)
            {
                gameController.overheated = true;
                gameController.isDead = true;

                //Turn off the player character and play the explosion particles
                guardian.SetActive(false);
                explosion.SetActive(true);
            }
        }
        //When the player is not firing their weapon, the overheat bar decreases
        else if (!Input.GetKey(KeyCode.Space) && !gameController.overheated)
            uiController.overheatBar.value -= coolRate;

        if (gameController.blackHoleCollapsed)
        {
            blackHoleAnimator.SetBool("Black Hole Expand", true);

            expandTime = 0.0f;

            swallowed = true;
            gameController.blackHoleCollapsed = false;
        }

        expandTime += Time.deltaTime;

        //After the explosion particle system has been deactivated, the game over screen displays
        if (explosion.activeSelf == false && gameController.isDead == true && gameController.overheated)
        {
            if (swallowed)
                uiController.gameOverReasonText.text = "You overheated and the Black Hole collapsed!";
            else
                uiController.gameOverReasonText.text = "You overheated!";

            gameController.gameIsOver = true;
        }

        if (expandTime >= 0.54f && gameController.isDead == true && swallowed == true)
        {
            if (gameController.overheated)
                uiController.gameOverReasonText.text = "You overheated and the Black Hole collapsed!";
            else
                uiController.gameOverReasonText.text = "The Black Hole collapsed!";

            gameController.gameIsOver = true;
        }
    }
}
