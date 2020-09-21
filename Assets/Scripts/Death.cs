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
    [SerializeField]
    private GameObject top_text;

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

        if (transform.parent.GetComponent<PlayerHandler>().photonView.IsMine)
        {
            top_text.GetComponent<Text>().text = "You died!";

            for (int i = 0; i < transform.parent.childCount; i++)
            {
                GameObject child = transform.parent.GetChild(i).gameObject;
                if (child.name.Equals("Main Camera"))
                {
                    child.GetComponent<Camera>().enabled = false;
                    child.GetComponent<AudioListener>().enabled = false;
                }
            }

            GameObject.Find("SpectateArea").GetComponent<BasicSpin>().enabled = true;
            GameObject.Find("SpectateCamera").GetComponent<Camera>().enabled = true;
            GameObject.Find("SpectateCamera").GetComponent<AudioListener>().enabled = true;
        }

        transform.parent.GetComponent<PlayerHandler>().enabled = false;

        dead = true;
    }
    
}
