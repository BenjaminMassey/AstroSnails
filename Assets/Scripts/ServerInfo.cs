using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ServerInfo : MonoBehaviourPun
{
    Text t;

    private float secs = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        StartCoroutine("Updater");
    }

    IEnumerator Updater()
    {
        while (true)
        {
            string content = "Server: " + PhotonNetwork.CloudRegion + "\n";
            content += "Room: " + PhotonNetwork.CurrentRoom.Name + "\n";
            content += "Ping: " + PhotonNetwork.GetPing();
            t.text = content;
            for (int i = 0; i < 50 * secs; i++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
