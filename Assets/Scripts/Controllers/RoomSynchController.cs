using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSynchController : MonoBehaviourPunCallbacks
{
    Room room;
    public int id;

    private bool foundRightHand = false;

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected");
        ApiController.GetInstance().mainMenu.SetActive(true);
    }

    private void Update()
    {
        if (GameObject.Find("Right_RiggedHandRight(Clone)") && !foundRightHand && ApiController.GetInstance().pv.IsMine)
        {
            GameObject.Find("Right_RiggedHandRight(Clone)").AddComponent<PhotonView>().ObservedComponents = new List<Component>();
            GameObject.Find("Right_RiggedHandRight(Clone)").GetComponent<PhotonView>().ObservedComponents.Add(GameObject.Find("Right_RiggedHandRight(Clone)").AddComponent<PhotonTransformView>());
            foundRightHand = true;
        }
        else if (!GameObject.Find("Right_RiggedHandRight(Clone)") && foundRightHand && ApiController.GetInstance().pv.IsMine)
        {
            foundRightHand = false;
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
