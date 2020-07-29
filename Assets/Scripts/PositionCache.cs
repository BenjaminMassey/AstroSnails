using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCache : MonoBehaviour
{
    public List<Vector3> data;
    // Start is called before the first frame update
    void Start()
    {
        data = new List<Vector3>();
        StartCoroutine("Cacher");
    }

    IEnumerator Cacher()
    {
        while (Globals.running)
        {
            data.Add(transform.position);

            float iter = 50 * Globals.collider_time;
            for (int i = 0; i < iter; i++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
