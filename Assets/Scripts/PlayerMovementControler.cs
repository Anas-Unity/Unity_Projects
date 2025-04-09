using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class PlayerMovementController : MonoBehaviour
{
    public InputAction moveInputAction;
    Vector2 moveDirection;

    public Text gameScoreText, gameEndScoreText, gameHighScoreText;
    int gameScore = 0, highScore;
    public float playerSpeed = 5, playerMaxSpeed = 15;  //by default variables are private but we can make them public to show them in unity inspector to change value
    public float turnSpeed, horizontalMovement, verticalMovement, jumpMovementForce, jumpInput;

    public Rigidbody playerRB;
    public bool isGrounded, gameOver;

    public GameObject startBtn, gameEndCanvas, restartGame;

/*
 float turnSpeed variable is used to provide speed to our object in horizontal axis
 float horizontalMovement variable is used to move our object in horizntal axis " left, right "
 float verticalMovement variable is used to move our object in vertical axis    " front, back "
 */



    //we have different data types in unity
    public Vector3 movement = new Vector3(0,0,1);//we can save (x,y,z) values in vector3 variable

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        playerRB = GetComponent<Rigidbody>();       // it will get rigidBody of same gameObject with which the script is attached.

    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == false) 
        {
            horizontalMovement = Input.GetAxis("Horizontal"); // this is used to move object in horizontal axis
            verticalMovement = Input.GetAxis("Vertical"); // this is used to move object in horizontal axis
            jumpMovementForce = Input.GetAxis("Jump");  //this is used to allow jumping to our object

            if(playerSpeed < playerMaxSpeed)
            {
                playerSpeed += 0.3f * Time.deltaTime;
            }


            //transform.Translate(new Vector3(0,0,1)*Time.deltaTime*speed);  //working with new vector3 values as hardcoded
            //transform.Translate(movement*Time.deltaTime*speed);  //working with vector3 varible
            // "Time.deltaTime" help to move object at default speed and we can multiply it with any number to increase its speed
            //transform.Translate(Vector3.right*Time.deltaTime*speed);  //working with vector3.right function which works in moving right
            //transform.Translate(Vector3.up*Time.deltaTime*speed);  //working with vector3.up function which works in moving upward

            //transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalMovement);      //working with vector3.forward function which works in moving forward
            //transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);      //working with vector3.forward function which works in moving forward
            //transform.Translate(Vector3.right * Time.deltaTime * turnSpeed * horizontalMovement);      //working with vector3.right function which works in moving right

            moveDirection = moveInputAction.ReadValue<Vector2>();
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            transform.Translate(Vector3.right * Time.deltaTime * turnSpeed * moveDirection.x);


            if (jumpMovementForce > 0 && isGrounded)
            // jumpMovementForce stores force to detect if space is pressed or not
            // isGrounded is checking if the object is colliding with ground or not
            {
                playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, jumpInput, playerRB.linearVelocity.z);
                /*
                 linearVilocity is used to provide movement for our object in (x,y,z) axis
                 jumpInput variale has value which we are using to assingn our object value in Y-axis for jumping

                 */
            }


        } 
    }

    private void OnEnable()
    {
        moveInputAction.Enable();
    }

    private void OnDisable()
    {
        moveInputAction.Disable();
    }

    private void OnCollisionEnter(Collision colliderObject)    // this function is checking if the object is in collision 
    {

        if (colliderObject.gameObject.tag == "Ground")      // this will run if the object is in collision with ground
        {
            isGrounded = true;
        }

    }

    private void OnCollisionExit(Collision colliderObject)      // this function is checking if the object has exit collision
    {
        if (colliderObject.gameObject.tag == "Ground")      // this condition will run if the object has exit collision with ground
        {
            isGrounded = false;
        }
    }

    /*private void OnCollisionEnter(Collision colliderObject)     // this function will run when trigger is unchecked on box collider in unity
    {

        if (colliderObject.gameObject.tag == "Coin")
        {
            Debug.Log("Collided with Coin");        //this function is used to show output on unity console
            Destroy(colliderObject.gameObject);     // this function is used to distroy gameObjects
        }

    }*/
    private void OnTriggerEnter(Collider triggerObject)     // this function will run when trigger is checked on box collider in unity
    {

        if (triggerObject.gameObject.tag == "Coin")
        {

            //PlayerPrefs.SetInt("highScore", gameScore);  //playerprefs is used to store temporary data
            //int highScore = PlayerPrefs.GetInt("highScore", 0); //show highest score
            

            gameScore++;
            //Debug.Log("Score: " + gameScore);
            //Debug.Log("Triggered with Coin");        //this function is used to show output on unity console
            Destroy(triggerObject.gameObject);       // this function is used to distroy gameObjects

            gameScoreText.text = gameScore.ToString();

        }
        else if (triggerObject.gameObject.tag == "Hurdle")
        {
            Debug.Log("Triggered with Hurdle");        //this function is used to show output on unity console
            EndGame();
            //gameScoreText.text = "SCORE: " + gameScore;
        }

    }

    public void StartGame()
    {
        startBtn.SetActive(false);

        gameOver = false;
    }
    public void EndGame()
    {

        highScore = PlayerPrefs.GetInt("highScore", 0);
        if (gameScore > highScore)
        {

            //Debug.Log("game score: "+gameScore);
            PlayerPrefs.SetInt("highScore", gameScore);
            highScore = gameScore;
        }
        //Debug.Log("high score: "+highScore);
        gameHighScoreText.text = "High Score: " + highScore.ToString();

        gameOver = true;
        gameEndScoreText.text = "Score: " + gameScore.ToString();
        gameEndCanvas.SetActive(true);


    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
//  task 1) show highest score , 2) manage canvas in different aspect ratios , 3) create prefab