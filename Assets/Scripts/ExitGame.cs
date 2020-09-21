using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Disco()
    {
        Globals.win_data.Clear();
        PhotonNetwork.Disconnect();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
