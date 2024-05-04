using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Hands.Samples.Gestures.DebugTools;
using TMPro;
using Unity.XR.CoreUtils;
using System.Linq;

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

    private Quaternion LeftPageRotation;
    private Quaternion RightPageRotation;

    // The clones to be spawned when the paper is being flipped
    private GameObject LeftPageClone;
    private GameObject RightPageClone;

    private XRLever leverLeft;
    private XRLever leverRight;

    [SerializeField]
    private GameObject[] pageVisuals;

    private int pageIndexLeft = 0;

    private int pageIndexRight = 1;
    //Control whether a clone has spawned or not

    [Header("Fine Adjustment")]
    [Tooltip("The position of the spawned page when the right page is being flipped to the left (Used for fine adjustment)")]
    [SerializeField]
    private Vector3 rightFlipSpawnedPagePosition = new Vector3(0.0f, 0.0f, 0.0f);

    [Tooltip("The rotation of the spawned page when the right page is being flipped to the left (Used for fine adjustment)")]
    [SerializeField]
    private Vector3 rightFlipSpawnedPageRotation = new Vector3(0.0f, 0.0f, 0.0f);

    [Space]

    [Tooltip("The rotation of the spawned page when the left page is being flipped to the right (Used for fine adjustment)")]
    [SerializeField]
    private Vector3 leftFlipSpawnedPageRotation = new Vector3(0.0f, 0.0f, 0.0f);


    [Tooltip("The position of the spawned page when the left page is being flipped to the right (Used for fine adjustment)")]
    [SerializeField]
    private Vector3 leftFlipSpawnedPagePosition = new Vector3(0.0f, 0.0f, 0.0f);

    GameObject rightFlipPageSpawned;

    GameObject leftFlipPageSpawned;

    void Start()
    {
        LeftPagePosition = LeftPage.transform.position;
        RightPagePosition = RightPage.transform.position;
        LeftPageClone = null;
        RightPageClone = null;

        // Subscribe to the onLeverActivate event
        BookCoverLeft.GetComponent<XRLever>().onValueChanged.AddListener(ValueChangedLeftPage);
        BookCoverRight.GetComponent<XRLever>().onValueChanged.AddListener(ValueChangedRightPage);
        leverLeft = BookCoverLeft.GetComponent<XRLever>();
        leverRight = BookCoverRight.GetComponent<XRLever>();
    }

    private void OnDestroy()
    {
        // Remove listeners so there are no memory leaks
        BookCoverLeft.GetComponent<XRLever>().onValueChanged.RemoveListener(ValueChangedLeftPage);
        BookCoverRight.GetComponent<XRLever>().onValueChanged.RemoveListener(ValueChangedRightPage);
    }

   
    /// <summary>
    /// Called when the right hand stops grabbing the right page. Destroy the clones and update the page index
    /// </summary>
    public void OnEndGrabRight()
    {
        LeftPage.SetActive(true);
        if (RightPageClone != null)
        {
            Destroy(RightPageClone);
            RightPageClone = null;
        }

        if (rightFlipPageSpawned != null)
        {
            Destroy(rightFlipPageSpawned);
            rightFlipPageSpawned = null;
        }
        if (leverRight.leverState == LeverState.Off)
        {
            
            pageVisuals[pageIndexRight].SetActive(false);
            pageVisuals[pageIndexLeft].SetActive(false);
            if (pageIndexRight == pageVisuals.Length - 2)
            {
                leverRight.lastPage = true;
                pageVisuals[pageIndexRight + 1].SetActive(true);
                RightPage.SetActive(false);
                return;
            }
            if (leverLeft.lastPage == true) {
                LeftPage.SetActive(true);
                leverLeft.lastPage = false;
                pageVisuals[pageIndexRight].SetActive(true);
                pageVisuals[pageIndexLeft].SetActive(true);
                return;
            }
            pageIndexRight += 2;
            pageIndexLeft += 2;
            pageVisuals[pageIndexRight].SetActive(true);
            pageVisuals[pageIndexLeft].SetActive(true);
        }

    }


    /// <summary>
    /// Called when the left hand stops grabbing the left page. Destroy the clones and update the page index
    /// </summary>
    public void OnEndGrabLeft()
    {
        
        RightPage.SetActive(true);
        if (LeftPageClone != null) {
            Destroy(LeftPageClone);
            LeftPageClone = null;
        }

        if (leftFlipPageSpawned != null)
        {
            Destroy(leftFlipPageSpawned);
            leftFlipPageSpawned = null;
        }

        if (leverLeft.leverState == LeverState.Off){
                if (RightPageLastPage())
                    pageVisuals[pageVisuals.Length - 1].SetActive(false);
                pageVisuals[pageIndexRight].SetActive(false);
                pageVisuals[pageIndexLeft].SetActive(false);
                if (pageIndexLeft == 0) {
                    leverLeft.lastPage = true;
                    LeftPage.SetActive(false);
                    return;
                }
                if (leverRight.lastPage == true) {
                    RightPage.SetActive(true);
                    leverRight.lastPage = false;
                    pageVisuals[pageIndexRight].SetActive(true);
                    pageVisuals[pageIndexLeft].SetActive(true);
                    return;
                }
                pageIndexRight -= 2;
                pageIndexLeft -= 2;
                pageVisuals[pageIndexRight].SetActive(true);
                pageVisuals[pageIndexLeft].SetActive(true);
        }

    }


    /// <summary>
    /// Called when the left page is grabbed and changes state between on, off and flip. On = page is on the left, off = page is on the right and flip = page is in between
    /// </summary>
    /// <param name="state"></param>
     private void ValueChangedLeftPage(LeverState state)
    {
        // Get all children of left page
        GameObject[] children = pageVisuals[pageIndexLeft].GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        // Set all to inactive except text
        foreach (GameObject child in children)
        {
            print(child.name);
            if (child.name != "Text (TMP)")
            {
                child.SetActive(false);
            }
        }
        switch(state){
            case LeverState.On:
                if (LeftPageClone != null) {
                    Destroy(LeftPageClone);
                    LeftPageClone = null;
                }
                break;
            case LeverState.Off:
                RightPage?.SetActive(false);
                break;
            case LeverState.Flip:
                if (!RightPageLastPage())
                    RightPage?.SetActive(true);
                if (LeftPageClone == null && pageIndexLeft != 0){
                    LeftPageClone = InstantiateLeftPage(); // Spawn clone of the left page meanwhile the main page is being flipped
                    int indexForLeftClone = 0;
                    int indexForFlipPage = -1;
                    if (RightPage.activeSelf == true){
                        indexForLeftClone = 2;
                        indexForFlipPage = 1;
                    }
                    GameObject pageToBeShownOnClone = pageVisuals[pageIndexLeft - indexForLeftClone];
                    print(pageToBeShownOnClone.name);
                    GameObject pageClone = Instantiate(pageToBeShownOnClone, pageToBeShownOnClone.transform.position, pageToBeShownOnClone.transform.rotation); // Spawn the page text on the clone
                    pageClone.transform.parent = LeftPageClone.transform;
                    pageClone.transform.localScale = pageToBeShownOnClone.transform.localScale;
                    pageClone.SetActive(true);

                    if (leftFlipPageSpawned == null) {
                        GameObject pageToBeShownOnClone2 = pageVisuals[pageIndexLeft - indexForFlipPage];
                        GameObject currentLeftPage = pageVisuals[pageIndexLeft];
                        leftFlipPageSpawned = Instantiate(pageToBeShownOnClone2, currentLeftPage.transform.position, currentLeftPage.transform.rotation); // Spawn the page text on the clone
                        leftFlipPageSpawned.transform.position = currentLeftPage.transform.position + leftFlipSpawnedPagePosition;
                        leftFlipPageSpawned.transform.parent = LeftPage.transform;
                        leftFlipPageSpawned.transform.localScale = currentLeftPage.transform.localScale;
                        GameObject pageText = leftFlipPageSpawned.GetNamedChild("Text (TMP)");
                        pageText.transform.localScale = new Vector3(-1 * pageText.transform.localScale.x, pageText.transform.localScale.y, pageText.transform.localScale.z);
                        leftFlipPageSpawned.SetActive(true);
                    }
                }
                break;
        }
    }


    /// <summary>
    /// Called when the right page is grabbed and changes state between on, off and flip. On = page is on the right, off = page is on the left and flip = page is in between
    /// </summary>
    /// <param name="state"></param>
    private void ValueChangedRightPage(LeverState state)
    {
        // Get all children of right page
        GameObject[] children = pageVisuals[pageIndexRight].GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        // Set all to inactive except text
        foreach (GameObject child in children)
        {
            print("Child name: " + child.name);
            print(!child.name.Contains("Page"));
            if (child.name != "Text (TMP)" && !child.name.Contains("Page"))
            {
                child.SetActive(false);
            }
        }
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
                LeftPage?.SetActive(false);
                break;
            case LeverState.Flip:
                print("Right page is flipping");
                print("Right Page Index: " + pageIndexRight);
                print("Page Visuals Length: " + pageVisuals.Length);
                int indexForRightClone = 0;
                int indexForFlipPage = -1;
                if (LeftPage.activeSelf == true) {
                        indexForRightClone = 2;
                        indexForFlipPage = 1;
                }
                LeftPage?.SetActive(true);
                if (RightPageClone == null){
                    if (!RightPageLastPage()) {
                        RightPageClone = InstantiateRightPage(); // Spawn clone of the right page meanwhile the main page is being flipped
                        GameObject pageToBeShownOnClone = pageVisuals[pageIndexRight + indexForRightClone];
                        // If it already exists return
                        GameObject pageClone = Instantiate(pageToBeShownOnClone, pageToBeShownOnClone.transform.position, pageToBeShownOnClone.transform.rotation); // Spawn the page text on the clone
                        pageClone.transform.parent = RightPageClone.transform;
                        pageClone.transform.localScale = pageToBeShownOnClone.transform.localScale;
                        pageClone.SetActive(true);
                    }

                    if (rightFlipPageSpawned == null) {
                        print("trying to spawn right flip page");
                        GameObject pageToBeShownOnClone2 = pageVisuals[pageIndexRight + indexForFlipPage];
                        GameObject currentRightPage = pageVisuals[pageIndexRight];
                        rightFlipPageSpawned = Instantiate(pageToBeShownOnClone2, currentRightPage.transform.position, currentRightPage.transform.rotation); // Spawn the page text on the clone
                        rightFlipPageSpawned.transform.position = currentRightPage.transform.position + rightFlipSpawnedPagePosition;
                        rightFlipPageSpawned.transform.parent = RightPage.transform;
                        rightFlipPageSpawned.transform.localScale = currentRightPage.transform.localScale;
                        GameObject pageText = rightFlipPageSpawned.GetNamedChild("Text (TMP)");
                        pageText.transform.localScale = new Vector3(-1 * pageText.transform.localScale.x, pageText.transform.localScale.y, pageText.transform.localScale.z);
                        rightFlipPageSpawned.SetActive(true);
                    }
                }
                break;
        }
    }

    private bool RightPageLastPage()
    {
        return pageIndexRight == pageVisuals.Length - 2;
    }

    private bool LeftPageLastPage()
    {
        return pageIndexLeft == 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the position of the spawned pages while flipping (only used for fine adjustment)
        if (rightFlipPageSpawned != null)
        {
            rightFlipPageSpawned.transform.localPosition = RightPage.transform.position + rightFlipSpawnedPagePosition;
            rightFlipPageSpawned.transform.localRotation = Quaternion.Euler(rightFlipSpawnedPageRotation);
        }
        if (leftFlipPageSpawned != null)
        {
            leftFlipPageSpawned.transform.localPosition = LeftPage.transform.position + leftFlipSpawnedPagePosition;
            leftFlipPageSpawned.transform.localRotation = Quaternion.Euler(leftFlipSpawnedPageRotation);
        }
    }

    public void SetLeftPagePosAndRot()
    {
        LeftPagePosition = LeftPage.transform.position;
        LeftPageRotation = LeftPage.transform.localRotation;
    }

    public void SetRightPagePosAndRot()
    {
        RightPagePosition = RightPage.transform.position;
        RightPageRotation = RightPage.transform.localRotation;
    }

    public GameObject InstantiateLeftPage()
    {
        GameObject LeftPageClone = Instantiate(PaperPrefab, LeftPagePosition, Quaternion.identity);
        LeftPageClone.transform.SetParent(LeftPage.transform.parent);
        LeftPageClone.transform.localScale = LeftPage.transform.localScale;
        LeftPageClone.transform.localRotation = LeftPageRotation;
        LeftPageClone.transform.position = LeftPagePosition;
        return LeftPageClone;
    }

    public GameObject InstantiateRightPage()
    {
        print(RightPagePosition);
        print(RightPageRotation);
        GameObject RightPageClone = Instantiate(PaperPrefab, RightPagePosition, Quaternion.identity);
        RightPageClone.transform.SetParent(RightPage.transform.parent);
        RightPageClone.transform.localScale = RightPage.transform.localScale;
        RightPageClone.transform.localRotation = RightPageRotation;
        RightPageClone.transform.position = RightPagePosition;
        return RightPageClone;
    }
}
