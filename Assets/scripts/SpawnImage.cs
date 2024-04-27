using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnImage : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject picture;
    public void Start()
    {
       picture.gameObject.SetActive(false);
    }

    public void Switch()
    {
        if (picture.activeSelf == false)
        {
            picture.SetActive(true);
        } else {
            picture.SetActive(false);
        }
    } 
}
