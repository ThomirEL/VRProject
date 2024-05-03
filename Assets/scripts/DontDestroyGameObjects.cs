using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class DontDestroyGameObjects : MonoBehaviour
{
    private GameObject[] objs;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        // Find all gameobjects
        objs = GameObject.FindGameObjectsWithTag("DontDestroy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwappedScene()
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(true);
        }
    }

    public void SetActiveObs(bool active)
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(active);
        }
    }
}
