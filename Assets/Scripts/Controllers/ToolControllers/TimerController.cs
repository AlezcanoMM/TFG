using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    private ApiController api;

    private GameObject notifPanel;

    private int counter = 0;
    private bool paused = true;

    public string seconds = "00";
    public string minutes = "00";
    public string hours = "00";

    private int mins = 0;
    private int hrs = 0;


    private void Start()
    {
        api = ApiController.GetInstance();
    }

    public void InstantiateTimerPanel() {
        notifPanel = Instantiate(api.notificationPanelPrefab);
        DelegateButtonActions();
        counter = 0;
        notifPanel.GetComponent<NotificationPanelController>().buttons.SetActive(true);
        notifPanel.GetComponent<NotificationPanelController>().UpdateText("00:00:00");
    }

    public void CloserTimerPanel()
    {
        ResetTimer();
        Destroy(notifPanel);
    }

    public void DelegateButtonActions() {
        notifPanel.GetComponent<NotificationPanelController>().buttons.transform.GetChild(0).gameObject.GetComponent<ButtonController>().invokeMethodOn.AddListener(delegate { PlayTimer(); });
        notifPanel.GetComponent<NotificationPanelController>().buttons.transform.GetChild(0).gameObject.GetComponent<ButtonController>().invokeMethodOff.AddListener(delegate { PauseTimer(); });
        notifPanel.GetComponent<NotificationPanelController>().buttons.transform.GetChild(1).gameObject.GetComponent<ButtonController>().invokeMethodOn.AddListener(delegate { ResetTimer(); });
    }

    IEnumerator TimeSeconds()
    {
        while (!paused)
        {
            yield return new WaitForSeconds(1);
            counter++;
            SetTimerText();
        }
    }

    public void SetTimerText()
    {
        UpdateTimeString();
        notifPanel.GetComponent<NotificationPanelController>().UpdateText(hours + ":" + minutes + ":" + seconds);
    }

    public void PlayTimer()
    {
        paused = false;
        StopCoroutine(TimeSeconds());
        StartCoroutine(TimeSeconds());
    }

    public void PauseTimer()
    {
        paused = true;
        StopCoroutine(TimeSeconds());
        StartCoroutine(TimeSeconds());
    }

    public void ResetTimer()
    {
        if (!paused) {
            notifPanel.GetComponent<NotificationPanelController>().buttons.transform.GetChild(0).gameObject.GetComponent<ButtonController>().ForceChangeState();
        }

        paused = true;
        StopCoroutine(TimeSeconds());
        counter = 0;
        mins = 0;
        hrs = 0;
        seconds = "00";
        minutes = "00";
        hours = "00";
        StartCoroutine(ForceTimerTextToZero());
    }

    IEnumerator ForceTimerTextToZero()
    {
        yield return new WaitForSeconds(1);
        notifPanel.GetComponent<NotificationPanelController>().UpdateText("00:00:00");
    }

    public void UpdateTimeString() {
        if (counter < 10)
        {
            seconds = "0" + counter.ToString();
        }
        else if (counter < 60)
        {
            seconds = counter.ToString();
        }
        else if (counter >= 60)
        {
            counter = 0;
            seconds = "00";
            mins++;
        }

        if (mins < 10)
        {
            minutes = "0" + mins;
        }
        else if (mins < 60)
        {
            minutes = mins.ToString();
        }
        else if (mins >= 60)
        {
            mins = 0;
            minutes = "00";
            hrs++;
        }

        if (hrs < 10)
        {
            hours = "0" + hrs;
        }
        else
        {
            hours = hrs.ToString();
        }
    }

}
