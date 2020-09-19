using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class StartButton : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public override void OnEnable() { PhotonNetwork.AddCallbackTarget(this); }
    public override void OnDisable() { PhotonNetwork.RemoveCallbackTarget(this); }

    private const byte ready_event_code = 69;

    private int ready_count;

    private void Start()
    {
        ready_count = 1;

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("ReadyWait");
        }
        else
        {
            StartCoroutine("CheckStarted");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && 
            PhotonNetwork.IsMasterClient && 
            GetComponent<Button>().interactable)
        {
            Press();
        }
    }

    private void FixedUpdate()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<TrailRenderer>().Clear();
        }
    }

    public override void OnPlayerEnteredRoom(Player p)
    {
        base.OnPlayerEnteredRoom(p);
        if (PhotonNetwork.IsMasterClient)
        {
            StopCoroutine("DelayedInteractible");
            //StartCoroutine("DelayedInteractible");
            StopCoroutine("ReadyWait");
            StartCoroutine("ReadyWait");
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            StopCoroutine("CheckStarted");
            StartCoroutine("DelayedInteractible");
        }
    }

    IEnumerator ReadyWait()
    {
        ReadyInit();
        transform.GetChild(0).GetComponent<Text>().text = "Not all ready...";
        GetComponent<Button>().interactable = false;

        bool all_ready = false;
        while (!all_ready)
        {
            all_ready = ReadyCheck();

            Debug.Log("Ready Check: " + all_ready);

            for (int _ = 0; _ < 30; _++) { yield return new WaitForFixedUpdate(); }
        }

        StartCoroutine("DelayedInteractible");
    }

    IEnumerator DelayedInteractible()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<Button>().interactable = false;
            transform.GetChild(0).GetComponent<Text>().text = "Loading...";
            for (int i = 0; i < 50; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            GetComponent<Button>().interactable = true;
            transform.GetChild(0).GetComponent<Text>().text = "START";
        }
    }

    IEnumerator CheckStarted()
    {
        transform.GetChild(0).GetComponent<Text>().text = "READY";
        GetComponent<Button>().interactable = true;

        while (!Globals.running)
        {
            yield return new WaitForFixedUpdate();
        }
        Press();
        Destroy(gameObject);
    }

    private void ReadyInit()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient) { continue; }

            if (PhotonNetwork.PlayerList[i].CustomProperties.ContainsKey("ready"))
            {
                PhotonNetwork.PlayerList[i].CustomProperties["ready"] = false;
            }
            else
            {
                PhotonNetwork.PlayerList[i].CustomProperties.Add("ready", false);
            }
        }
    }

    private bool ReadyCheck()
    {
        return ready_count >= PhotonNetwork.PlayerList.Length;

        /*
        Debug.Log("ReadyCheck() found " + PhotonNetwork.PlayerList.Length + " players");

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].IsMasterClient) { continue; }

            if (PhotonNetwork.PlayerList[i].CustomProperties.ContainsKey("ready"))
            {
                Debug.Log("Player \"" + (string)PhotonNetwork.PlayerList[i].CustomProperties["player_name"] +
                    "\" has ready value of " + (bool)PhotonNetwork.PlayerList[i].CustomProperties["ready"]);

                if (!(bool)PhotonNetwork.PlayerList[i].CustomProperties["ready"])
                {
                    return false;
                }
            }
            else
            {
                PhotonNetwork.PlayerList[i].CustomProperties.Add("ready", false);
                return false;
            }
        }
        return true;
        */
    }

    public void OnEvent(EventData photonEvent)
    {
        if (PhotonNetwork.IsMasterClient && photonEvent.Code == ready_event_code)
        {
            Debug.Log("Got photon event!");
            ready_count++;
        }
    }


    public void Press()
    {
        if (transform.GetChild(0).GetComponent<Text>().text.Equals("READY"))
        {
            /*
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("ready"))
            {
                PhotonNetwork.LocalPlayer.CustomProperties["ready"] = true;
            }
            else
            {
                PhotonNetwork.LocalPlayer.CustomProperties.Add("ready", true);
            }
            */
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
            PhotonNetwork.RaiseEvent(ready_event_code, null, raiseEventOptions, SendOptions.SendReliable);

            transform.GetChild(0).GetComponent<Text>().text = "Waiting...";
            GetComponent<Button>().interactable = false;
            return;
        }

        ColliderHandler ch = GameObject.Find("Colliders").GetComponent<ColliderHandler>();
        ch.GetPlayers();
        ch.Clear();
        ch.data_iter = new int[] { 0, 0, 0, 0 };
        
        Globals.running = true;
        Globals.first_start = false;

        // Work around for bug that trails aren't cleared
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<TrailRenderer>().Clear();
        }

        GameObject.Find("CharCustom").SetActive(false);

        Destroy(gameObject);
    }
}
