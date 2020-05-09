using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {

    public GameObject nucleus;
    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        orbitAroundNucleus();
	}

    void orbitAroundNucleus()
    {
        transform.RotateAround(nucleus.transform.position, Vector3.right, speed * Time.deltaTime);
    }
}
