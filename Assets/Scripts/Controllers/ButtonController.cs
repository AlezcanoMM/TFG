using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour, IMixedRealityFocusHandler, IMixedRealityPointerHandler, IMixedRealityTouchHandler
{
    public bool buttonChanges = true;

    [SerializeField]
    private bool isClicked = false;

    [Header("Methods On/Off")]
    public UnityEvent invokeMethodOn;
    public UnityEvent invokeMethodOff;

    public TextMeshPro label;

    public void OnClick() {
        if (!isClicked)
        {
            invokeMethodOn?.Invoke();
        }
        else {
            invokeMethodOff?.Invoke();
        }
        ChangeState();
    }

    private void ChangeState() {
        if (buttonChanges) {
            isClicked = !isClicked;
        }
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Debug.Log("CLICK Button " + gameObject.name + " has been clicked.");
        OnClick();
    }

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        Debug.Log("TOUCH Button " + gameObject.name + " has been clicked.");
        OnClick();
    }

    public void OnFocusEnter(FocusEventData eventData)
    {
       //throw new System.NotImplementedException();
    }

    public void OnFocusExit(FocusEventData eventData)
    {
        //throw new System.NotImplementedException();
    }


    #region unused
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {

    }

    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {

    }

    public void OnTouchUpdated(HandTrackingInputEventData eventData)
    {

    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {

    }
    #endregion
}
