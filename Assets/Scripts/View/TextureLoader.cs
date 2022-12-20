using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TextureLoader : Loader
{
    public PhotonView pv;
    private AppController app;

    private void Start()
    {
        app = AppController.GetInstance();
    }

    public override void Load(string slideId) {
        //pv.RPC("LoadRPC", RpcTarget.All, slideId);
        string downloadUrl = "https://drive.google.com/uc?export=download&id=" + slideId;
        StartCoroutine(LoadTextureFromUrl(downloadUrl));
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
        loadedSlidesTextures.Clear();
    }

    IEnumerator LoadTextureFromUrl(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Object slide = DownloadHandlerTexture.GetContent(uwr);
                loadedSlidesTextures.Add(slide);
                app.slidesController.IncrementRecursiveCounter();
                app.slidesController.LoadSlidesRecursive();
            }
        }
    }
}
