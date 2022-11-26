using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidesController : MonoBehaviour
{
    private ApiController api;
    public int index = 0;

    public int textureIndex = 0;
    public int videoIndex = 0;

    private void Start()
    {
        api = ApiController.GetInstance();
    }

    public void LoadSlides()
    {
        index = 0;
        textureIndex = 0;
        videoIndex = 0;

        foreach (string slideId in api.presentationSlidesIds)
        {
            string[] slideInfo = slideId.Split('|');
            if (slideInfo[0] == "image")
            {
                api.textureLoader.Load(slideInfo[1]);
            }
            else if (slideInfo[0] == "video" || slideInfo[0] == "linkVideo")
            {
                api.videoLoader.Load(slideId);
            }
        }        
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
