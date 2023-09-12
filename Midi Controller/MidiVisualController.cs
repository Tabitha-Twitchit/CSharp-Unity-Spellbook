using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Assets.Pixelation.Scripts;
using UnityStandardAssets.ImageEffects;
using QFSW.RetroFXUltimate;

/// 
/// An integration of EasyController's midi package with various Post-Processing, Camera Effects and controls, 
///video player controls, as well as other third party image effects systems for use in live video mixing.
/// 
/// If installing in a new project will require the following packages: 
/// Unity Post-Processing
/// YarnSpinner (if using dialog: https://docs.yarnspinner.dev/using-yarnspinner-with-unity/installation-and-setup), 
/// Retro FX Ultimate by QSFW (https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/retro-fx-ultimate-118259)
/// "Realistic Glitch Lite" by SF-Productions (https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/realistic-glitches-lite-107974)
/// 
/// If Installing in project post Unity 2020.3.7f1 you can delete the "Fullscreen" Package from imported assets and use Unity's built in solution
/// 
/// Spreadsheet of input mapping lives here: https://docs.google.com/spreadsheets/d/1T3OhrCCSI0Ow3CveV9jwtQYvMtBpD8ClaSXbvxwuFKA/edit?usp=sharing
///

public class MidiVisualController : MonoBehaviour
{
    [Header("Global Shit")]
    private int layer = 0;
    private int startValue = 63;

    [Header("Fog Settings")]
    private ParticleSystem groundFog;
    public bool fogOn = false;
    public Image transitionCard;
    private float cardAlpha;

    [Header("Camera Settings")]
    public Camera cam;
    private float fov;
    public bool camLockOn = false;
    public bool isSpinning = false;
    public float rotateSpeed = 3f;
    private CharacterController characterController;
    private FPController firstPersonController;
    private FlyingController flyingController;
    private FlyingControllerLocked lockedController;
    private float orthoSize;
    private bool camOrtho;
    private bool screenClearOff;
    private Blur blur;
    private int blurIntensity;

    [Header("Post-Processing Settings")]
    //NOTE All these effects require Post Processing Dependencies
    public PostProcessVolume volume;
    private ColorGrading colorGradingLayer = null;
    private ChromaticAberration chromaticAbLayer = null;
    private Bloom bloomLayer = null;
    private AmbientOcclusion ambientOcclusionLayer = null;
    private LensDistortion lensDistortionLayer = null;
    private Grain grainLayer = null;
    private Vignette vignetteLayer = null;
    private float temperature;
    private float hueShift;
    private float saturation;
    private float contrast;
    //private float chromaticAbIn;
    //private float chromaticAbOut;
    private float chromaticAb;
    private float bloom;
    private float ambientOc;
    private float lenseDistortion;
    private float lenseDistortionScale;
    public bool distortionScaleLockOn;
    private float grainIntensity;
    private float grainSize;
    private float vignetteIntensity;

    [Header("Glitch Settings")]
    //NOTE All these effects require various custom dependencies: https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/realistic-glitches-lite-107974
    private DeepFryer deepFryer;
    private float deepFryerIntensity;
    private ShaderEffect_BleedingColors bleeder;
    private float bleedIntensity;
    private float bleedShift;
    private ShaderEffect_CorruptedVram corrupted;
    private float corruptionShift;
    private ShaderEffect_CRT scanlines;
    private float scanlineIntensity;
    private int scanlineThickness;
    private ShaderEffect_Tint tinter;
    private float tintIntensity;
    private Pixelation pixelator;
    private float pixelIntensity;
    public bool pixelLockOn;

    [Header("Retro FX Settings")]
    //NOTE Requires RetroFX Asset: https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/retro-fx-ultimate-118259
    public bool retroLockOn;
    private RetroFX retro;
    private int retroResolution;

    [Header("Video Layer Settings")]
    private float alphaOne;
    private float alphaTwo;
    private RawImage vidOne;
    private RawImage vidTwo;
    private VideoPlayer vidClipOne;
    private VideoPlayer vidClipTwo;
    private int vidOnePlayCounter = 0;
    private int vidTwoPlayCounter = 0;

    void Start()
    {
        //initializes all channels at a default value to avoid them waking up at -1
        EasyController.esconsend.Send_data(1, startValue);
        EasyController.esconsend.Send_data(2, startValue);
        EasyController.esconsend.Send_data(3, startValue);
        EasyController.esconsend.Send_data(4, startValue);
        EasyController.esconsend.Send_data(5, startValue);
        EasyController.esconsend.Send_data(6, startValue);
        EasyController.esconsend.Send_data(7, startValue);
        EasyController.esconsend.Send_data(8, startValue);

        //fog stuff
        groundFog = GetComponentInChildren<ParticleSystem>();

        //Camera Stuff
        characterController = GetComponent<CharacterController>();
        firstPersonController = GetComponent<FPController>();
        flyingController = GetComponent<FlyingController>();
        lockedController = GetComponent<FlyingControllerLocked>();
        blur = GetComponentInChildren<Blur>();

        //post-processing stuff
        volume.profile.TryGetSettings(out colorGradingLayer);
        volume.profile.TryGetSettings(out chromaticAbLayer);
        volume.profile.TryGetSettings(out bloomLayer);
        volume.profile.TryGetSettings(out ambientOcclusionLayer);
        volume.profile.TryGetSettings(out lensDistortionLayer);
        volume.profile.TryGetSettings(out grainLayer);
        volume.profile.TryGetSettings(out vignetteLayer);

        //glitch stuff
        deepFryer = GetComponentInChildren<DeepFryer>();
        bleeder = GetComponentInChildren<ShaderEffect_BleedingColors>();
        corrupted = GetComponentInChildren<ShaderEffect_CorruptedVram>();
        scanlines = GetComponentInChildren<ShaderEffect_CRT>();
        tinter = GetComponentInChildren<ShaderEffect_Tint>();
        pixelator = GetComponentInChildren<Pixelation>();

        //Retro FX Stuff
        retro = GetComponentInChildren<RetroFX>();  

        //Video Layer Stuff
        vidOne = GameObject.Find("Video1").GetComponent<RawImage>();
        vidTwo = GameObject.Find("Video2").GetComponent<RawImage>();
        vidClipOne = GameObject.Find("Video Player 1").GetComponent<VideoPlayer>();
        vidClipTwo = GameObject.Find("Video Player 2").GetComponent<VideoPlayer>();
        vidOne.enabled = false;
        vidTwo.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        KeyInputs();
        Layerchange();

        if (isSpinning)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }

        if (layer == 0)
        {
            /*Nothing goes here, this layer is essentially a clutch to reset values.*/

            //Shows channel levels while in clutch for reset
            Debug.Log("Channel 1: " + EasyController.escon.get_state(1));
            Debug.Log("Channel 2: " + EasyController.escon.get_state(2));
            Debug.Log("Channel 3: " + EasyController.escon.get_state(3));
            Debug.Log("Channel 4: " + EasyController.escon.get_state(4));
            Debug.Log("Channel 5: " + EasyController.escon.get_state(5));
            Debug.Log("Channel 6: " + EasyController.escon.get_state(6));
            Debug.Log("Channel 7: " + EasyController.escon.get_state(7));
            Debug.Log("Channel 8: " + EasyController.escon.get_state(8));

        }

        if (layer == 1)
        {
            //controls fog particle effect
            var emission = groundFog.emission;
            emission.rateOverTime = EasyController.escon.get_state(1);

            //controls fade-to-grey transition card. Alpha gets divided by 100 because alpha is between 0 and 1, so dividing
            //by 100 gives you a gradient on the knob v on/off.
            cardAlpha = EasyController.escon.get_state(2);
            transitionCard.color = new Color(transitionCard.color.r, transitionCard.color.g, transitionCard.color.b, cardAlpha / 100);
        }

        if (layer == 2)
        {
            /*noting that what causes snapping is that as soon as the layer is set
            * the state is gotten, and so this may be a space to just re-assert 
            * the old or desired neutral value*/

            //CHANNEL 1 dedicated to FOV/Ortho rect size contingent on a boolean handled in KeyInputs()
            if (camOrtho)
            {
                //orthoSize +1 because 0 orthoSize throws an error. May need more math tuning depending on ideal ranges
                //of orthoSize
                orthoSize = EasyController.escon.get_state(1);
                cam.orthographicSize = orthoSize + 1;
            }
            else
            {
                //Weird math to try to give as full a range between 0 and 180 degrees as possible
                fov = EasyController.escon.get_state(1) ;
                cam.fieldOfView = (fov *= 1.7f) - 47;
            }

            //CHANNEL 2 lense distortion intensity
            lenseDistortion = EasyController.escon.get_state(2) - 63;
            lensDistortionLayer.intensity.value = lenseDistortion *= 1.7f;
            Debug.Log("Lens Distortion: " + lensDistortionLayer.intensity.value);

            //CHANNEL 3 lens distortion scale uses a lock because it causes erratic behavior with virtually any
            //lense distortion intensity, but it does make a nice psychedellic puddle effect when used intentionally.
            if (distortionScaleLockOn == false)
            {
                lenseDistortionScale = EasyController.escon.get_state(3) - 62;
                lensDistortionLayer.scale.value = lenseDistortionScale / 10;
                Debug.Log("Distortion Scale: " + lensDistortionLayer.scale.value);
            }
            //crucially this resets the value when the lock is turned back off to prevent it getting stuck in weird values
            else 
            { 
                lensDistortionLayer.scale.value = 1;
            }

            //CHANNEL 4 Grain Intensity. Divided by the full 63 because although you can push values higher than 1,
            //it doesn stick after a layer change. This could be adjusted up with a larger midi controller
            grainIntensity = EasyController.escon.get_state(4) - 63;
            grainLayer.intensity.value = grainIntensity / 63;
            Debug.Log("Grain Intensity: " + grainLayer.intensity.value);

            //CHANNEL 5 Grain Size. Similar to intensity, values don't carry over beyond their max.
            grainSize = EasyController.escon.get_state(5) - 62;
            grainLayer.size.value = grainSize / 21;
            Debug.Log("Grain Size: " + grainLayer.size.value);

            //CHANNEL 6 Vignette Intensity
            vignetteIntensity = EasyController.escon.get_state(6) - 63.5f;
            vignetteLayer.intensity.value = vignetteIntensity / 20;
            Debug.Log("Vignette Intensity: " + vignetteLayer.intensity.value);

            //CHANNEL 7 Blur Intensity. NOTE This uses an old ass "Image Effects" dependency from standard assets and so is
            //liable to break or if other Standard Assets are brought in may conflict with newer fixes (e .g. Bloom in PP stack)
            blurIntensity = EasyController.escon.get_state(7) -63;
            //blur has no value at which it does not apply some effect, ergo negative values used as an off switch
            if(blurIntensity <=0)
            {
                blur.enabled = false;
            }
            //divisor used to maintain blur at reasonable levels, esp given the slowness of the function.
            else 
            {
                blur.enabled = true;
                blur.iterations = blurIntensity / 5;
            }
            

            /*  Notes from MidiSwitcher Script
                *   if(layer == 1)
            {
                //the double duty of knob 8 and 16 doesn't work here. Not sure if it's because I'm 
                //not giving the correct index number, or because Unity knows I only have 8 hardware
                //knobs, despite it being double mapped in the easycontroller mapper. hrmmmm
                redValue = EasyController.escon.get_state(6);
                rend.material.color = new Color(redValue, greenValue, blueValue, 1);
                this.transform.Rotate(speed, 0, 0);

                /*pseudocode but it may need somewhere something like,
                    * if layer == 2 {Easycontroller(6)=Easycontroller(14)} etc*/
        }

        if (layer == 3)
        {
            //post-processing goes here. NOTE, when I dip into this layer from clutch without twiddling nobs that's when
            //it seems like values start off, but if I reset everything in clutch they pull in normal. Not sure whether
            //to try to initalize them in start with something or just twiddle in clutch. Need to test

            /*CHANNEL 1 temp Updates the temp variable with the knob value -63 so that we can get negative values to
            achieve cold temps. This is then multiplied by 1.7 somewhat arbitrarily to get a fuller range of 
            temps on a scale of -100 to 100*/
            temperature = EasyController.escon.get_state(1) - 63;
            colorGradingLayer.temperature.value = temperature *= 1.7f;
            Debug.Log("Temperature: " + colorGradingLayer.temperature.value);

            //CHANNEL 2 hue shift
            hueShift = EasyController.escon.get_state(2) - 63;
            colorGradingLayer.hueShift.value = hueShift * 3;
            Debug.Log("Hue Shift: " + colorGradingLayer.hueShift.value);

            //CHANNEL 3 saturation
            saturation = EasyController.escon.get_state(3) - 63;
            colorGradingLayer.saturation.value = saturation *= 3f;
            Debug.Log("Saturation: " + colorGradingLayer.saturation.value);

            //CHANNEL 4 contrast. Note despite contrast being a -100 to 100 scale I used a higher multiplier, allowing you to both
            //totally blow out an image, muddy, or effectively invert colors by going below the muddy range.
            contrast = EasyController.escon.get_state(4) - 63;
            colorGradingLayer.contrast.value = contrast *= 4;
            Debug.Log("Contrast: " + colorGradingLayer.contrast.value);

            //CHANNEL 5 Brightness -- using Luma. This is a very slow effect that overblows quite easily. If possible find another
            //long term solution
            tintIntensity = EasyController.escon.get_state(5) - 62;
            //Allows tint to hit higher ranges after a certain point
            if(tintIntensity >55)
            {
                tinter.y = tintIntensity * 1.5f;
            }
            //if tinter.y goes below 1 the screen blacks out, this statement prevents that
            else
            if(tintIntensity <1)
            {
                tinter.y = 1;
            }
            //this allows for normal function at all other values
            else 
            {
                tinter.y = tintIntensity;
            }
            Debug.Log("Tint Intensity:" + tinter.y);

            //CHANNEL 6 Bloom Intensity. Check soft knee, threshold, diffusion for other aspects.
            //NOTE: Bloom really softens/kills a lot of feedback effect. Also in inverted contrast mode becomes a
            //black cloud, almost link ambient occlusion which is p cool. 
            bloom = EasyController.escon.get_state(6) - 63;
            bloomLayer.intensity.value = bloom;
            Debug.Log("Bloom: " + bloomLayer.intensity.value);

            //CHANNEL 7 ambient occlusion. Compliments high contrast. has a weird falloff for what it considers a shadow
            ambientOc = EasyController.escon.get_state(7) - 63;
            ambientOcclusionLayer.intensity.value = ambientOc /= 10;
            Debug.Log("Ambient Occlusion: " + ambientOcclusionLayer.intensity.value);

            //CHANNEL 8 chromatic aberation. Values above 1 yield a kind of tunnel vision effect. Current peak using "/4" is 16 with stops at
            //.25 increments.
            chromaticAb = EasyController.escon.get_state(8) - 63;
            chromaticAbLayer.intensity.value = chromaticAb / 4;
            Debug.Log("Chromatic Aberation: " + chromaticAbLayer.intensity.value);

            /*
            Alternative method that should work on a curve but doesn't seem to respond to the higher value change
            
            chromaticAbIn = EasyController.escon.get_state(5) - 63;
            if(chromaticAbIn >0 && chromaticAbIn < 75)
                {
                    chromaticAbOut = Mathf.Pow(chromaticAbIn, 2)/Mathf.Pow(75, 2);                    
                }
            else if (chromaticAbIn > 75)
                {
                    chromaticAbOut = chromaticAbIn/2 ;
                }
            chromaticAbLayer.intensity.value = chromaticAbOut;
            Debug.Log("Chromatic Aberation: " + chromaticAbLayer.intensity.value);
            */
        }

        if (layer == 4)
        {
            //CHANNEL 1. Deep Fryer. BW effect that gets wild at any value besides 1. Seems to Intensify colors to max and ignore
            //lightness or darkness. Neg values invert crazy colors.
            deepFryerIntensity = EasyController.escon.get_state(1) - 63;
            deepFryer.intensity = deepFryerIntensity * 1.5f;
            Debug.Log("Fry Temp: " + deepFryer.intensity);

            //CHANNEL 2. Controls Intensity of the colors to be separated out
            bleedIntensity = EasyController.escon.get_state(2) - 63;
            bleeder.intensity = bleedIntensity;
            Debug.Log("Bleeder Intensity: " + bleeder.intensity);

            //CHANNEL 3. Controls the distance from which the colors are offset
            bleedShift = EasyController.escon.get_state(3) - 63;
            bleeder.shift = bleedShift * 1.5f;
            Debug.Log("Bleeder Shift: " + bleeder.shift);

            //CHANNEL 4. Distance of corrupted vram shift. Weirdly the script and shader this connects with offers
            //no clear neutral value. So the if/else performs a switching function.
            corruptionShift = EasyController.escon.get_state(4) - 63;
            Debug.Log("base corruption shift"+corruptionShift);
            if (corruptionShift > 2)
            {
                corrupted.enabled = true;
                corrupted.shift = corruptionShift;
                Debug.Log("Corruption Shift: " + corrupted.shift);
            }
            else
            if (corruptionShift < 2)
            {
                corrupted.enabled = false;
            }

            //CHANNEL 5 Scanline Intensity. Very slow to take effect and clogs the queue--you have to 
            //wait for it to reach its target value before new updates from other effects are drawn
            scanlineIntensity = EasyController.escon.get_state(5) - 63;
            scanlines.scanlineIntensity = scanlineIntensity * 1.5f;
            Debug.Log("Scanline Intensity: " + scanlines.scanlineIntensity);

            //CHANNEL 6 Scanline Width. 
            scanlineThickness = EasyController.escon.get_state(6) - 63;
            scanlines.scanlineWidth = scanlineThickness * 2;
            Debug.Log("Scanline Width: " + scanlines.scanlineWidth);

            //CHANNEL 7 Pixelation
            pixelIntensity = EasyController.escon.get_state(7);
            if(pixelLockOn == false)
            {
                pixelator.BlockCount = pixelIntensity;
            }

            //CHANNEL 8 RetroFX
            retroResolution = EasyController.escon.get_state(8);
            retro.ReferenceHeight = retroResolution * 2;
            Debug.Log("RetroFX Resolution:" + retro.ReferenceHeight);   
        }

        if (layer == 9)
        {
            alphaOne = EasyController.escon.get_state(1) - 63f;
            vidOne.material.color = new Color(1, 1, 1, alphaOne *= 0.02f);
            Debug.Log("Current Alpha Ch 1 = " + alphaOne);

            alphaTwo = EasyController.escon.get_state(2) - 63f;
            vidTwo.material.color = new Color(1, 1, 1, alphaTwo *= 0.02f);
            Debug.Log("Current Alpha Ch 2 = " + alphaTwo);
        }

    }
    //END UPDATE

    #region Custom FUnctions

    void ChangeControl()
    {
        if (characterController.enabled && firstPersonController.enabled)
        {
            characterController.enabled = false;
            firstPersonController.enabled = false;

            flyingController.enabled = true;
        }
        else if (flyingController.enabled)
        {
            flyingController.enabled = false;

            characterController.enabled = true;
            firstPersonController.enabled = true;
        }
    }

    void KeyInputs()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeControl();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            pixelLockOn = !pixelLockOn;
            if (pixelLockOn)
            {
                pixelator.enabled = false;
            }
            else 
            {
                pixelator.enabled = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            retroLockOn = !retroLockOn;
            if(retroLockOn)
            {
                retro.enabled = false;
            }
            else
            { 
                retro.enabled = true; 
            }
        }

        //If using this with a separate FP controller, remember to check the inputs against that so you don't double up.
        if (Input.GetKeyDown(KeyCode.J))
        {
            vidOnePlayCounter++;
            {
                if (vidOnePlayCounter == 1)
                {
                    vidOne.enabled = true;
                    vidClipOne.Play();
                }
                if (vidOnePlayCounter >= 2)
                {
                    vidOne.enabled = false;
                    vidClipOne.Pause();
                    vidOnePlayCounter = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            fogOn = !fogOn;
            if (fogOn)
            {
                groundFog.Play();
            }
            else
            {
                groundFog.Stop();
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            camLockOn = !camLockOn;
            if (camLockOn)
            {
                characterController.enabled = false;
                firstPersonController.enabled = false;
                flyingController.enabled = false;
                lockedController.enabled = true;

            }
            else
            {
                characterController.enabled = false;
                firstPersonController.enabled = false;
                flyingController.enabled = true;
                lockedController.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            distortionScaleLockOn = !distortionScaleLockOn;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            screenClearOff = !screenClearOff;
            if (screenClearOff)
            {
                cam.clearFlags = CameraClearFlags.Nothing;
            }
            else
            {
                cam.clearFlags = CameraClearFlags.Skybox;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            camOrtho = !camOrtho;
            if (camOrtho)
            {
                cam.orthographic = true;
            }
            else
            {
                cam.orthographic = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isSpinning = !isSpinning;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            vidTwoPlayCounter++;
            {
                if (vidTwoPlayCounter == 1)
                {
                    vidTwo.enabled = true;
                    vidClipTwo.Play();
                }
                if (vidTwoPlayCounter >= 2)
                {
                    vidTwo.enabled = false;
                    vidClipTwo.Pause();
                    vidTwoPlayCounter = 0;
                }
            }
        }

    }

    void Layerchange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            layer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            layer = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            layer = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            layer = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            layer = 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            layer = 5;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            layer = 6;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            layer = 7;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            layer = 8;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            layer = 9;
        }
        Debug.Log("Current Layer = " + layer);
    }
    #endregion
}