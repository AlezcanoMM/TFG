using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaiseHandController : MonoBehaviour
{
    public int time = 5;

    public void NotifyRaiseHand() {
        string message = "User: "+PhotonNetwork.NickName+" has raised their hand!";
        StartCoroutine(NotifyForSeconds(time, message));
    }

    IEnumerator NotifyForSeconds(int time, string message)
    {

        int counter = 0;

        GameObject notification = PhotonNetwork.Instantiate("NotificationPanelPhoton", new Vector3(0.1f, -0.05f, 0), Quaternion.identity, 0);
        notification.GetComponent<NotificationPanelController>().UpdateText(message);

        while (counter < time)
        {
            yield return new WaitForSeconds(1);
            counter++;
        }

        PhotonNetwork.Destroy(notification);
    }
}
