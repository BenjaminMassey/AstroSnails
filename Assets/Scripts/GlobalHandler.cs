using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GlobalHandler : MonoBehaviourPun
{

    private Text t;

    void Start()
    {
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable properties =
                new ExitGames.Client.Photon.Hashtable() { { "running", false } };
            PhotonNetwork.MasterClient.SetCustomProperties(properties);
        }
        */

        ExitGames.Client.Photon.Hashtable properties =
                new ExitGames.Client.Photon.Hashtable();
        properties.Add("running", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        t = GameObject.Find("FlyText").GetComponent<Text>();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("running"))
            {
                
                if (Globals.running != (bool)PhotonNetwork.MasterClient.CustomProperties["running"])
                {
                    /*
                    ExitGames.Client.Photon.Hashtable properties =
                        new ExitGames.Client.Photon.Hashtable() { { "running", Globals.running }, { "player_num", 0 } };
                    */
                    ExitGames.Client.Photon.Hashtable properties =
                        new ExitGames.Client.Photon.Hashtable();
                    properties.Add("running", Globals.running);
                    properties.Add("player_num", 1);
                    PhotonNetwork.MasterClient.SetCustomProperties(properties);
                }
            }
        }
        else
        {
            Globals.running = (bool) PhotonNetwork.MasterClient.CustomProperties["running"];
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            t.text = Globals.running.ToString();
        }
    }
}

public static class Globals
{
    public static bool running = false;
    public static float collider_time = 0.1f;
    public static bool first_start = true;
}