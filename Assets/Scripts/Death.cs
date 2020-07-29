using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER: " + other.gameObject.name);
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length - 1 == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            GameObject.Find("Colliders").GetComponent<ColliderHandler>().GetPlayers();

            Destroy(gameObject);
        }
    }
}
