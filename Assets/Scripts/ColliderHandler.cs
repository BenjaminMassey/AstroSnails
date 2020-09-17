using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ColliderHandler : MonoBehaviour
{
    public GameObject[] players;

    public int[] data_iter;
    public bool[] first_time;

    private Vector3 size;

    private Vector3 dummy = new Vector3(69.0f, 69.0f, 69.0f);

    private float[] speeds;

    private int current_player_num;

    // Start is called before the first frame update
    void Start()
    {
        GetPlayers();
        data_iter = new int[] { 0, 0, 0, 0 };
        first_time = new bool[] { true, true, true, true };
        size = new Vector3(0.5f, 0.5f, 0.5f);
        StartCoroutine("Starter");
    }
    
    IEnumerator Starter()
    {
        while (true)
        {
            if (Globals.running)
            {
                GetPlayers();
                speeds = new float[players.Length];
                for (int i = 0; i < players.Length; i++)
                {
                    speeds[i] = players[i].transform.parent.GetComponent<PlayerHandler>().run_speed;
                }

                current_player_num = 0;
                foreach (GameObject player in players)
                {
                    StartCoroutine("PlayerThread");
                    current_player_num++;
                }

                break;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
                
        }
    }

    private IEnumerator PlayerThread()
    {
        int player_num = current_player_num;

        float speed_coeff = speeds[player_num] / 40.0f;

        while (true)
        {
            if (first_time[player_num])
            {
                float wait_iter = 15 * speed_coeff;
                for (int i = 0; i < wait_iter; i++)
                {
                    yield return new WaitForFixedUpdate();
                }
                first_time[player_num] = false;
            }

            bool failure = false;
            List<Vector3> points = players[player_num].GetComponent<PositionCache>().data;
            if (data_iter[player_num] < points.Count)
            {
                BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                collider.size = size;
                collider.center = points[data_iter[player_num]];
                collider.isTrigger = true;
                if (collider.bounds.Intersects(players[player_num].GetComponent<BoxCollider>().bounds))
                {
                    failure = true;
                    Destroy(collider);
                }
            }
            else { failure = true; }

            float iters = (50 * Globals.collider_time) / speed_coeff;
            for (int i = 0; i < iters; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            if (failure) { continue; }

            data_iter[player_num]++;
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
