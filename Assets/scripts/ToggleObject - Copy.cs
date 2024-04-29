using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject model;

    public void Start()
    {
        model.gameObject.SetActive(false);
    }

    public void toggleActiveState(bool newState)
    {
        model.SetActive(newState);
    }
}
