using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class StartButton : MonoBehaviour
{
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("DelayedInteractible");
        }
        else
        {
            GetComponent<Button>().interactable = false;
            transform.GetChild(0).GetComponent<Text>().text = "Waiting...";
            StartCoroutine("CheckStarted");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && 
            PhotonNetwork.IsMasterClient && 
            GetComponent<Button>().interactable)
        {
            Press();
        }
    }

    private void FixedUpdate()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<TrailRenderer>().Clear();
        }
    }

    IEnumerator DelayedInteractible()
    {
        GetComponent<Button>().interactable = false;
        transform.GetChild(0).GetComponent<Text>().text = "Loading...";
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        GetComponent<Button>().interactable = true;
        transform.GetChild(0).GetComponent<Text>().text = "START";
    }

    IEnumerator CheckStarted()
    {
        while (!Globals.running)
        {
            yield return new WaitForFixedUpdate();
        }
        Press();
        Destroy(gameObject);
    }
    public void Press()
    {
        /* Doesn't fix :(
        // Fix for loading in with only one player (need a reload)
        if (PhotonNetwork.IsMasterClient && !Globals.first_start && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            int player_count = GameObject.FindGameObjectsWithTag("Player").Length;
            Debug.Log("See " + player_count + " players in the scene");
            if (player_count == 1)
            {
                PhotonNetwork.LoadLevel(2);
            }
        }
        */

        ColliderHandler ch = GameObject.Find("Colliders").GetComponent<ColliderHandler>();
        ch.GetPlayers();
        ch.Clear();
        ch.data_iter = 0;
        
        Globals.running = true;
        Globals.first_start = false;

        // Work around for bug that trails aren't clear
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<TrailRenderer>().Clear();
        }

        Destroy(gameObject);
    }
}
