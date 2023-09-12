using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //Sets aside components from the player for functions to access, as well as childed particles.
    private Rigidbody playerRB;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public ParticleSystem explosion;
    public ParticleSystem dirt;

    /*scripts handling jumping. jumpCounter is used for limiting double jumps. 
     */
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public AudioClip jumpSound;
    private int jumpCounter = 0;

    /*Scripts around scoring, preGame, and gameOff states (used before play begins and after game over). 
     */
    public bool gameOff;
    public bool preGame;
    public Transform startingLine;
    public AudioClip crashSound;
    public float startSpeed;
    private float score = 0;
    public Text scoreBoard;

    //bool that manages what happens when a player dashes
    public bool dashBool = false;


    void Start()
    {
        //assigns private vars to their specific components
        playerRB = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        
        /*before using gravityModifier we reset the gravity to a base value. Because the modifier 
         * is expressed as a multiplier, and because physics is a static variable, the multiplication become 
         * cumulative with every reload of the scene. (Not sure why the tutorial didn't have us just set the
         * gravity value we want instead, but w/e). Having a public variable also gives us something to play 
         * with in inspector or on input if we wanted I guess*/
        Physics.gravity = new Vector3(0, -8.0f, 0);
        Physics.gravity *= gravityModifier;
        
        //updates the score constanty in debug.log. Redundant if using the UI text scoreboard.
        InvokeRepeating("ScorePrinter", 0.5f, 0.5f);
        
        //sets the default scoreboard string for the text UI object
        scoreBoard.text = "";
    }


    void Update()
    {
        //starts scoring only after preGame has ended.
        if(preGame == false)
        {
            Scorer();
        }

        //during preGame moves the player to starting position
        if (preGame)
        {
            PreMover();
        }
        
        //All inputs in a separate function to keep Update method clean
        Inputs();
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*while player is on ground and while the game is active (not after obstacle impact) we run the 
         * dirt particles and set our jumpCounter to 0 to allow for double jump.*/
        if (collision.gameObject.CompareTag("Ground") && gameOff == false)
        {
            //isOnGround = true;
            jumpCounter = 0;
            dirt.Play();
        }
        
        /*but if we collide with an object tagged "Obstacle" we: stop the repeated score update, print game over,
        print the final score, set a bool to gameOff, play a specific death animation, play an explosion particle
        stop playing the running/dirt particle, play a crash sound*/
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            CancelInvoke();
            Debug.Log("Game Over");
            Debug.Log("Score =" + score);
            gameOff = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosion.Play();
            dirt.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }

    /*when the "Starter" tagged trigger area is entered we switch an animation bool to go from walking to running,
     * end the preGame phase, end the gameOff phase, and then destroy the object to prevent it from being re-triggered.
     * (knowing it will respawn when we reload the game)*/
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Starter"))
        {
            playerAnim.SetBool("preGame", false);
            gameOff = false;
            preGame = false;
            Destroy(other.gameObject);
        }
    }

    //CUSTOM FUNCTIONS BELOW

    //Inputs wrapped into a single function
    public void Inputs()
    {
        /*When the spacebare is pressed and gameOff is False (game is running) and the jump couter is
         * less than 2, a jump force is applied as an impulse, we become ungrounded, play a jump anim,
         * play a jump sound effect, stop the dirt particle and increment the jump counter by 1 (so
         * as to limit us to only 2 jumps, per if statement*/
        if (Input.GetKeyDown(KeyCode.Space) && !gameOff && jumpCounter < 2)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            dirt.Stop();
            jumpCounter += 1;
        }

        //on press of left control we reload this scene, which is number 0 in build settings.
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(0);
        }

        //on HOLDING DOWN w, we call the Dash function
        if (Input.GetKey(KeyCode.W))
        {
            Dash();
        }
        /*but if we are NOT holding it down we run the animation at 1.5f (its default speed here.) and
        we set the dashBool to false. This dashBool is crucial as it is what's passed to the moveLeft
        script to toggle the speed of obstacles, background, and ground texture scrolling--those values
        reside within that script vs being set here*/
        else
        {
            playerAnim.speed = 1.5f;
            dashBool = false;
        }
        
    }

    /*a basic transform mover to take the player model to the starting position. Could not get root motion
     * in animation to work, so this is the jank solution. Causes shuddering in the model. Could maybe be
     * fixed by applying it to an empty parent.*/
    private void PreMover()
    {
        transform.position = Vector3.MoveTowards(transform.position, startingLine.position, startSpeed * Time.deltaTime);
    }
    
    /*The scorer is essentially a timer, keeping track of how long you make it vs number of jumps or w/e. If dashing,
     * score is multipled by 2, so it's more dangerous, but more valuable. When the game is NOT off the score is 
     * turned to a string that is sent to the scorboard text UI element (vs just debug.log)*/
    private void Scorer()
    {
        if (dashBool)
        {
            score += Time.deltaTime * 2;
        }
        else
        {
            score += Time.deltaTime;
        }

        if (!gameOff)
        {
            scoreBoard.text = score.ToString();
        }
    }
    
    /*Function called by invokeRepeating to get regular score updates in debug.log, redundant if using UI Text*/
    private void ScorePrinter()
    {
        Debug.Log("Score =" + score);
    }

    /*inscreases the speed of the animations on the player (run looks faster) and sets the dashBool true (which 
     * passes to moveLeft to increase speeds of obstacles, background, and ground texture.*/
    void Dash()
    {
        playerAnim.speed = 2;
        dashBool = true;
    }
    
}
