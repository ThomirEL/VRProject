using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedBookStart : MonoBehaviour
{
    public GameObject closedBook;

    public Vector3 StartingPosition;
    // Start is called before the first frame update
    void Start()
    {
        closedBook.transform.position = StartingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
