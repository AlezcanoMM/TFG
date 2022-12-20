using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TextureController : MonoBehaviour
{
    private AppController app;

    public PhotonView pv;

    private Vector3 initialPlaneTransform;

    private void Start()
    {
        app = AppController.GetInstance();
        initialPlaneTransform = new Vector3(0.225f, 0.225f, 0.225f); ;
    }

    public void LoadTextureOnPlane(int index) {
        pv.RPC("LoadTextureOnPlaneRPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void LoadTextureOnPlaneRPC(int index)
    {
        Texture2D texture = app.textureLoader.GetLoadedSlidesTextures()[index] as Texture2D;
        app.plane.GetComponent<Renderer>().material.mainTexture = texture;
        float scaleFactor = (float)texture.width / (float)texture.height;
        app.plane.transform.localScale = new Vector3(initialPlaneTransform.x*scaleFactor, initialPlaneTransform.y, initialPlaneTransform.z);
    }

    public void ClearTextureOnPlane() {
        pv.RPC("ClearTextureOnPlaneRPC", RpcTarget.All);
    }

    [PunRPC]
    public void ClearTextureOnPlaneRPC()
    {
        app.plane.GetComponent<Renderer>().material.mainTexture = null;
    }
}
