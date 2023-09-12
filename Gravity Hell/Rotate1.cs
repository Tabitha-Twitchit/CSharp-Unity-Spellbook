using UnityEngine;
using System.Collections;

public class Rotate1 : MonoBehaviour {
	public float speed = 30.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward,speed*Time.deltaTime);
	}
}
