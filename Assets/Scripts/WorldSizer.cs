using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSizer : MonoBehaviour
{
    // Globals.world_size and the room property set up in RoomCreator.cs and RoomController.cs

    // To be attached to world and each player

    /* WORLD_SIZE: is broken

    // Start is called before the first frame update
    void Start()
    {
        int world_size = (int)PhotonNetwork.CurrentRoom.CustomProperties["world_size"];

        transform.localScale = Vector3.one * world_size;

        if (transform.childCount > 0) // player, not world
        {
            Transform child = transform.GetChild(0);
            child.localScale *= (1.0f / world_size);
            child.GetComponent<TrailRenderer>().startWidth *= (1.0f / world_size);
            child.GetComponent<TrailRenderer>().endWidth *= (1.0f / world_size);
        }
    }

    */
}
