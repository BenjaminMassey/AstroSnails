using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class DummyLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void OnLevelWasLoaded()//Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
