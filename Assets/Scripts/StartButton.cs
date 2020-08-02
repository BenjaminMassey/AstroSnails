using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            transform.GetChild(0).GetComponent<Text>().text = "Waiting...";
            StartCoroutine("CheckStarted");
        }
    }

    IEnumerator CheckStarted()
    {
        while (!Globals.running)
        {
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
    public void Press()
    {
        GameObject.Find("Colliders").GetComponent<ColliderHandler>().GetPlayers();
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
