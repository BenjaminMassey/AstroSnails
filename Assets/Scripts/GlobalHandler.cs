using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GlobalHandler : MonoBehaviourPun
{ 
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable properties =
                new ExitGames.Client.Photon.Hashtable() { { "running", false } };
            PhotonNetwork.MasterClient.SetCustomProperties(properties);
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("running"))
            {
                if (Globals.running != (bool)PhotonNetwork.MasterClient.CustomProperties["running"])
                {
                    ExitGames.Client.Photon.Hashtable properties =
                        new ExitGames.Client.Photon.Hashtable() { { "running", Globals.running } };
                    PhotonNetwork.MasterClient.SetCustomProperties(properties);
                }
            }
        }
        else
        {
            Globals.running = (bool) PhotonNetwork.MasterClient.CustomProperties["running"];
        }
    }
}

public static class Globals
{
    public static bool running = false;
    public static float collider_time = 0.1f;
}