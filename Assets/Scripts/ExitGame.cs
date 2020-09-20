using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Disco()
    {
        PhotonNetwork.Disconnect();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
