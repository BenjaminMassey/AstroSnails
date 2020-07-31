using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartButton : MonoBehaviour
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(gameObject);
        }
    }
    public void Press()
    {
        Globals.running = true;
        Destroy(gameObject);
    }
}
