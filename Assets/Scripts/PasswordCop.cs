using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PasswordCop : MonoBehaviourPunCallbacks
{
    public override void OnEnable() { PhotonNetwork.AddCallbackTarget(this); }

    public override void OnDisable() { PhotonNetwork.RemoveCallbackTarget(this); }

    public void Start()
    {
        if (PhotonNetwork.IsMasterClient) { return; }

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("password"))
        {
            if (!((string)PhotonNetwork.LocalPlayer.CustomProperties["password"]).Equals(
                ((string)PhotonNetwork.MasterClient.CustomProperties["password"])))
            {
                Debug.Log("Wrong password: breaking out");
                PhotonNetwork.Disconnect();
            }
        }
        else
        {
            Debug.Log("No password: breaking out");
            PhotonNetwork.Disconnect();
        }
    }
}
