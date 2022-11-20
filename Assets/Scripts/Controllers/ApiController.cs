using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiController : MonoBehaviour
{
    public TextureController textureController;
    public Loader loader;

    public GameObject plane;
    public GameObject mainMenu;
    public GameObject slidesMenu;

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

    private void Start()
    {
        plane.SetActive(false);
    }

    public void Present() {
        plane.SetActive(true);
        slidesMenu.SetActive(true);
    }

    public void EndPresentation() {
        plane.SetActive(false);
        slidesMenu.SetActive(false);
    }
}
