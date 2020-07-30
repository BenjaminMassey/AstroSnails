using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    private float run_speed = 45.0f;
    private float turn_speed = 125.0f;
    private float jump_amount = 0.20f;
    private float fall_speed = 0.30f;
    private float max_fly = 100.0f;
    private float fly_cost = 1.0f;
    private float fly_gain = 1.0f;

    private float curr_fly;

    private bool gravity_on;
    private bool jumpable;

    private GameObject player;
    private float player_local_z_start;

    private Text t;
    private Slider s;

    // Start is called before the first frame update
    void Start()
    {
        gravity_on = true;
        jumpable = true;
        curr_fly = max_fly;
        player = transform.GetChild(0).transform.gameObject;
        player_local_z_start = player.transform.localPosition.z;
        t = GameObject.Find("Text").GetComponent<Text>();
        t.text = "";
        s = GameObject.Find("Slider").GetComponent<Slider>();
        StartCoroutine("FlyRegen");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.running) { return; }

        transform.Rotate(/*Input.GetAxis("Vertical") * */Time.deltaTime * run_speed * -1.0f,
                         0.0f,
                         Input.GetAxis("Horizontal") * Time.deltaTime * turn_speed);

        //t.text = player.transform.localPosition.ToString();

        if (jumpable && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("Jump");
        }

        if (player.transform.localPosition.z > player_local_z_start)
        {
            if (gravity_on)
            {
                RaiseLowerPlayer(-1.0f * fall_speed * Time.deltaTime);
            }
        }
        else
        {
            jumpable = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void RaiseLowerPlayer(float amount)
    {
        Vector3 lp = player.transform.localPosition;
        player.transform.localPosition = new Vector3(lp.x, lp.y, lp.z + amount);
    }

    IEnumerator Jump()
    {
        jumpable = false;
        gravity_on = false;
        float seconds = 0.5f;
        float iters = 50 * seconds;
        for (int i = 0; i < iters; i++)
        {
            RaiseLowerPlayer(jump_amount / iters);
            yield return new WaitForFixedUpdate();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine("Fly");
        }
        else
        {
            gravity_on = true;
        }
    }

    IEnumerator Fly()
    {
        while (Input.GetKey(KeyCode.Space) && curr_fly > 0.0f)
        {
            curr_fly -= fly_cost;
            yield return new WaitForFixedUpdate();
        }
        gravity_on = true;
    }

    IEnumerator FlyRegen()
    {
        while (Globals.running)
        {
            //t.text = curr_fly.ToString();
            s.value = curr_fly / max_fly;
            float diff = max_fly - curr_fly;
            if (diff > 0.0f)
            {
                curr_fly += Mathf.Min(diff, fly_gain);
            }
            float iters = 50 * 0.1f;
            for (int i = 0; i < iters; i++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

}
