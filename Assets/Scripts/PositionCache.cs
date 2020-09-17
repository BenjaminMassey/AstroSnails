using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCache : MonoBehaviour
{
    public List<Vector3> data;

    private bool first;

    // Start is called before the first frame update
    void Start()
    {
        data = new List<Vector3>();

        first = true;

        StartCoroutine("Cacher");
    }

    IEnumerator Cacher()
    {
        while (true)
        {
            float speed_coeff = 1.0f;
            if (Globals.running)
            {
                if (first)
                {
                    speed_coeff = transform.parent.GetComponent<PlayerHandler>().run_speed / 40.0f;
                    first = false;
                }

                data.Add(transform.position);

                float iter = (50 * Globals.collider_time) / speed_coeff;
                for (int i = 0; i < iter; i++)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
