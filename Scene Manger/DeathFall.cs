﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathFall : MonoBehaviour {


	// Update is called once per frame
	void Update () {
        if (gameObject.transform.position.y <= -30)
        {
            SceneManager.LoadScene(0);
        }
    }
}