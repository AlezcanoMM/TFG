using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TextureController : MonoBehaviour
{
    private ApiController api;

    public PhotonView pv;

    private Vector3 initialPlaneTransform;

    private void Start()
    {
        api = ApiController.GetInstance();
        initialPlaneTransform = new Vector3(0.225f, 0.225f, 0.225f); ;
    }

    public void LoadTextureOnPlane(int index) {
        pv.RPC("LoadTextureOnPlaneRPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void LoadTextureOnPlaneRPC(int index)
    {
        Texture2D texture = api.textureLoader.GetLoadedSlidesTextures()[index] as Texture2D;
        api.plane.GetComponent<Renderer>().material.mainTexture = texture;
        float scaleFactor = (float)texture.width / (float)texture.height;
        api.plane.transform.localScale = new Vector3(initialPlaneTransform.x*scaleFactor, initialPlaneTransform.y, initialPlaneTransform.z);
    }

    public void ClearTextureOnPlane() {
        pv.RPC("LoadTextureOnPlaneRPC", RpcTarget.All);
    }

    [PunRPC]
    public void ClearTextureOnPlaneRPC()
    {
        api.plane.GetComponent<Renderer>().material.mainTexture = null;
    }
}
