using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiController : MonoBehaviour
{
    public TextureController textureController;
    public Loader loader;

    private static ApiController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static ApiController GetInstance()
    {
        return instance;
    }
}
