using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player...");
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerContainer"),
                                                      Vector3.zero,
                                                      Quaternion.identity);
        
        int player_count = GameObject.FindGameObjectsWithTag("Player").Length;
        player.transform.Rotate(0.0f, 0.0f, (player_count - 1) * 90.0f);
        player.transform.GetChild(0).name = "Player " + player_count.ToString();
    }
}
