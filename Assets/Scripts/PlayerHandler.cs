using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Linq;

public class PlayerHandler : MonoBehaviourPun
{
    public float player_local_z_start;
    //public Quaternion start_rot;
    public Vector3 start_rot;

    private float run_speed = 45.0f;
    private float turn_speed = 125.0f;
    private float jump_amount = 0.20f;
    private float fall_speed = 0.30f;
    private float max_fly = 100.0f;
    private float fly_cost = 1.0f;
    private float fly_gain = 1.0f;
    private float boost_speed = 3.0f;
    private float boost_time = 1.5f;
    private float boost_wait = 3.0f;

    private float curr_fly;

    private bool gravity_on;
    private bool jumpable;
    private bool boosting;

    private bool rot_set;

    private GameObject player;

    //private Text t;
    private Slider fly_slider;
    private Text boost_text;
    private TrailRenderer trail;

    // Start is called before the first frame update
    void Start()
    {
        gravity_on = true;
        jumpable = true;
        boosting = false;
        rot_set = false;
        curr_fly = max_fly;
        player = transform.GetChild(0).transform.gameObject;
        player_local_z_start = player.transform.localPosition.z;
        //t = GameObject.Find("Text").GetComponent<Text>();
        //t.text = "";
        //fly_slider = GameObject.Find("FlySlider").GetComponent<Slider>();
        Transform fly_slider_obj = transform.Find("Canvas").Find("FlySlider");
        if (fly_slider_obj != null)
        {
            fly_slider = fly_slider_obj.GetComponent<Slider>();
        }
        //boost_text = GameObject.Find("BoostText").GetComponent<Text>();
        Transform boost_text_obj = transform.Find("Canvas").Find("BoostText");
        if (boost_text_obj != null)
        {
            boost_text = boost_text_obj.GetComponent<Text>();
            boost_text.text = "READY";
        }
        trail = transform.GetChild(0).GetComponent<TrailRenderer>();
        // SHOUDL BE SET BY GAMESETUP start_rot = Vector3.zero;
        //start_rot = transform.localRotation;
        StartCoroutine("FlyRegen");

        if (!photonView.IsMine)
        {
            transform.Find("Canvas").gameObject.SetActive(false);
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("player_num"))
            {
                int playerNum = (int)PhotonNetwork.LocalPlayer.CustomProperties["player_num"];
                player.name = "Player " + playerNum;
                name = "Player " + playerNum + " Container";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!photonView.gameObject.transform.GetChild(0).name.Equals(player.name))
        {
            return;
        }
        */

        if (!photonView.IsMine) { return; }

        if (!Globals.running) 
        {
            if (rot_set) { return; }
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("player_num"))
            {
                float[] spots = new float[] { 0.0f, 90.0f, 180.0f, 270.0f };
                int index = (int) PhotonNetwork.LocalPlayer.CustomProperties["player_num"] - 1;
                float my_y = spots[index];
                transform.rotation = Quaternion.identity;
                transform.Rotate(new Vector3(0.0f, my_y, 0.0f));
                //transform.rotation.eulerAngles.Set(0.0f, my_y, 0.0f);
            }
            else
            {
                int playerNum = PhotonNetwork.CurrentRoom.PlayerCount;
                ExitGames.Client.Photon.Hashtable properties =
                        new ExitGames.Client.Photon.Hashtable();/*
                        { 
                            { "running", false },
                            { "player_num", (int) PhotonNetwork.CurrentRoom.PlayerCount }
                        };*/
                properties.Add("running", false);
                properties.Add("player_num", playerNum);

                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

                player.name = "Player " + playerNum;
                name = "Player " + playerNum + " Container";

                Debug.Log("Assigned player num: " + playerNum);
            }
            /*transform.rotation = Quaternion.identity;
            transform.Rotate(start_rot);*/
            //trail.Clear();
            //transform.rotation.eulerAngles.Set(start_rot.x, start_rot.y, start_rot.z);
            return;
        }
        rot_set = true;

        transform.Rotate(/*Input.GetAxis("Vertical") * */Time.deltaTime * run_speed * -1.0f,
                         0.0f,
                         Input.GetAxis("Horizontal") * Time.deltaTime * turn_speed);

        //t.text = player.transform.localPosition.ToString();

        if (jumpable && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("Jump");
        }

        if (!boosting && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine("Boost");
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
        while (true)
        {
            if (Globals.running)
            {
                if (fly_slider != null)
                {
                    fly_slider.value = curr_fly / max_fly;
                }

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
            else
            {
                int check_time = 25;
                for (int i = 0; i < check_time; i++)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }

    IEnumerator Boost()
    {
        boosting = true;

        Globals.collider_time = 0.05f;

        if (boost_text != null)
        {
            boost_text.text = "Boosting!";
        }

        float orig_speed = run_speed;
        run_speed *= boost_speed;

        float time_iters = 50 * boost_time;
        for (int i = 0; i < time_iters; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        run_speed = orig_speed;

        if (boost_text != null)
        {
            boost_text.text = "Recharging...";
        }

        float wait_iters = 50 * boost_wait;
        for (int i = 0; i < wait_iters; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        if (boost_text != null)
        {
            boost_text.text = "READY";
        }

        Globals.collider_time = 0.1f;

        boosting = false;
        
    }

    public void ResetFly()
    {
        curr_fly = max_fly;
    }
}
