using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidesController : MonoBehaviour
{
    private ApiController api;
    public int index = 0;

    public PhotonView pv;

    public int textureIndex = 0;
    public int videoIndex = 0;

    private int recursiveCounter = 0;

    private void Start()
    {
        api = ApiController.GetInstance();
    }

    public void LoadSlides()
    {
        index = 0;
        textureIndex = 0;
        videoIndex = 0;


        pv.RPC("LoadSlidesRecursiveRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void LoadSlidesRecursiveRPC() {
        LoadSlidesRecursive();
    }

    public void LoadSlidesRecursive() {
        if (recursiveCounter >= api.presentationSlidesIds.Count) {
            //loads first slide
            string[] firstSlideCheck = api.presentationSlidesIds[0].Split('|');
            if (firstSlideCheck[0] == "image")
            {
                api.textureController.LoadTextureOnPlane(0); //loads first slide
            }
            else if (firstSlideCheck[0] == "video" || firstSlideCheck[0] == "linkVideo")
            {
                api.videoController.LoadVideoOnPlane(0); //loads first slide
            }
            return;
        }

        string[] slideInfo = api.presentationSlidesIds[recursiveCounter].Split('|');
        if (slideInfo[0] == "image")
        {
            api.textureLoader.Load(slideInfo[1]);
        }
        else if (slideInfo[0] == "video" || slideInfo[0] == "linkVideo")
        {
            api.videoLoader.Load(api.presentationSlidesIds[recursiveCounter]);
            recursiveCounter++;
            LoadSlidesRecursive();
        }
        
    }

    public void IncrementRecursiveCounter() {
        recursiveCounter++;
    }

    public void ClearSlides() 
    {
        api.videoLoader.Clear();
        api.textureLoader.Clear();
        index = 0;
        textureIndex = 0;
        videoIndex = 0;
}

    public void LoadNextSlide()
    {
        if (index < (api.textureLoader.GetLoadedSlidesTextures().Count + api.videoLoader.GetLoadedSlidesUrls().Count)-1)
        {
            ClearPanel();

            index++;
            string[] slideInfo = api.presentationSlidesIds[index].Split('|');
            if (slideInfo[0] == "image") {
                textureIndex++;
                api.textureController.LoadTextureOnPlane(textureIndex);
            } else if (slideInfo[0] == "video" || slideInfo[0] == "linkVideo") {
                videoIndex++;
                api.videoController.LoadVideoOnPlane(videoIndex);
            }
        }
    }

    public void LoadPrevSlide()
    {
        if (index > 0)
        {
            ClearPanel();

            index--;
            string[] slideInfo = api.presentationSlidesIds[index].Split('|');
            if (slideInfo[0] == "image")
            {
                textureIndex--;
                api.textureController.LoadTextureOnPlane(textureIndex);
            }
            else if (slideInfo[0] == "video" || slideInfo[0] == "linkVideo")
            {
                videoIndex--;
                api.videoController.LoadVideoOnPlane(videoIndex);
            }
        }
    }

    public void ClearPanel() {
        api.textureController.ClearTextureOnPlane();
        if (api.videoController.videoPlayer.isPlaying)
        {
            api.videoController.videoPlayer.Stop();
            api.videoController.videoPlayer.enabled = false;
        }
    }
}
