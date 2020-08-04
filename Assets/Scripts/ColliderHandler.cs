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
                    int wait_iter = 25; // default 15
                    for (int i = 0; i < wait_iter; i++)
                    {
                        yield return new WaitForFixedUpdate();
                    }
                    first_time = false;
                }
                bool failure = false;
                for (int i = 0; i < players.Length; i++)
                {
                    List<Vector3> points = players[i].GetComponent<PositionCache>().data;
                    if (data_iter < points.Count)
                    {
                        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                        collider.size = size;
                        collider.center = points[data_iter];
                        collider.isTrigger = true;
                    }
                }

                float iters = 50 * Globals.collider_time;
                for (int i = 0; i < iters; i++)
                {
                    yield return new WaitForFixedUpdate();
                }

                if (failure) { continue; }

                data_iter++;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void Clear()
    {
        int max_iters = 50;
        int iter = 0;
        BoxCollider[] boxes = GetComponents<BoxCollider>();
        do
        {
            if (iter == max_iters) { break; }
            foreach (BoxCollider box in boxes)
            {
                Destroy(box);
            }
            boxes = GetComponents<BoxCollider>();
            iter++;

        } while (boxes.Length > 0);
    }

    public void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
