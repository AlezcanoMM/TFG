using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandPositionController : MonoBehaviour
{
    public Transform wrist;
    [SerializeField]
    private Transform wristRootBone;

    public bool isRightHand = true;

    private string hand;

    // Start is called before the first frame update
    void Start()
    {
        if (isRightHand)
        {
            hand = "Right_RiggedHandRight(Clone)/R_Hand_MRTK_Rig/R_Wrist";
        }
        else {
            hand = "Left_RiggedHandLeft(Clone)/L_Hand_MRTK_Rig/L_Wrist";
        }
    }

    private void Update()
    {
        //Set Right hand
        if (GameObject.Find(hand) && ApiController.GetInstance().pv.IsMine)
        {
            wristRootBone = GameObject.Find(hand).GetComponent<Transform>();
        }
        else 
        {
            wristRootBone = null;
            //photonHand.gameObject.transform.parent.gameObject.SetActive(false);
        }

        if (wristRootBone != null) {
            wrist.position = wristRootBone.position;
            wrist.rotation = wristRootBone.rotation;
        }
    }
}
