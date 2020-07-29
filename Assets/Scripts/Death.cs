using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER: " + other.gameObject.name);
        if (transform.parent.name.Contains("1"))
        {
            Globals.running = false;
        }
        else
        {
            Destroy(gameObject);
        }
        //GameObject.Find("Colliders").GetComponent<ColliderHandler>().GetPlayers();
    }
}
