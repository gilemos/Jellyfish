using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour {

    private bool canFade;
    private Color alphaColor;
    private float timeToFade = 1.0f;

    // Use this for initialization
    void Start () {
        canFade = true;
        alphaColor = GetComponent<MeshRenderer>().material.color;
        alphaColor.a = 0;

    }
	
	// Update is called once per frame
	void Update () {
        if (canFade)
        {
            GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color, alphaColor, timeToFade * Time.deltaTime);
        }
    }
}
