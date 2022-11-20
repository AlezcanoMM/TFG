using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureLoader : Loader
{
    public PhotonView pv;
    public string presentationName;

    private void Start()
    {
        Load();
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
        foreach (Object slide in Resources.LoadAll("Presentations/" + presentationName))
        {
            loadedSlides.Add(slide);
        }
    }

    [PunRPC]
    public void ClearRPC() 
    {
        loadedSlides.Clear();
    }
}
