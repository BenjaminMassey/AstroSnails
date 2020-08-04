using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using Photon.Realtime;
using System.Linq;
using ExitGames.Client.Photon;

public class Death : MonoBehaviour
{
    private bool dead;

    private FinishHandler fh;

    private void Start()
    {
        dead = false;
        fh = GameObject.Find("GameSetup").GetComponent<FinishHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("TRIGGER ENTER: " + other.gameObject.name);

        if (!Globals.running || dead) { return; }

        Debug.Log(other.gameObject.name + " was hit");

        fh.num_dead++;
        fh.graveyard.Add(gameObject);

        transform.parent.GetComponent<PlayerHandler>().enabled = false;

        dead = true;
    }
    
}
