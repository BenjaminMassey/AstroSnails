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

    private const byte start_event_code = 70;

    private int ready_count;

    private GameObject top_text_obj;

    private bool pressing;

    private bool guest_waiting;

    private void Start()
    {
        ready_count = 1;

        Transform p = transform.parent;
        for (int i = 0; i < p.childCount; i++)
        {
            GameObject c = p.GetChild(i).gameObject;
            if (c.name.Equals("Text"))
            {
                top_text_obj = c;
                break;
            }
        }

        pressing = false;
        guest_waiting = true;

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

        while (guest_waiting)
        {
            yield return new WaitForFixedUpdate();
        }
        Press();
    }

    private bool ReadyCheck()
    {
        return ready_count >= PhotonNetwork.PlayerList.Length;
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == ready_event_code)
        {
            string name = (string) photonEvent.CustomData;
            if (Globals.ready_status.ContainsKey(name))
            {
                Globals.ready_status[name] = true;
            }
            else
            {
                Globals.ready_status.Add(name, true);
            }
            GameObject.Find("Leaderboard").GetComponent<Leaderboard>().RefreshBoard();
        }

        if (PhotonNetwork.IsMasterClient && photonEvent.Code == ready_event_code)
        {
            Debug.Log("Master got a ready-up");
            ready_count++;
        }

        if (photonEvent.Code == start_event_code)
        {
            guest_waiting = false;
        }
    }


    public void Press()
    {
        if (transform.GetChild(0).GetComponent<Text>().text.Equals("READY"))
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            string name = (string) PhotonNetwork.LocalPlayer.CustomProperties["player_name"];
            PhotonNetwork.RaiseEvent(ready_event_code, name, raiseEventOptions, SendOptions.SendReliable);

            transform.GetChild(0).GetComponent<Text>().text = "Waiting...";
            GetComponent<Button>().interactable = false;
            return;
        }

        if (pressing) { return; }

        pressing = true;

        ColliderHandler ch = GameObject.Find("Colliders").GetComponent<ColliderHandler>();
        ch.GetPlayers();
        ch.Clear();
        ch.data_iter = new int[] { 0, 0, 0, 0 };

        StartCoroutine("StartGame");
    }

    private IEnumerator StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(start_event_code, null, raiseEventOptions, SendOptions.SendReliable);
            transform.GetChild(0).GetComponent<Text>().text = "Starting...";
            GetComponent<Button>().interactable = false;
        }
        Text t = top_text_obj.GetComponent<Text>();
        t.text = "3...";
        for (int _ = 0; _ < 50; _++) { yield return new WaitForFixedUpdate(); }
        t.text = "2...";
        for (int _ = 0; _ < 50; _++) { yield return new WaitForFixedUpdate(); }
        t.text = "1...";
        for (int _ = 0; _ < 50; _++) { yield return new WaitForFixedUpdate(); }
        t.text = "GO!";
        for (int _ = 0; _ < 5; _++) { yield return new WaitForFixedUpdate(); }
        t.text = "";

        Globals.running = true;
        Globals.first_start = false;

        GameObject.Find("Leaderboard").GetComponent<Leaderboard>().RefreshBoard();

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
