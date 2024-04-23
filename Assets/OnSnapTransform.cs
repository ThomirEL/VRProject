using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class OnSnapTransform : MonoBehaviour
{
    [SerializeField]
    GameObject CoverToMoveLeft;

    [SerializeField]
    GameObject CoverToMoveRight;

    [SerializeField]
    Vector3 DesiredRotationLeft;

    [SerializeField]
    Vector3 DesiredPositionLeft;

    [SerializeField]
    Vector3 DesiredRotationRight;

    [SerializeField]
    Vector3 DesiredPositionRight;

    [SerializeField]
    GameObject ClosedBook;

    [SerializeField]
    private Page_Flip page_flip;
    public bool openBook = false;

    public Vector3 positonSnap;
    public Vector3 rotationSnap;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenBook()
    {
        openBook = true;
        // this.transform.position = ClosedBook.transform.position;
        // this.transform.rotation = ClosedBook.transform.rotation;
    }

    public void Print()
    {
        Debug.Log("Print");
    }

    // Update is called once per frame
    void Update()
    {
        if (openBook == true)
        {
            CoverToMoveLeft.transform.rotation = Quaternion.Slerp(CoverToMoveLeft.transform.rotation, Quaternion.Euler(DesiredRotationLeft), Time.deltaTime * 3);
            CoverToMoveLeft.transform.localPosition = Vector3.Lerp(CoverToMoveLeft.transform.localPosition, DesiredPositionLeft, Time.deltaTime * 3);
            CoverToMoveRight.transform.rotation = Quaternion.Slerp(CoverToMoveRight.transform.rotation, Quaternion.Euler(DesiredRotationRight), Time.deltaTime * 3);
            CoverToMoveRight.transform.localPosition = Vector3.Lerp(CoverToMoveRight.transform.localPosition, DesiredPositionRight, Time.deltaTime * 3);
        }

        //If done moving the cover
        if (CoverToMoveLeft.transform.rotation == Quaternion.Euler(DesiredRotationLeft) && CoverToMoveLeft.transform.localPosition == DesiredPositionLeft && openBook)
        {
            print("done moving left cover");
            //openBook = false;
            page_flip.SetLeftPagePosAndRot();
            page_flip.SetRightPagePosAndRot();
        }
    }

}
