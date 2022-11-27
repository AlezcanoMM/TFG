using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationPanelController : MonoBehaviour, IPunObservable
{
    public TextMeshPro text;
    public GameObject buttons;

    public PhotonView pv;

    public void UpdateText(string text) {
        this.text.text = text;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(text.text);
        }
        else
        {
            text.text = (string)stream.ReceiveNext();
        }
    }
}
