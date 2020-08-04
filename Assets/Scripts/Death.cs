using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using Photon.Realtime;
using System.Linq;
using ExitGames.Client.Photon;

public class Death : MonoBehaviourPunCallbacks
{
    private int num_dead;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        num_dead = 0;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("dead"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["dead"] = false;
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add("dead", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("TRIGGER ENTER: " + other.gameObject.name);

        if (!Globals.running) { return; }

        if (gameObject.GetPhotonView().IsMine)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("dead"))
            {
                PhotonNetwork.LocalPlayer.CustomProperties["dead"] = true;
            }
            else
            {
                PhotonNetwork.LocalPlayer.CustomProperties.Add("dead", true);
            }
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (num_dead == players.Length - 1)
        {
            StartCoroutine("Finish");
        }
        else
        {
            GameObject.Find("Colliders").GetComponent<ColliderHandler>().GetPlayers();

            transform.parent.GetComponent<PlayerHandler>().enabled = false;

            num_dead++;
        }
    }

    IEnumerator Finish()
    {
        Debug.Log("Level finish");
        
        Player winner = null;
        Player[] players = PhotonNetwork.CurrentRoom.Players.Values.ToArray();
        foreach (Player p in players)
        {
            if (p.CustomProperties.ContainsKey("dead") &&
                !(bool)p.CustomProperties["dead"])
            {
                winner = p;
            }
        }
        if (winner != null)
        {
            if (winner.CustomProperties.ContainsKey("player_name"))
            {
                GameObject.Find("Text").GetComponent<Text>().text =
                    "Finished!\nWinner: " + winner.CustomProperties["player_name"];
                Debug.Log("Finished!\nWinner: " + winner.CustomProperties["player_name"]);
            }
        }
        else
        {
            Debug.Log("Couldn't find winner");
        }

        Globals.running = false;
        //final_player.transform.parent.GetComponent<PlayerHandler>().enabled = false;

        float results_iter = 50 * 1.5f;
        for (int i = 0; i < results_iter; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        /*
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        PositionCache[] pcs = new PositionCache[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            pcs[i] = players[i].GetComponent<PositionCache>();
            pcs[i].StopAllCoroutines();
        }

        ColliderHandler ch = GameObject.Find("Colliders").GetComponent<ColliderHandler>();
        ch.StopAllCoroutines();

        GameObject.Find("GameSetup").GetComponent<Resetter>().Reset();

        float pause_iter = 50 * 5.0f;
        for (int i = 0; i < pause_iter; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < players.Length; i++)
        {
            pcs[i].StartCoroutine("Cacher");
        }

        ch.StartCoroutine("Colliderer");

        */

        //Globals.running = true;

        //SceneManager.LoadScene(2);

        /*
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            PhotonNetwork.Destroy(players[i].transform.parent.gameObject);
        }
        */

        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(2);
        }
        /*
        else 
        {
            SceneManager.LoadScene(1);
        }
        */
        
    }
    
}
