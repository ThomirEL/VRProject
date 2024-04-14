using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Book : MonoBehaviour
{
    public GameObject bookCover;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = bookCover.transform.position;
    }
}
