using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviourPunCallbacks
{

    // Start is called before the first frame update
    
    void Start()
    {
        /*
        // Fix for loading in with only one player (need a reload)
        if (!Globals.first_start)
        {
            int player_count = GameObject.FindGameObjectsWithTag("Player").Length;
            Debug.Log("See " + player_count + " players in the scene");
            if (player_count == 1)
            {
                PhotonNetwork.LoadLevel(2);
            }
        }
        */

        CreatePlayer();

        // FIX ROTATION, CLEAR TRAILS
        //PlayersReset();
    }

    public void PlayersReset()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<TrailRenderer>().Clear();
        }
    }

    /*
    void OnLevelWasLoaded(int level)
    {
        CreatePlayer();
    }
    */

    private void CreatePlayer()
    {
        Debug.Log("Creating player...");
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerContainer"),
                                                      Vector3.zero,
                                                      Quaternion.identity);
        /*
        int player_count = PhotonNetwork.PlayerList.Length;
        //player.transform.Rotate(0.0f, (player_count - 1) * 90.0f, 0.0f);
        //player.GetComponent<PlayerHandler>().start_rot = transform.rotation;
        float y_rot = (new float[] { 0.0f, 90.0f, 180.0f, 270.0f })[player_count - 1];
        player.GetComponent<PlayerHandler>().start_rot = new Vector3(0.0f, y_rot, 0.0f);
        player.name = "Player" + player_count.ToString() + "Container";
        player.transform.GetChild(0).name = "Player " + player_count.ToString();
        */
    }
}
