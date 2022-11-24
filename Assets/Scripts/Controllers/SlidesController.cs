using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidesController : MonoBehaviour
{
    private ApiController api;
    public int index = 0;

    private void Start()
    {
        api = ApiController.GetInstance();
    }

    public void LoadSlides()
    {
        api.loader.Load();
        index = 0;
    }

    public void ClearSlides() 
    {
        api.loader.Clear();
        index = 0;
    }

    public void LoadNextSlide()
    {
        if (index < api.loader.GetLoadedSlides().Count-1)
        {
            index++;
            api.textureController.LoadTextureOnPlane(index);
        }
    }

    public void LoadPrevSlide()
    {
        if (index > 0)
        {
            index--;
            api.textureController.LoadTextureOnPlane(index);
        }
    }
}
