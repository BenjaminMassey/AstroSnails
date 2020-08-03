using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class DummyLoad : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()//OnLevelWasLoaded()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        StartCoroutine("LoadGame");

        
    }

    IEnumerator LoadGame()
    {
        
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
        
    }
}
