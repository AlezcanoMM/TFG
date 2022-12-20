using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineController : MonoBehaviour
{
    public PhotonView pv;

    public LineRenderer line;
    public List<Vector3> points = new List<Vector3>();

    private int i = 0;
    private int pointsAddedCounter = 0;

    private void Start()
    {
        pv = gameObject.GetComponent<PhotonView>();
    }

    public void SetPoints(List<Vector3> points) {
        for (; i < points.Count; i++) {
            pv.RPC("LinePositionCountRPC", RpcTarget.All, points[i].x, points[i].y, points[i].z);
        }
    }

    [PunRPC]
    public void LinePositionCountRPC(float x, float y, float z) {
        this.points.Add(new Vector3(x,y,z));
        line.positionCount = points.Count;
    }

    private void Update()
    {
        if (points.Count > 0 && AppController.GetInstance().drawToolController.GetDrawing() && pointsAddedCounter <= i) {
            for (int i = 0; i < points.Count; i++)
            {
                pv.RPC("DrawLinePositionRPC", RpcTarget.All, i);
                pointsAddedCounter++;
            }
        }
    }

    [PunRPC]
    public void DrawLinePositionRPC(int i) {
        line.SetPosition(i, points[i]);
    }

    public void ClearPoints()
    {
        pointsAddedCounter = 0;
        pv.RPC("ClearPointsRPC", RpcTarget.All);
    }

    [PunRPC]
    public void ClearPointsRPC() {
        i = 0;
        points.Clear();
        this.gameObject.SetActive(false);
    }
}
