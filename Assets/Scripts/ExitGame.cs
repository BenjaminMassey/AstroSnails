using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Disco()
    {
        Globals.win_data.Clear();
        GameObject GS = GameObject.Find("GameSetup");
        if (GS != null)
        {
            GS.GetComponent<FinishHandler>().StopAllCoroutines();
        }
        PhotonNetwork.Disconnect();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
