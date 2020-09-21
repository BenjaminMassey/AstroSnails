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
        Globals.ready_status.Clear();
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
        //List<int> ignores = new List<int>();
        for (int i = 0; i < names.Length; i++)
        {
            if (!pnames.Contains(names[i]))
            {
                //ignores.Add(i);
                Globals.win_data.Remove(names[i]);
                names = Globals.win_data.Keys.ToArray<string>();
            }
        }

        int[] score = Globals.win_data.Values.ToArray<int>();

        string content = "";
        for (int i = 0; i < Globals.win_data.Count; i++)
        {
            //if (ignores.Contains(i)) { continue; }
            content += names[i] + ": " + score[i];
            if (!Globals.running)
            {
                if (Globals.ready_status.ContainsKey(names[i]) &&
                Globals.ready_status[names[i]] == true)
                {
                    content += " [✓]";
                }
                else
                {
                    content += " [ ]";
                }
            }
            if (i != Globals.win_data.Count - 1) { content += "\n"; }
        }
        Debug.Log("Leaderboard content: " + content);
        t.text = content;
    }
}
