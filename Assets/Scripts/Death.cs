using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER: " + other.gameObject.name);
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length - 1 == 0)
        {
            StartCoroutine("Finish");
        }
        else
        {
            GameObject.Find("Colliders").GetComponent<ColliderHandler>().GetPlayers();

            Destroy(gameObject);
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

        Globals.running = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
