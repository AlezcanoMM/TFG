using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class VideoLoader : Loader
{
    public PhotonView pv;
    private ApiController api;

    private int counter = 0;

    private void Start()
    {
        api = ApiController.GetInstance();
    }

    public override void Load(string slideId)
    {
        //pv.RPC("LoadVidRPC", RpcTarget.All, slideId);
        string downloadUrl;
        string[] slideInfo = slideId.Split('|');

        if (slideInfo[0] == "video")
        {
            downloadUrl = "https://drive.google.com/uc?export=download&id=" + slideInfo[1];
        }
        else if (slideInfo[0] == "linkVideo")
        {
            downloadUrl = slideInfo[1];
        }
        else
        {
            Debug.LogError("Not suported file type: " + slideInfo[0]);
            return;
        }

        loadedSlidesUrls.Add(downloadUrl);
    }

    public override void Clear()
    {
        pv.RPC("ClearVidRPC", RpcTarget.All);
    }

    [PunRPC]
    public void LoadVidRPC(string slideId)
    {
        string downloadUrl;
        string[] slideInfo = slideId.Split('|');

        if (slideInfo[0] == "video")
        {
            downloadUrl = "https://drive.google.com/uc?export=download&id=" + slideInfo[1];
        }
        else if (slideInfo[0] == "linkVideo")
        {
            downloadUrl = slideInfo[1];
        }
        else
        {
            Debug.LogError("Not suported file type: " + slideInfo[0]);
            return;
        }

        loadedSlidesUrls.Add(downloadUrl);
    }

    [PunRPC]
    public void ClearVidRPC()
    {
        counter = 0;
        loadedSlidesUrls.Clear();
    }
}
