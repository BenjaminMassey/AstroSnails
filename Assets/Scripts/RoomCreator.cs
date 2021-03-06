﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class RoomCreator : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject name_input_obj;
    [SerializeField]
    private GameObject name_input_field_text_obj;
    [SerializeField]
    private GameObject password_input_field_obj;
    [SerializeField]
    private GameObject size_input_field_text_obj;

    private RoomOptions roomOps;
    private string roomName;

    public void CreateRoom()
    {
        roomName = name_input_field_text_obj.GetComponent<Text>().text;

        string password = password_input_field_obj.GetComponent<InputField>().text;
        //Globals.room_password = password;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("password"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["password"] = password;
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add("password", password);
        }

        /* WORLD_SIZE: is broken
        string raw_worldSize = size_input_field_text_obj.GetComponent<Text>().text;
        int worldSize;
        if (raw_worldSize.Length > 0)
        {
            try { worldSize = int.Parse(raw_worldSize); }
            catch (Exception e)
            { size_input_field_text_obj.GetComponent<Text>().text = "Invalid size (need 1-100)"; return; }
        }
        else
        {
            worldSize = 10;
        }

        Globals.world_size = worldSize;
        */

        bool already_exists = false;
        foreach (RoomInfo RI in Globals.photon_rooms)
        {
            if (RI.Name.Equals(roomName))
            {
                already_exists = true;
                break;
            }
        }

        if (already_exists)
        {
            name_input_obj.GetComponent<InputField>().text = "NAME EXISTS";
            return;
        }

        Debug.Log("Attempting to create room \"" + roomName + "\"");
        transform.GetChild(0).GetComponent<Text>().text = "Creating...";
        roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 4
        };
        
        StartCoroutine("SafeCreateRoom");
    }

    private IEnumerator SafeCreateRoom()
    {
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return new WaitForFixedUpdate();
        }
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        name_input_obj.GetComponent<InputField>().text = "FAILED";
        transform.GetChild(0).GetComponent<Text>().text = "Create";
    }
}
