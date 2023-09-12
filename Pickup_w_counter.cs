using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pickup_w_counter : SingletonDontDestroy<Pickup_w_counter>
{
    //Above: NOTE the script inherits from a special singleton class used to preserve objects between scenes, so that class must be
    //present for it to work. Note that class inherits from monobehavior so it will share that functionality as well.
    //Below: the canvas/UI element th script uses / prints to, and the actual value tracked. 

    public TextMeshProUGUI countText;
    private int count;
  
    void Start()
    {
        count = 0;
        SetCountText();
    }

    //if the player hits another collider that is setup as a trigger it checks if the tag says "packet" and if so, deactivates
    //it and adds one to the counter
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("packet"))
        {
            other.gameObject.SetActive(false);
            count = count + 100;
            SetCountText();
        }

        if (other.gameObject.CompareTag("kilopacket"))
        {
            other.gameObject.SetActive(false);
            count = count + 1000;
            SetCountText();
        }
    }

    //this defines the base text shown in the UI and appends it with the value set above
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }
}
