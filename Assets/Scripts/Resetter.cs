using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Resetter : MonoBehaviour
{
    public void Reset()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] containers = new GameObject[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            containers[i] = players[i].transform.parent.gameObject;
        }

        for (int i = 0; i < players.Length; i++)
        {
            PlayerHandler ph = containers[i].GetComponent<PlayerHandler>();
            containers[i].transform.rotation = ph.start_rot;
            players[i].transform.localPosition = new Vector3(0.0f, 0.0f, ph.player_local_z_start);
            players[i].GetComponent<TrailRenderer>().Clear();
            players[i].GetComponent<PositionCache>().data.Clear();
            ph.enabled = true;
            ph.ResetFly();
        }

        int max_iters = 50;
        int iter = 0;
        GameObject cols = GameObject.Find("Colliders");
        BoxCollider[] boxes = cols.GetComponents<BoxCollider>();
        do
        {
            if (iter == max_iters) { break; }
            foreach (BoxCollider box in boxes)
            {
                Destroy(box);
            }
            boxes = cols.GetComponents<BoxCollider>();
            iter++;
            
        } while (boxes.Length > 0);

        ColliderHandler ch = cols.GetComponent<ColliderHandler>();
        ch.first_time = true;
        ch.data_iter = 0;

        GameObject.Find("Text").GetComponent<Text>().text = "";
    }
}
