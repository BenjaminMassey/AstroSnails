using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER: " + other.gameObject.name);

        if (!Globals.running) { return; }
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (/*players.Length - 1 == 0*/ num_dead == players.Length - 1)
        {
            transform.parent.GetComponent<PlayerHandler>().enabled = false;

            StartCoroutine("Finish");
            /*
            foreach (GameObject player in players)
            {
                player.GetComponent<Death>().StartCoroutine("Finish");
            }
            */
        }
        else
        {
            GameObject.Find("Colliders").GetComponent<ColliderHandler>().GetPlayers();

            transform.parent.GetComponent<PlayerHandler>().enabled = false;

            num_dead++;
            //Destroy(gameObject);
        }
    }

    IEnumerator Finish()
    {

        GameObject final_player = GameObject.FindGameObjectWithTag("Player");

        GameObject.Find("Text").GetComponent<Text>().text = "Finished!\nWinner: " + final_player.name;

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
