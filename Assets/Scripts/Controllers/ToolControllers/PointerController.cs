using Microsoft.MixedReality.Toolkit.Input;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour, IPunObservable
{
    private AppController app;
    
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
        app = AppController.GetInstance();

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

            if (InputRayUtils.TryGetHandRay(Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right, out rightHandRay))
            {
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

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
            stream.SendNext(LaserPointerOrigin);
            stream.SendNext(LaserPointerDestination);
            stream.SendNext(isPointerOn);
        }
        else
        {
            networkLocalPosition = (Vector3)stream.ReceiveNext();
            networkLocalRotation = (Quaternion)stream.ReceiveNext();
            LaserPointerOrigin = (Vector3)stream.ReceiveNext();
            LaserPointerDestination = (Vector3)stream.ReceiveNext();
            isPointerOn = (bool)stream.ReceiveNext();
        }
    }
}
