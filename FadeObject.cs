using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    /*Fades out an object over time. Note the material must allow transparency to fade via alpha value, 
     * e.g. Material with Unity Standard Shader set to either Transparent or Cutout Mode.
     */

    //private bool fadeOut, fadeIn;
    public float fadeSpeed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(FadeOutObject());
        }
    }
        /*        
                if(fadeOut)
                {
                    //this is a really key line that lets us put a pin in the current values we want to keep, while letting us change others
                    Color objectColor = this.GetComponent<Renderer>().material.color;
                    float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                    objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                    this.GetComponent<Renderer>().material.color = objectColor;

                    if(objectColor.a <= 0 ) 
                    {
                        fadeOut = false;
                    }
                }
            }

            public void FadeOutObject()
            {
                fadeOut = true;
            }

            public void FadeInObject()
            {
                fadeIn = true;
            }
        */
        public IEnumerator FadeOutObject()
        {
            while (this.GetComponent<Renderer>().material.color.a > 0)
            {
                //this is a really key line that lets us put a pin in the current values we want to keep, while letting us change others
                Color objectColor = this.GetComponent<Renderer>().material.color;
                float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                this.GetComponent<Renderer>().material.color = objectColor;
                yield return null;
            }
        }
}
