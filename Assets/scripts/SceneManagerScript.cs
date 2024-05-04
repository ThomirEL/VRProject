using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public ClosedBookStart book; 

    public void loadScene(string sceneName){
        // Here you set the AlreadyOpened property
        GlobalBookSettings.AlreadyOpened = true;
        SceneManager.LoadScene(sceneName);
    }
}
