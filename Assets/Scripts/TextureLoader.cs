using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TextureLoader : Loader
{
    public PhotonView pv;
    private ApiController api;

    private int counter = 0;

    private void Start()
    {
        api = ApiController.GetInstance();
    }

    public override void Load(string slideId) {
        pv.RPC("LoadRPC", RpcTarget.All, slideId);
    }

    public override void Clear()
    {
        pv.RPC("ClearRPC", RpcTarget.All);
    }

    [PunRPC]
    public void LoadRPC(string slideId) 
    {
        string downloadUrl = "https://drive.google.com/uc?export=download&id=" + slideId;
        StartCoroutine(LoadTextureFromUrl(downloadUrl));
    }

    [PunRPC]
    public void ClearRPC() 
    {
        counter = 0;
        loadedSlidesTextures.Clear();
    }

    IEnumerator LoadTextureFromUrl(string url)
    {
        int prevCounter = counter;
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
                counter++;
            }
            else
            {
                Object slide = DownloadHandlerTexture.GetContent(uwr);
                loadedSlidesTextures.Add(slide);
                counter++;
                api.textureController.LoadTextureOnPlane(0); //loads first slide
            }
        }
    }
}
