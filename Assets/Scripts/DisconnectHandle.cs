using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class DisconnectHandle : MonoBehaviourPunCallbacks
{
    public override void OnEnable() { PhotonNetwork.AddCallbackTarget(this); }

    public override void OnDisable() { PhotonNetwork.RemoveCallbackTarget(this); }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Globals.wrong_password = true;
        SceneManager.LoadScene(0);
    }
}
