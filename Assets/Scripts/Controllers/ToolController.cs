using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{
    public ApiController api;

    private LineRenderer lineRenderer;

    private bool hasActivated = false;

    private void Start()
    {
        api = ApiController.GetInstance();
    }

    public void ActivatePointer() {
        if (!hasActivated)
        {
            lineRenderer = PhotonNetwork.Instantiate("Pointer", Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
            hasActivated = true;
        }
        else 
        {
            lineRenderer.gameObject.SetActive(true);
        }
    }

    public void DeactivatePointer() {
        lineRenderer.gameObject.SetActive(false);
    }
}
