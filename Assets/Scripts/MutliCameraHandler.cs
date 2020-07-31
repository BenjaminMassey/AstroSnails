using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutliCameraHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!transform.parent.parent.GetComponent<PlayerHandler>().photonView.IsMine)
        {
            Destroy(gameObject);
        }
    }
}
