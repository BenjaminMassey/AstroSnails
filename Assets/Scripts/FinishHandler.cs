using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class FinishHandler : MonoBehaviourPunCallbacks
{
    public int num_dead;

    public List<GameObject> graveyard;

    public override void OnEnable() { PhotonNetwork.AddCallbackTarget(this); }
    public override void OnDisable() { PhotonNetwork.RemoveCallbackTarget(this); }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("winner_found"))
        {
            PhotonNetwork.MasterClient.CustomProperties["winner_found"] = false;
        }
        else
        {
            PhotonNetwork.MasterClient.CustomProperties.Add("winner_found", false);
        }

        if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("winner"))
        {
            PhotonNetwork.MasterClient.CustomProperties["winner"] = null;
        }
        else
        {
            PhotonNetwork.MasterClient.CustomProperties.Add("winner", null);
        }

        num_dead = 0;
        graveyard = new List<GameObject>();
        StartCoroutine("Checker");
    }

    IEnumerator Checker()
    {
        while (true)
        {
            int count = 0;
            try { count = PhotonNetwork.CurrentRoom.PlayerCount; }
            catch (Exception e) { }

            if (count > 1)
            {
                if (num_dead >= count - 1) { break; }
            }
            else
            {
                if (num_dead == 1) { break; }
            }
            if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("winner_found") &&
                (bool)PhotonNetwork.MasterClient.CustomProperties["winner_found"])
            {
                Debug.Log("Host found winner: abort");
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("HostFinish");
        }
        else
        {
            StartCoroutine("GuestFinish");
        }
    }

    IEnumerator HostFinish()
    {

        Debug.Log("Level finish");

        Player winner = null;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (!graveyard.Contains(player))
            {
                winner = player.GetPhotonView().Owner;
            }
        }

        PhotonNetwork.MasterClient.CustomProperties["winner_found"] = true;
        PhotonNetwork.MasterClient.CustomProperties["winner"] = winner;

        if (winner != null)
        {
            Leaderboard lb = GameObject.Find("Leaderboard").GetComponent<Leaderboard>();
            lb.RefreshBoard();
            Globals.win_data[(string)winner.CustomProperties["player_name"]]++;
            lb.RefreshBoard();

            if (winner.CustomProperties.ContainsKey("player_name"))
            {
                GameObject.Find("Text").GetComponent<Text>().text =
                    "Finished!\nWinner: " + winner.CustomProperties["player_name"];
                Debug.Log("Finished!\nWinner: " + winner.CustomProperties["player_name"]);
            }
        }
        else
        {
            GameObject.Find("Text").GetComponent<Text>().text = "Tie!";
            Debug.Log("Couldn't find winner");
        }

        Globals.running = false;

        float results_iter = 50 * 1.5f;
        for (int i = 0; i < results_iter; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.LoadLevel(2);

    }

    IEnumerator GuestFinish()
    {

        Debug.Log("Level finish");

        bool winner_found = false;
        while (!winner_found)
        {
            winner_found = (bool)PhotonNetwork.MasterClient.CustomProperties["winner_found"];
            yield return new WaitForFixedUpdate();
        }

        Player winner = (Player)PhotonNetwork.MasterClient.CustomProperties["winner"];

        if (winner != null)
        {
            Leaderboard lb = GameObject.Find("Leaderboard").GetComponent<Leaderboard>();
            lb.RefreshBoard();
            Globals.win_data[(string)winner.CustomProperties["player_name"]]++;
            lb.RefreshBoard();

            if (winner.CustomProperties.ContainsKey("player_name"))
            {
                GameObject.Find("Text").GetComponent<Text>().text =
                    "Finished!\nWinner: " + winner.CustomProperties["player_name"];
                Debug.Log("Finished!\nWinner: " + winner.CustomProperties["player_name"]);
            }
        }
        else
        {
            GameObject.Find("Text").GetComponent<Text>().text = "Tie!";
            Debug.Log("Couldn't find winner");
        }

        Globals.running = false;
    }

}
