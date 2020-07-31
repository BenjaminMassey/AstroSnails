using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class Death : MonoBehaviour
{
    private int num_dead;
    private void Start()
    {
        num_dead = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER: " + other.gameObject.name);
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (/*players.Length - 1 == 0*/ num_dead == players.Length - 1)
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<Death>().StartCoroutine("Finish");
            }
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

        float iter = 50 * 1.5f;
        for (int i = 0; i < iter; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        //GameObject.Find("GameSetup").GetComponent<Resetter>().Reset();

        SceneManager.LoadScene(2);
        /*
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(2);
        }
        */
    }
    
}
