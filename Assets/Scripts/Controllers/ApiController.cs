using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiController : MonoBehaviour
{
    public TextureController textureController;
    public PresentationController presenationController;
    public Loader loader;

    public GameObject plane;
    public GameObject mainMenu;
    public GameObject presentButton;
    public GameObject slidesMenu;
    public GameObject presentationSelectMenu;

    public GameObject presentationOptionPrefab;

    public PhotonView pv;
    public List<string> presentationSlidesIds = new List<string>();

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
        pv.RPC("PresentRPC", RpcTarget.All);
    }

    public void EndPresentation() {
        pv.RPC("EndPresentationRPC", RpcTarget.All);
    }

    [PunRPC]
    public void PresentRPC()
    {
        plane.SetActive(true);
        slidesMenu.SetActive(true);
    }

    [PunRPC]
    public void EndPresentationRPC()
    {
        plane.transform.localScale = Vector3.zero;
        plane.SetActive(false);
        slidesMenu.SetActive(false);
    }

}
