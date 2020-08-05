using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    Text t;

    public override void OnEnable() { PhotonNetwork.AddCallbackTarget(this); }
    public override void OnDisable() { PhotonNetwork.RemoveCallbackTarget(this); }

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        RefreshBoard();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            RefreshBoard();
        }
    }

    public override void OnPlayerEnteredRoom(Player p)
    {
        base.OnPlayerEnteredRoom(p);
        RefreshBoard();
    }

    public override void OnPlayerLeftRoom(Player p)
    {
        base.OnPlayerLeftRoom(p);
        RefreshBoard();
    }

    public void RefreshBoard()
    {
        //Player[] players = PhotonNetwork.CurrentRoom.Players.Values.ToArray();
        Player[] players = PhotonNetwork.PlayerList;
        string[] pnames = new string[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            string pname = (string)players[i].CustomProperties["player_name"];
            if (!Globals.win_data.ContainsKey(pname))
            {
                Globals.win_data.Add(pname, 0);
            }
            pnames[i] = pname;
        }

        string[] names = Globals.win_data.Keys.ToArray<string>();

        foreach (string n in names)
        {
            if (!pnames.Contains(n))
            {
                Globals.win_data.Remove(n);
                names = Globals.win_data.Keys.ToArray<string>();
            }
        }

        int[] score = Globals.win_data.Values.ToArray<int>();

        string content = "";
        for (int i = 0; i < Globals.win_data.Count; i++)
        {
            content += names[i] + ": " + score[i];
            if (i != Globals.win_data.Count - 1) { content += "\n"; }
        }
        Debug.Log("Leaderboard content: " + content);
        t.text = content;
    }
}
