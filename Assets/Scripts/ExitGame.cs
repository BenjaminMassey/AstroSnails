using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Press()
    {
        PhotonNetwork.Disconnect();
    }
}
