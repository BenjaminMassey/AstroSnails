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
