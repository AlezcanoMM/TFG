using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiController : MonoBehaviour
{
    public TextureController textureController;
    public Loader loader;

    public int index = 0;

    private void Start()
    {
        loader.Load();
        textureController.LoadTextureOnPlane(loader.loadedSlides[index]);
    }
}
