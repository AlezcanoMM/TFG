using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineController : MonoBehaviour
{
    public PhotonView pv;

    public LineRenderer line;
    public List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        pv = gameObject.GetComponent<PhotonView>();
    }

    public void SetPoints(List<Vector3> points) {
        pv.RPC("LinePositionCountRPC", RpcTarget.All);
        this.points = points;
    }

    [PunRPC]
    public void LinePositionCountRPC() {
        line.positionCount = points.Count;
    }

    private void Update()
    {
        if (points.Count > 0 && AppController.GetInstance().drawToolController.GetDrawing()) {
            for (int i = 0; i < points.Count; i++)
            {
                pv.RPC("DrawLinePositionRPC", RpcTarget.All, points[i].x, points[i].y, points[i].z, i);
            }
        }
    }

    [PunRPC]
    public void DrawLinePositionRPC(float x, float y, float z, int i) {
        line.SetPosition(i, new Vector3(x, y, z));
    }

    public void ClearPoints() {
        points.Clear();
    }
}
