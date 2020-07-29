using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ColliderHandler : MonoBehaviour
{
    private GameObject[] players;

    private Vector3 size;

    private Vector3 dummy = new Vector3(69.0f, 69.0f, 69.0f);

    // Start is called before the first frame update
    void Start()
    {
        GetPlayers();
        size = new Vector3(0.5f, 0.5f, 0.5f);
        StartCoroutine("Colliderer");
    }
    
    IEnumerator Colliderer()
    {
        int wait_iter = 15;
        for (int i = 0; i < wait_iter; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        int data_iter = 0;
        while (Globals.running)
        {
            for (int i = 0; i < players.Length; i++)
            {
                
                BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                collider.size = size;
                collider.center = players[i].GetComponent<PositionCache>().data[data_iter];
                collider.isTrigger = true;
            }

            float iters = 50 * Globals.collider_time;
            for (int i = 0; i < iters; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            data_iter++;
        }
    }

    public void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
