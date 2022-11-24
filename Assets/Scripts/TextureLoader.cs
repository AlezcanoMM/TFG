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

    public override void Load() {
        pv.RPC("LoadRPC", RpcTarget.All);
    }

    public override void Clear()
    {
        pv.RPC("ClearRPC", RpcTarget.All);
    }

    [PunRPC]
    public void LoadRPC() 
    {
        foreach (string slideId in api.presentationSlidesIds) {
            string downloadUrl = "https://drive.google.com/uc?export=download&id=" + slideId;
            StartCoroutine(LoadTextureFromUrl(downloadUrl));
        }
    }

    [PunRPC]
    public void ClearRPC() 
    {
        counter = 0;
        loadedSlides.Clear();
    }

    IEnumerator LoadTextureFromUrl(string url)
    {
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
                loadedSlides.Add(slide);
                counter++;
            }
        }

        //wait for coroutine to end before loading presentation
        if (counter == api.presentationSlidesIds.Count - 1)
        {
            api.textureController.LoadTextureOnPlane(0); //loads first slide
        }
        else 
        {
            Debug.Log("Loading slides...");
        }
    }
}
