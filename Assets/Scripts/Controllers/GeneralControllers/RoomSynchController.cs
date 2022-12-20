using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSynchController : MonoBehaviourPunCallbacks
{
    private bool roomMaster = false;

    Room room;
    public int id;

    public PhotonView pv;

    private string username;

    private bool instantiatedRight = false;
    private bool instantiatedLeft = false;

    private GameObject userButton;

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected");

        if (PhotonNetwork.CountOfPlayers == 1 && photonView.IsMine) {
            Debug.Log("You are room master");
            roomMaster = true;
            AppController.GetInstance().presentationsButton.SetActive(true);
        }

        AppController.GetInstance().mainMenu.SetActive(true);
        username = PhotonNetwork.NickName;
        pv.RPC("CreateUserButtonRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, username);

    }

    public bool GetRoomMaster() {
        return roomMaster;
    }

    private void Update()
    {
        /*if (GameObject.Find("Right_RiggedHandRight(Clone)") && !instantiatedRight) {
            PhotonNetwork.Instantiate("Right_HandModel",Vector3.zero,Quaternion.identity,0);
            instantiatedRight = true;
        }

        if (GameObject.Find("Left_RiggedHandLeft(Clone)") && !instantiatedLeft)
        {
            PhotonNetwork.Instantiate("Left_HandModel", Vector3.zero, Quaternion.identity, 0);
            instantiatedLeft = true;
        }*/
    }

    [PunRPC]
    private void CreateUserButtonRPC(Player localPlayer, string username) {
        GameObject userButton = PhotonNetwork.Instantiate("UserButton", new Vector3(-500, -500, 0), Quaternion.Euler(-90, 0, 0), 0);

        //Delegate to button that if roomMaster clicks it, you can forfeit roomMaster
        userButton.GetComponent<InterfaceController>().label.text = username;
        if (pv.Owner == localPlayer)
        {
            userButton.GetComponent<InterfaceController>().label.text = "You";
        }
        userButton.GetComponent<InterfaceController>().invokeMethodOn.AddListener(delegate { ForfeitMaster(localPlayer); });

        userButton.transform.SetParent(AppController.GetInstance().usersMenu.transform, false);
    }

    public void ForfeitMaster(Player player) {
        if (!roomMaster) {
            return;
        }

        roomMaster = false;

        bool statePresentationsButton = AppController.GetInstance().presentationsButton.activeSelf;
        bool statePresentationSelectMenu = AppController.GetInstance().presentationSelectMenu.activeSelf;
        bool statePresentButton = AppController.GetInstance().presentButton.activeSelf;
        bool stateSlidesMenu = AppController.GetInstance().slidesMenu.activeSelf;

        AppController.GetInstance().presentationsButton.SetActive(false);
        AppController.GetInstance().presentationSelectMenu.SetActive(false);
        AppController.GetInstance().presentButton.SetActive(false);
        AppController.GetInstance().slidesMenu.SetActive(false);

        pv.RPC("ForfeitMasterRPC", RpcTarget.AllBuffered, player, statePresentationsButton, statePresentationSelectMenu, statePresentButton, stateSlidesMenu);
    }

    [PunRPC]
    public void ForfeitMasterRPC(Player player, bool statePresentationsButton, bool statePresentationSelectMenu, bool statePresentButton, bool stateSlidesMenu)
    {
        if (PhotonNetwork.LocalPlayer == player)
        {
            roomMaster = true;

            AppController.GetInstance().presentationsButton.SetActive(statePresentationsButton);
            AppController.GetInstance().presentationSelectMenu.SetActive(statePresentationSelectMenu);
            AppController.GetInstance().presentButton.SetActive(statePresentButton);
            AppController.GetInstance().slidesMenu.SetActive(stateSlidesMenu);
        }
    }

    /*
    void Awake()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        room = new Room
        {
            roomName = "test",
            roomCapacity = 4
        };
        id = 0;

        Connect();
    }

    public void Connect()
    {

        room.roomName = Application.productName + id;

        if (PhotonNetwork.ConnectUsingSettings()) //Connects to master photon server
        {
            Debug.Log("Connected to server");
        }
        else
        {
            Debug.Log("Failed to connect");
        }

    }

    public void CreateRoom()
    {
        RoomOptions roomOpts = new RoomOptions() { MaxPlayers = room.roomCapacity };
        Debug.Log("Attempting to create room: " + room.roomName);
        PhotonNetwork.CreateRoom(room.roomName, roomOpts);
        Debug.Log("Room created succesfully");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(room.roomName);
    }

    #region PUN Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("User has connected to the photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        JoinRoom();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + room.roomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room with name: " + room.roomName + "\n it does not exist");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed: A room with this name already exists");
        room.roomName = room.roomName + Random.Range(1, 100).ToString();
        Debug.Log("Generated a new room name: " + room.roomName);
        CreateRoom();
    }
    #endregion
    */
}
