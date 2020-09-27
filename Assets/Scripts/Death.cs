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

public class Death : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public override void OnEnable() { PhotonNetwork.AddCallbackTarget(this); }
    public override void OnDisable() { PhotonNetwork.RemoveCallbackTarget(this); }

    [SerializeField]
    private GameObject top_text;

    private bool dead;

    private FinishHandler fh;

    private const byte death_event_code = 72;

    private void Start()
    {
        dead = false;
        fh = GameObject.Find("GameSetup").GetComponent<FinishHandler>();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == death_event_code)
        {
            Debug.Log("Got death event in Death");

            Player dead_player = (Player)photonEvent.CustomData;

            GameObject dead_player_go = null;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p_go in players)
            {
                Player p = p_go.transform.parent.GetComponent<PlayerHandler>().photonView.Owner;
                if (p == dead_player)
                {
                    dead_player_go = p_go;
                    break;
                }
            }

            PlayerDeath(dead_player_go);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("TRIGGER ENTER: " + other.gameObject.name);

        if (!Globals.running || dead) { return; }

        if (!transform.parent.GetComponent<PlayerHandler>().photonView.IsMine)
        {
            return;
        }

        Debug.Log(other.gameObject.name + " was hit");

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        Player me = PhotonNetwork.LocalPlayer;
        PhotonNetwork.RaiseEvent(death_event_code, me, raiseEventOptions, SendOptions.SendReliable);

        PlayerDeath(gameObject);
        
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

    private void PlayerDeath(GameObject player_go)
    {
        if (player_go == null)
        {
            Debug.Log("Death.PlayerDeath got null argument: perhaps player didn't match with game object?");
            return;
        }

        if (fh.graveyard.Contains(player_go))
        {
            Debug.Log("Attempted to add duplicate to graveyard: ignoring");
            return;
        }

        fh.num_dead++;
        fh.graveyard.Add(player_go);

        player_go.transform.parent.GetComponent<PlayerHandler>().enabled = false;

        player_go.GetComponent<Death>().dead = true;
    }

    public bool IsDead()
    {
        return dead;
    }
}
