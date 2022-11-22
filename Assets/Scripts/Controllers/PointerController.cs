using Microsoft.MixedReality.Toolkit.Input;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    private ApiController api;
    
    public LineRenderer lineRenderer;
    public PhotonView pv;
    public bool isPointerOn = true;

    private Ray rightHandRay;

    private Vector3 networkLocalPosition;
    private Quaternion networkLocalRotation;

    private Vector3 LaserPointerOrigin;
    private Vector3 LaserPointerDestination;

    private void Start()
    {
        api = ApiController.GetInstance();

        networkLocalPosition = transform.localPosition;
        networkLocalRotation = transform.localRotation;

        LaserPointerOrigin = new Vector3();
        LaserPointerDestination = new Vector3();

        DrawLine(LaserPointerOrigin, LaserPointerDestination);
    }

    private void Update()
    {
        if (!pv.IsMine)
        {
            transform.localPosition = networkLocalPosition;
            transform.localRotation = networkLocalRotation;
        }
        else
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
            //Debug.Log("Right HAND available?");
            if (InputRayUtils.TryGetHandRay(Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right, out rightHandRay))
            {
                //Debug.Log("Right HAND available: origin = " + rightHandRay.origin + " direction = " + rightHandRay.direction);
                LaserPointerOrigin = rightHandRay.origin;
                LaserPointerDestination = rightHandRay.origin + rightHandRay.direction;
            }
            else
            {
                LaserPointerOrigin = new Vector3();
                LaserPointerDestination = new Vector3();
            }
        }
        if (!isPointerOn)
        {
            LaserPointerOrigin = new Vector3();
            LaserPointerDestination = new Vector3();
        }
        DrawLine(LaserPointerOrigin, LaserPointerOrigin + (LaserPointerDestination - LaserPointerOrigin) * 10f);
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        if (lineRenderer != null)
        {
            lineRenderer.gameObject.transform.position = start;
            lineRenderer.startWidth = 0.005f;
            lineRenderer.endWidth = 0.005f;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

        }
    }
}
