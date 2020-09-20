using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomJoiner : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField]
    private GameObject password_input_field_obj;

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

        string password = password_input_field_obj.GetComponent<InputField>().text;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("password"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["password"] = password;
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add("password", password);
        }

        /*
        Debug.Log("Attempting to find rooms...");
        RoomInfo wanted_room = null;
        foreach (RoomInfo room in Globals.photon_rooms)
        {
            Debug.Log("See room with name \"" + room.Name + "\"" + " and password \"" + room.CustomProperties["password"] + "\"");
            if (room.Name.Equals(name))
            {
                if (room.CustomProperties.ContainsKey("password"))
                {
                    if (((string)room.CustomProperties["password"]).Equals(password))
                    {
                        wanted_room = room;
                    }
                }
                else
                {
                    Debug.Log("Didn't find password for room \"" + room.Name + "\"");
                    wanted_room = room;
                }
            }
        }

        if (wanted_room != null)
        {*/
            t.text = "Joining...";
            PhotonNetwork.JoinRoom(name);/*
        }
        else
        {
            StartCoroutine("Fail");
        }*/
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
