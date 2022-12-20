using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneInteractionController : MonoBehaviour, IMixedRealityPointerHandler
{
    private AppController app;

    public List<Vector3> points = new List<Vector3>();

    private Vector3 originRay = new Vector3();
    private Vector3 destinationRay = new Vector3();
    private Vector3 endPointRay = new Vector3();

    private bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        app = AppController.GetInstance();
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        /*Debug.Log("CLICK Button " + gameObject.name + " has been clicked.");
        if (app.drawToolController.GetDrawing()) {
            GetCursorPositionOnPlane();
            app.drawToolController.line.GetComponent<DrawLineController>().SetPoints(points);
        }*/
    }

    

    private void GetCursorPositionOnPlane()
    {
        Ray rightHandRay;
        Vector3 position = new Vector3();
        if (InputRayUtils.TryGetHandRay(Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right, out rightHandRay))
        {
            //Debug.Log("Right HAND available: origin = " + rightHandRay.origin + " direction = " + rightHandRay.direction);
            originRay = rightHandRay.origin;
            destinationRay = rightHandRay.origin + rightHandRay.direction;
            endPointRay = originRay + (destinationRay - originRay) * 10f;
        }
        else if (InputRayUtils.TryGetHandRay(Microsoft.MixedReality.Toolkit.Utilities.Handedness.Left, out rightHandRay))
        {
            //Debug.Log("Right HAND available: origin = " + rightHandRay.origin + " direction = " + rightHandRay.direction);
            originRay = rightHandRay.origin;
            destinationRay = rightHandRay.origin + rightHandRay.direction;
            endPointRay = originRay + (destinationRay - originRay) * 10f;
        }
        RaycastHit[] hits = Physics.RaycastAll(rightHandRay, 10);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Plane"))
            {
                position[0] = hit.point.x;
                position[1] = hit.point.y;
                position[2] = 4.99f;
            }
        }
        points.Add(position);
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        isDragging = true;
    }

    private void FixedUpdate()
    {
        if (isDragging) {
            if (app.drawToolController.GetDrawing())
            {
                GetCursorPositionOnPlane();
                app.drawToolController.line.GetComponent<DrawLineController>().SetPoints(points);
            }
        }
    }

    #region unused
    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        /*if (app.drawToolController.GetDrawing())
        {
            GetCursorPositionOnPlane();
            app.drawToolController.line.GetComponent<DrawLineController>().SetPoints(points);
        }*/
    }
    #endregion
}
