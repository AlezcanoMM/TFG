using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public TextureController textureController;
    public VideoController videoController;
    public PresentationController presenationController;
    public TimerController timerController;
    public SlidesController slidesController;
    public RoomSynchController roomSynchController;
    public DrawToolController drawToolController;
    public PlaneInteractionController planeInteractionController;

    public Loader textureLoader;
    public Loader videoLoader;

    public GameObject plane;
    public GameObject mainMenu;
    public GameObject presentButton;
    public GameObject presentationsButton;
    public GameObject slidesMenu;
    public GameObject presentationSelectMenu;
    public GameObject usersMenu;

    public GameObject presentationOptionPrefab;
    public GameObject notificationPanelPrefab;

    public GameObject loader;

    public PhotonView pv;
    public List<string> presentationSlidesIds = new List<string>();

    private static AppController instance = null;

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

    public static AppController GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        Debug.LogError(PhotonNetwork.MasterClient);
    }

    public void Present() {
        pv.RPC("PresentRPC", RpcTarget.All);
        slidesMenu.SetActive(true);
    }

    public void EndPresentation() {
        pv.RPC("EndPresentationRPC", RpcTarget.All);
        slidesMenu.SetActive(false);
    }

    [PunRPC]
    public void PresentRPC()
    {
        plane.SetActive(true);
    }

    [PunRPC]
    public void EndPresentationRPC()
    {
        plane.transform.localScale = Vector3.zero;
        plane.SetActive(false);
    }
}
