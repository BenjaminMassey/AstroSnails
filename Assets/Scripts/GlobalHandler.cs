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
        Globals.collider_time = 2;
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable properties =
                new ExitGames.Client.Photon.Hashtable() { { "running", false } };
            PhotonNetwork.MasterClient.SetCustomProperties(properties);
        }
        */


        ExitGames.Client.Photon.Hashtable properties =
                PhotonNetwork.LocalPlayer.CustomProperties;
        if (!properties.ContainsKey("running"))
        {
            properties.Add("running", false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }

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
                        PhotonNetwork.MasterClient.CustomProperties;
                    //properties.Add("running", Globals.running);
                    properties["running"] = Globals.running;
                    //properties.Add("player_num", 1);
                    PhotonNetwork.MasterClient.SetCustomProperties(properties);
                }
            }
        }
        else
        {
            if (!PhotonNetwork.IsConnected) { return; }
            if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("running"))
            {
                Globals.running = (bool)PhotonNetwork.MasterClient.CustomProperties["running"];
            }
            else
            {
                Debug.Log("GlobalHandler.cs does not see 'running' in Master's properties: bad");
            }
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            //t.text = Globals.running.ToString();
        }
    }

    IEnumerator LessFrequentUpdate()
    {
        float time = 1.0f; // seconds
        while (true)
        {
            

            for (int _ = 0; _ < 50 * time; _++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

}

public static class Globals
{
    public static bool running = false;
    public static int collider_time = 2;
    public static bool first_start = true;
    public static Dictionary<string, int> win_data = new Dictionary<string, int>();
    public static Dictionary<string, int> customization_points = null;
    public static int points_left = 10;
    public static float speed_amount = 3.5f;
    public static float turn_amount = 19.0f;
    public static float cap_amount = 25.0f;
    public static float regen_amount = 0.25f;
    //public static int world_size = 0;
    //public static string room_password = "";
    //public static List<RoomInfo> photon_rooms;
    public static bool wrong_password = false;
    public static Dictionary<string, bool> ready_status = new Dictionary<string, bool>();
}