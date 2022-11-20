using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TextureController : MonoBehaviour
{
    private ApiController api;

    public PhotonView pv;
    public GameObject plane;

    private void Start()
    {
        api = ApiController.GetInstance();
        plane.SetActive(false);
    }

    public void LoadTextureOnPlane(int index) {
        pv.RPC("LoadTextureOnPlaneRPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void LoadTextureOnPlaneRPC(int index)
    {
        Texture2D texture = api.loader.GetLoadedSlides()[index] as Texture2D;
        plane.GetComponent<Renderer>().material.mainTexture = texture;
        float scaleFactor = (float)texture.width / (float)texture.height;
        plane.transform.localScale = new Vector3(scaleFactor, 1, 1);
    }
}
