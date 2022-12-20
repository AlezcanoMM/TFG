using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidesController : MonoBehaviour
{
    private AppController app;
    public int index = 0;

    public PhotonView pv;

    public int textureIndex = 0;
    public int videoIndex = 0;

    private int recursiveCounter = 0;

    private void Start()
    {
        app = AppController.GetInstance();
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
        ClearPanel();
        app.loader.SetActive(true);
        LoadSlidesRecursive();
    }

    public void LoadSlidesRecursive() {
        if (recursiveCounter >= app.presentationSlidesIds.Count) {
            app.loader.SetActive(false);
            //loads first slide
            string[] firstSlideCheck = app.presentationSlidesIds[0].Split('|');
            if (firstSlideCheck[0] == "image")
            {
                app.textureController.LoadTextureOnPlane(0); //loads first slide
            }
            else if (firstSlideCheck[0] == "video" || firstSlideCheck[0] == "linkVideo")
            {
                app.videoController.LoadVideoOnPlane(0); //loads first slide
            }
            return;
        }

        string[] slideInfo = app.presentationSlidesIds[recursiveCounter].Split('|');
        if (slideInfo[0] == "image")
        {
            app.textureLoader.Load(slideInfo[1]);
        }
        else if (slideInfo[0] == "video" || slideInfo[0] == "linkVideo")
        {
            app.videoLoader.Load(app.presentationSlidesIds[recursiveCounter]);
            recursiveCounter++;
            LoadSlidesRecursive();
        }
    }

    public void IncrementRecursiveCounter() {
        recursiveCounter++;
    }

    public void ClearSlides() 
    {
        pv.RPC("ClearSlidesRPC", RpcTarget.All);
    }

    [PunRPC]
    public void ClearSlidesRPC() {
        app.videoLoader.Clear();
        app.textureLoader.Clear();
        app.drawToolController.Clear();
        index = 0;
        textureIndex = 0;
        videoIndex = 0;
        recursiveCounter = 0;
        ClearPanel();
    }

    public void LoadNextSlide()
    {
        if (index < (app.textureLoader.GetLoadedSlidesTextures().Count + app.videoLoader.GetLoadedSlidesUrls().Count)-1)
        {
            ClearPanel();

            index++;
            string[] slideInfo = app.presentationSlidesIds[index].Split('|');
            if (slideInfo[0] == "image") {
                textureIndex++;
                app.textureController.LoadTextureOnPlane(textureIndex);
            } else if (slideInfo[0] == "video" || slideInfo[0] == "linkVideo") {
                videoIndex++;
                app.videoController.LoadVideoOnPlane(videoIndex);
            }
        }
    }

    public void LoadPrevSlide()
    {
        if (index > 0)
        {
            ClearPanel();

            index--;
            string[] slideInfo = app.presentationSlidesIds[index].Split('|');
            if (slideInfo[0] == "image")
            {
                textureIndex--;
                app.textureController.LoadTextureOnPlane(textureIndex);
            }
            else if (slideInfo[0] == "video" || slideInfo[0] == "linkVideo")
            {
                videoIndex--;
                app.videoController.LoadVideoOnPlane(videoIndex);
            }
        }
    }

    public void ClearPanel() {
        app.textureController.ClearTextureOnPlane();
        if (app.videoController.videoPlayer.isPlaying)
        {
            app.videoController.videoPlayer.Stop();
        }
        app.videoController.videoPlayer.enabled = false;
    }
}
