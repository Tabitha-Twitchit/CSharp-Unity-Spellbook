using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    private Rigidbody playerRB;
    private GameObject focalPoint;
    
    private float destroyDistance = -7.5f;

    [Header("All Powerups")]
    public int powerupDuration;

    [Header("Black Hole Powerup")]
    public GameObject powerupIndicator;
    public bool hasPowerUp = false;
    public float powerUpStrength;

    [Header("Comet Powerup")]
    public GameObject powerupIndicator1;
    public GameObject projectile;
    public bool hasPowerUp1 = false;
    public bool canFire = false;
    private float projectileCooldownTime = 0.5f;

    [Header("Coffee Bounce Powerup")]
    public GameObject powerupIndicator2;
    public GameObject explosion;
    public bool hasPowerUp2 = false;
    public float jumpForce;
    public bool isAirborn = false;


    [Header("Satellite Belt Powerup")]
    public GameObject powerupIndicator3;
    public GameObject powerupMoon;
    public bool hasPowerUp3 = false;


    void Start()
    {
        //by tracking the direction the focal point faces we determin the direction of movement for the marble.
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    private void Update()
    {
        /*we move the powerup indicator along with the player with a slight offset. This happens irrespective
         * of whether it's enabled*/
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerupIndicator1.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerupIndicator2.transform.position = transform.position + new Vector3(0, 3.5f, 0);
        powerupMoon.transform.position = transform.position;



        /*After firing, canFire is set to false until a cooldown period set in the coroutine. Prevents spamming*/
        if (Input.GetKeyDown(KeyCode.Space) && hasPowerUp1 && canFire)
        {
            canFire = false;
            Instantiate(projectile, transform.position, transform.rotation);
            StartCoroutine(FireCooldown());
        }

        /*Jumping sets isAirBorn to true to avoid double jumps and adds impulse up on the player (aka jumps)*/
        if (Input.GetKeyDown(KeyCode.Space) && hasPowerUp2 && !isAirborn)
        {
            isAirborn = true;
            playerRB.AddForce(focalPoint.transform.up * jumpForce, ForceMode.Impulse);
        }

        //restarts on pressing R
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        //restarts scene if player falls, this also intrinsically resets the wave number/enemy number
        if (transform.position.y < destroyDistance)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }
    void FixedUpdate() 
    {
        //creates a parameter keyed to the input that applies force to the player rigidbody.
        float forwardInput = Input.GetAxis("Vertical");
        playerRB.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if you hit a powerup trigger, turn on the boolean, make the indicator visible, destroy the powerup
         * object and begin a coroutine that counts down its duration.*/
        if(other.CompareTag("Powerup"))
        {
            hasPowerUp = true;  
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }

        if (other.CompareTag("Powerup1"))
        {
            hasPowerUp1 = true;
            canFire = true;
            powerupIndicator1.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }

        if (other.CompareTag("Powerup2"))
        {
            hasPowerUp2 = true;
            powerupIndicator2.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }

        //same, but here the indicator and effect are essentially the same
        if (other.CompareTag("Powerup3"))
        {
            hasPowerUp3 = true;
            Destroy(other.gameObject);
            powerupMoon.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }

        /*ground2 is an additional invisible trigger above the playing field. This differentiates it from the
         * actual ground which is not setup as a trigger. This creates continuous explosions on contact with
         * the collider until the hasPowerup2 is set to false when the PowerupCountdownRoutine ends.*/
        if (other.CompareTag("ground2") && hasPowerUp2 && isAirborn)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    /*the cool down loop for the comet powerup, this is separate from the loop that turns the powerup off,
     * it sets a pause between when the player can fire so they can't spam comets*/
    IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(projectileCooldownTime);
        canFire = true;
    }
    /*wait for a number of seconds before feeding back and resetting the indicator and boolean*/
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(powerupDuration);
        powerupIndicator.gameObject.SetActive(false);
        powerupIndicator1.gameObject.SetActive(false);
        powerupIndicator2.gameObject.SetActive(false);
        powerupMoon.gameObject.SetActive(false);
        hasPowerUp = false;
        hasPowerUp1 = false;
        hasPowerUp2 = false;
        hasPowerUp3 = false;
        isAirborn = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if the player has a powerup and collides with an enemy
        if(collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            //access the enemies rigidbody, and establish a vector in the opposite direction of the player 
            Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>(); 
            Vector3 awayFromPlayer = collision.gameObject.transform.position -transform.position;

            //add force as an impulse in the direction opposite the player multiplied by the powerupstrength
            enemyRB.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }


    }


}
