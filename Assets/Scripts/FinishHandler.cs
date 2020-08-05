using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class FinishHandler : MonoBehaviourPunCallbacks
{
    public int num_dead;

    public List<GameObject> graveyard;

    public override void OnEnable() { PhotonNetwork.AddCallbackTarget(this); }
    public override void OnDisable() { PhotonNetwork.RemoveCallbackTarget(this); }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("FINISH"))
        {
            PhotonNetwork.MasterClient.CustomProperties["FINISH"] = null;
            PhotonNetwork.MasterClient.CustomProperties.Remove("FINISH");
        }
        
        num_dead = 0;
        graveyard = new List<GameObject>();
        StartCoroutine("Checker");
    }

    IEnumerator Checker()
    {
        /*
        while (!Globals.running) { yield return new WaitForFixedUpdate(); }

        for (int i = 0; i < 75; i++) { yield return new WaitForFixedUpdate(); }

        if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("FINISH"))
        {
            PhotonNetwork.MasterClient.CustomProperties["FINISH"] = null;
            PhotonNetwork.MasterClient.CustomProperties.Remove("FINISH");
        }
        */
        while (true)
        {
            int count = PhotonNetwork.CurrentRoom.PlayerCount;
            if (count > 1)
            {
                if (num_dead >= count - 1) { break; }
            }
            else
            {
                if (num_dead == 1) { break; }
            }
            if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("FINISH") &&
                PhotonNetwork.MasterClient.CustomProperties["FINISH"] != null)
                { Debug.Log("FUCK ABORT ABORT"); break; }
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine("Finish");
    }

    IEnumerator Finish()
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
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.MasterClient.CustomProperties.Add("FINISH", winner);
        }
        else
        {
            if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("FINISH"))
            {
                winner = (Player)PhotonNetwork.MasterClient.CustomProperties["FINISH"];
            }
            else
            {
                PhotonNetwork.MasterClient.CustomProperties.Add("FINISH", winner);
            }
        }

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

        if (PhotonNetwork.MasterClient.CustomProperties.ContainsKey("FINISH"))
        {
            PhotonNetwork.MasterClient.CustomProperties["FINISH"] = null;
            PhotonNetwork.MasterClient.CustomProperties.Remove("FINISH");
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(2);
        }

    }
}
