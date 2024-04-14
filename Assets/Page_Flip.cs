using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Hands.Samples.Gestures.DebugTools;

public class Page_Flip : MonoBehaviour
{
    public GameObject LeftPage;
    public GameObject RightPage;
    public GameObject LeftPagePrefab;
    public GameObject RightPagePrefab;
    private Vector3 LeftPagePosition;
    private Vector3 RightPagePosition;
    private GameObject LeftPageClone;
    private GameObject RightPageClone;
    // Start is called before the first frame update
    void Start()
    {
        LeftPagePosition = LeftPage.transform.position;
        RightPagePosition = RightPage.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (LeftPage.GetComponent<XRLever>().Mid_Flip)
        {
            LeftPageClone = InstantiateLeftPage();
        }
        else {
            Destroy(LeftPageClone);
        }
        if (RightPage.GetComponent<XRLever>().Mid_Flip)
        {
            RightPageClone = InstantiateRightPage();
        }
        else {
            Destroy(RightPageClone);
        }
    }

    public GameObject InstantiateLeftPage()
    {
        GameObject LeftPageClone = Instantiate(LeftPagePrefab, LeftPagePosition, Quaternion.identity);
        LeftPageClone.transform.SetParent(LeftPage.transform.parent);
        LeftPageClone.transform.localScale = LeftPage.transform.localScale;
        LeftPageClone.transform.localRotation = LeftPage.transform.localRotation;
        LeftPageClone.transform.localPosition = LeftPagePosition;
        LeftPage = LeftPageClone;
        return LeftPageClone;
    }

    public GameObject InstantiateRightPage()
    {
        GameObject RightPageClone = Instantiate(RightPagePrefab, RightPagePosition, Quaternion.identity);
        RightPageClone.transform.SetParent(RightPage.transform.parent);
        RightPageClone.transform.localScale = RightPage.transform.localScale;
        RightPageClone.transform.localRotation = RightPage.transform.localRotation;
        RightPageClone.transform.localPosition = RightPagePosition;
        RightPage = RightPageClone;
        return RightPageClone;
    }
}
