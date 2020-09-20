using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomController : MonoBehaviourPunCallbacks
{
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        StartGame();
    }

    void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            /* WORLD_SIZE: is broken
            PhotonNetwork.CurrentRoom.CustomProperties.Add("world_size", Globals.world_size);
            */
            /* Password via room properties is broken
            Debug.Log("Making room with password of \"" + Globals.room_password + "\"");
            PhotonNetwork.CurrentRoom.CustomProperties.Add("password", Globals.room_password);
            */
            Debug.Log("Starting game");
            PhotonNetwork.LoadLevel(1);
        }
    }
}
