using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int gameSceneIndex;

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
            Debug.Log("Starting game");
            PhotonNetwork.LoadLevel(gameSceneIndex);
        }
    }
}
