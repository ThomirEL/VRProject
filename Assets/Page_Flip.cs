using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Hands.Samples.Gestures.DebugTools;

public class Page_Flip : MonoBehaviour
{

    // Controls the left and right page (Current active)
    public GameObject LeftPage;
    public GameObject RightPage;

    // Prefab paper for instantiating
    public GameObject PaperPrefab;

    // GameObjects for Covers that control the paper mechanics
    public GameObject BookCoverLeft;
    public GameObject BookCoverRight;

    // Positions for the papers to spawn in the correct way
    private Vector3 LeftPagePosition;
    private Vector3 RightPagePosition;

    // The clones to be spawned when the paper is being flipped
    private GameObject LeftPageClone;
    private GameObject RightPageClone;

    //Control whether a clone has spawned or not
    private bool hasSpawnedRight;

    private bool hasSpawnedLeft;

    void Start()
    {
        LeftPagePosition = LeftPage.transform.position;
        RightPagePosition = RightPage.transform.position;
        LeftPageClone = null;
        RightPageClone = null;
        // Subscribe to the onLeverActivate event
        BookCoverLeft.GetComponent<XRLever>().onValueChanged.AddListener(ValueChangedLeftPage);
        BookCoverRight.GetComponent<XRLever>().onValueChanged.AddListener(ValueChangedRightPage);
        // BookCoverLeft.GetComponent<XRLever>().onLeverDeactivate.AddListener(HandleLeverDeactivateLeft);
        // BookCoverRight.GetComponent<XRLever>().onLeverDeactivate.AddListener(HandleLeverDeactivateRight);
    }

    private void OnDestroy()
    {
        // Remove listeners so there are no memory leaks
        BookCoverLeft.GetComponent<XRLever>().onValueChanged.RemoveListener(ValueChangedLeftPage);
        BookCoverRight.GetComponent<XRLever>().onValueChanged.RemoveListener(ValueChangedRightPage);
    }

    private void ValueChangedLeftPage(LeverState state)
    {
        switch (state)
        {
            case LeverState.On:
                if (LeftPageClone != null) {
                    Destroy(LeftPageClone);
                    LeftPageClone = null;
                    }
                break;
            case LeverState.Off:
                ChangePaperLeft();
                break;
            case LeverState.Flip:
                LeftPageClone = InstantiateLeftPage();
                break;
        }
    }

    private void ValueChangedRightPage(LeverState state)
    {
        print("Right page is in current state: " + state);
        switch (state)
        {
            case LeverState.On:
                if (RightPageClone != null) {
                    Destroy(RightPageClone);
                    RightPageClone = null;
                    }
                break;
            case LeverState.Off:
                ChangePaperRight();
                break;
            case LeverState.Flip:
                RightPageClone = InstantiateRightPage();
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (BookCoverLeft.GetComponent<XRLever>().Mid_Flip && !hasSpawnedLeft)
        // {
        //     hasSpawnedLeft = true;
        //     LeftPageClone = InstantiateLeftPage();
        // }
        // else if (!BookCoverLeft.GetComponent<XRLever>().Mid_Flip) {
        //     hasSpawnedLeft = false;
        //     Destroy(LeftPageClone);
        //     LeftPageClone = null;
        // }

        // if (BookCoverRight.GetComponent<XRLever>().value == LeverState.Flip && !hasSpawnedRight)
        // {
        //     hasSpawnedRight = true;
        //     RightPageClone = InstantiateRightPage();
        // }
        // else if (BookCoverRight.GetComponent<XRLever>().value == LeverState.On) {
        //     hasSpawnedRight = false;
        //     Destroy(RightPageClone);
        //     RightPageClone = null;
        // }
        // else if (BookCoverRight.GetComponent<XRLever>().value == LeverState.Off) {
        //     ChangePaperRight();
        // }
    }

    public void ChangePaperRight()
    {
        Destroy(LeftPage);
        LeftPage = RightPage;
        BookCoverLeft.GetComponent<XRLever>().handle = LeftPage.transform;
        RightPage = RightPageClone;
        BookCoverRight.GetComponent<XRLever>().handle = RightPage.transform;
        BookCoverRight.GetComponent<XRLever>().value = true;
        RightPageClone = null;
    }

    public void ChangePaperLeft()
    {
        Destroy(RightPage);
        RightPage = LeftPage;
        BookCoverRight.GetComponent<XRLever>().handle = RightPage.transform;
        LeftPage = LeftPageClone;
        BookCoverLeft.GetComponent<XRLever>().handle = LeftPage.transform;
        BookCoverLeft.GetComponent<XRLever>().value = true;
        LeftPageClone = null;
    }

    public GameObject InstantiateLeftPage()
    {
        GameObject LeftPageClone = Instantiate(PaperPrefab, LeftPagePosition, Quaternion.identity);
        LeftPageClone.transform.SetParent(LeftPage.transform.parent);
        LeftPageClone.transform.localScale = LeftPage.transform.localScale;
        LeftPageClone.transform.localRotation = LeftPage.transform.localRotation;
        LeftPageClone.transform.position = LeftPagePosition;
        return LeftPageClone;
    }

    public GameObject InstantiateRightPage()
    {
        GameObject RightPageClone = Instantiate(PaperPrefab, RightPagePosition, Quaternion.identity);
        RightPageClone.transform.SetParent(RightPage.transform.parent);
        RightPageClone.transform.localScale = RightPage.transform.localScale;
        RightPageClone.transform.localRotation = RightPage.transform.localRotation;
        RightPageClone.transform.position = RightPagePosition;
        return RightPageClone;
    }
}
