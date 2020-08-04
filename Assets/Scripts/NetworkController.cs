using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public void StartConnect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        GameObject.Find("ButtonText").GetComponent<Text>().text = "Connecting...";
        Debug.Log("Connecting...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the " + PhotonNetwork.CloudRegion + " server!");

        string name = GameObject.Find("NameInput").transform.Find("Text").GetComponent<Text>().text;
        ExitGames.Client.Photon.Hashtable properties =
                PhotonNetwork.LocalPlayer.CustomProperties;
        properties.Add("player_name", name);
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);//
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}
