using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour {

    // Use this for initialization
    public void exitButton()
    {
        Application.Quit();
        Debug.Log("Exit game");
    }
}
