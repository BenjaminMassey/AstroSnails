using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomJoiner : MonoBehaviourPunCallbacks
{
    private Text t;

    private string orig_text;

    private void Start()
    {
        t = transform.GetChild(0).GetComponent<Text>();
        orig_text = null;
    }
    public void JoinRoom()
    {
        string name = t.text;
        orig_text = name;
        t.text = "Joining...";
        PhotonNetwork.JoinRoom(name);
    }

    public void RefreshRooms()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        StartCoroutine("Fail");
    }

    IEnumerator Fail()
    {
        t.text = "FAILED";
        for (int i = 0; i < 70; i++) { yield return new WaitForFixedUpdate(); }
        if (orig_text != null)
        {
            t.text = orig_text;
        }
        RefreshRooms();
    }
}
