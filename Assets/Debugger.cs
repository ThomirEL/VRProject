using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Debugger : MonoBehaviour
{
    public XRSocketInteractor socket;
    public GameObject ClosedBook;
    public GameObject AdjustedBook;
    public OnSnapTransform onSnapTransform;
    // Start is called before the first frame update
    void Start()
    {
         socket.socketActive = false;
            ClosedBook.SetActive(false);
            AdjustedBook.SetActive(true);
            onSnapTransform.OpenBook();
        
    }

    // Update is called once per frame
    
}
