using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provided mainly for educational purposes this script demonstrates a method intended to be complimentory to 
/// a "Do Not Destroy on Load" type player controller wherein some actions/events are more easily handled within 
/// the scene vs the char. The intent is that this script could be placed on a single prefab with the prefab 
/// dropped into all scenes and handle various animation events that are triggered by a keypress from the player 
/// advancing a counter.
/// </summary>

public class SceneEventsManager : MonoBehaviour
{
    //OVERALL
    private int eventCount;

    //Scene "Car"
    private GameObject car;
    public float spinSpeed;
    public bool carIsSpinning = false;

    //Scene "Stairs"

    //Scene "Layers"
    private Animator heartAnimator;
    private GameObject moon;
    private GameObject starOne;
    private GameObject starTwo;
    private GameObject starThree;
    private GameObject starFour;
    private GameObject starFive;
    private GameObject starSix;
    private GameObject starSeven;
    private GameObject starEight;
    private GameObject starNine;
    private GameObject starTen;
    private GameObject candles;
    private GameObject torchOne;
    private GameObject torchTwo;
    private GameObject spotLight;
    private GameObject throneTorch;

    //Scene "Fountain"
    private Animator carouselAnimator;
    private int pressCount;

    //Scene "Crocodile"
    private Animator mossAnimator;

    //Scene "Kaleidoscope"
    private Animator kaleidoscopeAnimator;

    void Start()
    {
        eventCount=0;
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        if (sceneName == "Car")
        {
            car = GameObject.Find("Parkrose_Car");
            //eventCount = 0; I don't think we need the event count reset on scene name because start method should run each scene when the scene object is initialized
        }
        if (sceneName == "Stairs")
        {
        }
        if (sceneName == "Layers")
        {
            heartAnimator = GameObject.Find("Heart Castle").GetComponent<Animator>();
            spotLight = GameObject.Find("SpotLight");
            moon = GameObject.Find("Moon");
            starOne = GameObject.Find("StarOne");
            starTwo = GameObject.Find("StarTwo");
            starThree = GameObject.Find("StarThree");
            starFour = GameObject.Find("StarFour");
            starFive = GameObject.Find("StarFive");
            starSix = GameObject.Find("StarSix");
            starSeven = GameObject.Find("StarSeven");
            starEight = GameObject.Find("StarEight");
            starNine = GameObject.Find("StarNine");
            starTen = GameObject.Find("StarTen");
            candles = GameObject.Find("Candles");
            torchOne = GameObject.Find("TorchOne");
            torchTwo = GameObject.Find("TorchTwo");
            throneTorch = GameObject.Find("ThroneTorch");

            spotLight.SetActive(false);
            moon.SetActive(false);  
            starOne.SetActive(false);
            starTwo.SetActive(false);
            starThree.SetActive(false);
            starFour.SetActive(false);
            starFive.SetActive(false);
            starSix.SetActive(false);
            starSeven.SetActive(false);
            starEight.SetActive(false);
            starNine.SetActive(false);
            starTen.SetActive(false);
            candles.SetActive(false);
            torchOne.SetActive(false);
            torchTwo.SetActive(false);
            throneTorch.SetActive(false);
        }
        if (sceneName == "Fountain")
        {
            pressCount = 0;
            carouselAnimator = GameObject.Find("Carousel Pivot").GetComponent<Animator>();
        }
        if (sceneName == "Playground")
        {
        }
        if (sceneName == "Crocodile")
        {
            mossAnimator = GameObject.Find("PollyPile").GetComponent<Animator>();
        }
        if (sceneName == "Kaleidoscope")
        {
            kaleidoscopeAnimator = GameObject.Find("Kaleidoscope").GetComponent<Animator>();
        }
    }

    
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (Input.GetKeyDown(KeyCode.H))
        {
            eventCount++;
        }
        
        if (sceneName == "Car")
        {
            if (eventCount == 1)
            {
                carIsSpinning = true;
                if (carIsSpinning)
                {
                    car.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
                }
            }
        }
        
        if (sceneName == "Stairs")
        {
        }
        
        if (sceneName == "Layers")
        {
            if (eventCount == 1)
            {
                heartAnimator.SetBool("isOpening", true);
            }

            if (eventCount == 2)
            {
                StartCoroutine(LightCastle());
            }
        }
        
        if (sceneName == "Fountain")
        {
            if (eventCount == 1)
            {
                StartCoroutine(CarouselSwitcher());
            }
        }

        if (sceneName == "Playground")
        {
        }
        
        if (sceneName == "Crocodile")
        {
            if (eventCount == 1)
            {
                mossAnimator.SetBool("MossGrowing", true);
            }
        }
        
        if (sceneName == "Kaleidoscope")
        {
            if (eventCount == 1)
            {
                kaleidoscopeAnimator.SetBool("FadeIn", true);
            }
        }
    }
    #region COROUTINES
    IEnumerator LightCastle()
    {
        while (true)
        {
            starOne.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            starTwo.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            starThree.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            starFour.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            starFive.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            starSix.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            starSeven.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            starEight.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            starNine.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            starTen.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            moon.SetActive(true);
            yield return new WaitForSeconds(1);
            torchOne.SetActive(true);
            torchTwo.SetActive(true);
            yield return new WaitForSeconds(1.2f);
            candles.SetActive(true);
            spotLight.SetActive(true);
            throneTorch.SetActive(true);
            yield return null;
        }
    }

    IEnumerator CarouselSwitcher()
    {
        while (true)
        {
            carouselAnimator.SetInteger("switchCount", pressCount);
            yield return new WaitForSeconds(5);
            pressCount = 1;
            carouselAnimator.SetInteger("switchCount", pressCount);
            yield return new WaitForSeconds(5);
            pressCount = 2;
            carouselAnimator.SetInteger("switchCount", pressCount);
            yield return new WaitForSeconds(5);
            pressCount = 3;
            carouselAnimator.SetInteger("switchCount", pressCount);
            yield return new WaitForSeconds(5);
            pressCount = 4;
            carouselAnimator.SetInteger("switchCount", pressCount);
            yield return new WaitForSeconds(5);
            pressCount = 5;
            carouselAnimator.SetInteger("switchCount", pressCount);
            yield return new WaitForSeconds(5);
            pressCount = 6;
            carouselAnimator.SetInteger("switchCount", pressCount);
            yield return new WaitForSeconds(5);
            pressCount = 7;
            carouselAnimator.SetInteger("switchCount", pressCount);
            yield return new WaitForSeconds(5);
            pressCount = 8;
            carouselAnimator.SetInteger("switchCount", pressCount);
            yield return new WaitForSeconds(5);   
        }
    }

    #endregion

}

