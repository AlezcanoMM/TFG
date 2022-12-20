using Microsoft.MixedReality.Toolkit.Input;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawToolController : MonoBehaviour
{
    public PhotonView pv;

    [SerializeField]
    private List<GameObject> lines = new List<GameObject>();
    public GameObject line;

    private bool isDrawing = false;

    public void Draw() {
        line = PhotonNetwork.Instantiate("DrawLine", Vector3.zero, Quaternion.identity, 0);

        isDrawing = true;
        pv.RPC("DrawRPC", RpcTarget.All);
    }

    [PunRPC]
    public void DrawRPC() {
        lines.Add(line);
    }

    public void StopDrawing() {
        isDrawing = false;
    }

    public bool GetDrawing() {
        return isDrawing;
    }

    public void Clear() {
        pv.RPC("ClearRPC", RpcTarget.All);
    }

    [PunRPC]
    public void ClearRPC() {
        foreach (GameObject line in lines)
        {
            line.GetComponent<DrawLineController>().ClearPoints();
            line.SetActive(false);
        }
    }
}
