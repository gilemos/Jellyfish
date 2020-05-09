using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneMenu : MonoBehaviour {

    public void goToGame()
    {
        SceneManager.LoadScene("ARScene");
    }

    public void goToGoals()
    {
        SceneManager.LoadScene("GoalsScene");
    }

    public void goToStartScreen()
    {
        SceneManager.LoadScene("StartScene");
    }
}
