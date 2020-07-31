using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    /*
    [SerializeField]
    private GameObject quickStartButton;
    [SerializeField]
    private GameObject quickCancelButton;
    */
    [SerializeField]
    private int room_size;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Attempting to join existing room...");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join existing room.");
        CreateRoom();
    }

    void CreateRoom()
    {
        int randomRoomNumber = Random.Range(0, 50);
        string roomName = "Room " + randomRoomNumber.ToString();
        Debug.Log("Attempting to create room \"" + roomName + "\"");
        RoomOptions roomOps = new RoomOptions() 
        { 
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)room_size
        };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: retrying");
        CreateRoom();
    }
}
