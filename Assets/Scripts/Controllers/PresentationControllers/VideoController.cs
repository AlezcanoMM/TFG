using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private AppController app;

    public PhotonView pv;
    public VideoPlayer videoPlayer;

    private Vector3 initialPlaneTransform;

    private void Start()
    {
        app = AppController.GetInstance();
        initialPlaneTransform = new Vector3(0.225f, 0.225f, 0.225f); ;
    }

    public void LoadVideoOnPlane(int index)
    {
        pv.RPC("LoadVideoOnPlaneRPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void LoadVideoOnPlaneRPC(int index)
    {
        videoPlayer.enabled = true;

        string url = app.videoLoader.GetLoadedSlidesUrls()[index];
        Debug.Log("Loaded slides url: "+url);

        videoPlayer.url = url;

        Texture vidTexture = videoPlayer.texture;
        float scaleFactor = 1f; //(float)vidTexture.width / (float)vidTexture.height
        app.plane.transform.localScale = new Vector3(initialPlaneTransform.x * scaleFactor, initialPlaneTransform.y, initialPlaneTransform.z);

        videoPlayer.Prepare();
        videoPlayer.Play();
    }

    public void StopVideo() {
        pv.RPC("LoadVideoOnPlaneRPC", RpcTarget.All);
    }

    [PunRPC]
    public void StopVideoRPC()
    {
        videoPlayer.Stop();
    }
}
