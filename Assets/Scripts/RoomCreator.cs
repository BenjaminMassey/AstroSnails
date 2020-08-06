using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomCreator : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject input_field_text_obj;

    public void CreateRoom()
    {
        string roomName = input_field_text_obj.GetComponent<Text>().text;
        Debug.Log("Attempting to create room \"" + roomName + "\"");
        transform.GetChild(0).GetComponent<Text>().text = "Creating...";
        RoomOptions roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 4
        };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        input_field_text_obj.GetComponent<Text>().text = "FAILED";
        transform.GetChild(0).GetComponent<Text>().text = "Create";
    }
}
