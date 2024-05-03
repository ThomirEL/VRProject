using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    private GameObject[] objsSetToActive;
    private GameObject[] objsSetToInactive;
    public void loadScene(string sceneName){
        // if (sceneName == "360video") {
        //     GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        //     objsSetToActive = new GameObject[allObjects.Length];
        //     objsSetToInactive = new GameObject[allObjects.Length];
        //     int activeIndex = 0;
        //     int inactiveIndex = 0;
        //     foreach (GameObject obj in allObjects)
        //     {
        //         if (obj.activeSelf)
        //         {
        //             objsSetToActive[activeIndex] = obj;
        //             activeIndex++;
        //         }
        //         else
        //         {
        //             objsSetToInactive[inactiveIndex] = obj;
        //             inactiveIndex++;
        //         }
        //     }
        // }
        if (sceneName == "Hand Tracking"){
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        }
        else if (sceneName == "360video"){
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
    }
}
