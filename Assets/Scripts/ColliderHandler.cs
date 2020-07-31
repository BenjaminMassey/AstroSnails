using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ColliderHandler : MonoBehaviour
{
    public GameObject[] players;

    public int data_iter;
    public bool first_time;

    private Vector3 size;

    private Vector3 dummy = new Vector3(69.0f, 69.0f, 69.0f);

    // Start is called before the first frame update
    void Start()
    {
        GetPlayers();
        data_iter = 0;
        first_time = true;
        size = new Vector3(0.5f, 0.5f, 0.5f);
        StartCoroutine("Colliderer");
    }
    
    IEnumerator Colliderer()
    {
        while (true)
        {
            if (Globals.running)
            {
                if (first_time)
                {
                    int wait_iter = 15;
                    for (int i = 0; i < wait_iter; i++)
                    {
                        yield return new WaitForFixedUpdate();
                    }
                    first_time = false;
                }
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
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
