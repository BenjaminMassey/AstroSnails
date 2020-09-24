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
using Photon.Realtime;

public class PlayerHandler : MonoBehaviourPun
{
    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private AudioSource jump_audio_source;
    [SerializeField]
    private AudioSource fly_audio_source;
    [SerializeField]
    private AudioSource boost_audio_source;
    [SerializeField]
    private AudioSource boost_rev_audio_source;
    [SerializeField]
    private GameObject steam_effect;

    public float player_local_z_start;
    //public Quaternion start_rot;
    public Vector3 start_rot;

    // public ones are for Customization.cs
    public float run_speed;
    public float turn_speed;
    private float jump_amount = 0.10f;
    private float fall_speed = 0.15f;
    public float max_fly;
    private float fly_cost = 1.0f;
    public float fly_gain;
    private float boost_speed = 2.0f;
    private float boost_time = 1.5f;
    private float boost_wait = 3.0f;
    private float jump_buffer_percent = 0.1f; // can toggle a jump for last 10% of prev jump

    private float curr_fly;

    private bool gravity_on;
    private bool jumpable;
    private bool boosting;
    private bool jump_buffer;

    private bool rot_set;

    private GameObject player;

    //private Text t;
    private Slider fly_slider;
    private Text boost_text;
    private TrailRenderer trail;
    private Death death;

    // Start is called before the first frame update
    void Start()
    {
        // Customizible trait starting values
        run_speed = 20.0f;
        turn_speed = 40.0f;
        max_fly = 50.0f;
        fly_gain = 1.0f;

        if (Globals.customization_points != null)
        { 
            run_speed += (Globals.speed_amount * Globals.customization_points["speed"]);
            turn_speed += (Globals.turn_amount * Globals.customization_points["turn"]);
            max_fly += (Globals.cap_amount * Globals.customization_points["cap"]);
            fly_gain += (Globals.speed_amount * Globals.customization_points["regen"]);
        }

        gravity_on = true;
        jumpable = true;
        boosting = false;
        jump_buffer = false;
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

        death = transform.GetChild(0).GetComponent<Death>();
        // SHOUDL BE SET BY GAMESETUP start_rot = Vector3.zero;
        //start_rot = transform.localRotation;
        StartCoroutine("FlyRegen");

        if (!photonView.IsMine)
        {
            //transform.Find("Canvas").gameObject.SetActive(false);
            Destroy(transform.Find("Canvas").gameObject);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable properties = 
                PhotonNetwork.LocalPlayer.CustomProperties;
            if (properties.ContainsKey("player_name"))
            {
                player.name = (string)properties["player_name"];
                name = ((string)properties["player_name"]) + " Container";
            }
            else
            {
                if (properties.ContainsKey("player_num"))
                {
                    int playerNum = (int)PhotonNetwork.LocalPlayer.CustomProperties["player_num"];
                    player.name = "Player " + playerNum;
                    name = "Player " + playerNum + " Container";
                }
            }
        }

        StartCoroutine("SetVisual");
    }

    IEnumerator SetVisual()
    {
        bool done = false;
        while (!done)
        {
            if (photonView.Owner.CustomProperties.ContainsKey("player_num"))
            {
                int index = (int)photonView.Owner.CustomProperties["player_num"] - 1;
                if (materials.Length >= 4)
                {
                    GameObject model = player.transform.Find("ModelContainer").Find("Model").gameObject;
                    model.GetComponent<MeshRenderer>().material = materials[index];
                    trail.material = materials[index];
                }
                done = true;
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
        Debug.Log("Player visual has been set");
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
                Vector3[] starting_rots = new Vector3[]
                {
                    new Vector3(0.0f, 0.0f, 0.0f),
                    new Vector3(180.0f, 0.0f, 0.0f),
                    new Vector3(0.0f, -90.0f, 0.0f),
                    new Vector3(0.0f, 90.0f, -180.0f)
                };
                int index = (int) PhotonNetwork.LocalPlayer.CustomProperties["player_num"] - 1;
                transform.rotation = Quaternion.identity;
                transform.Rotate(starting_rots[index]);
            }
            else
            {
                Player[] players = PhotonNetwork.PlayerList;
                List<int> player_nums = new List<int>();
                foreach (Player player in players)
                {
                    if (player.CustomProperties.ContainsKey("player_num"))
                    {
                        player_nums.Add((int)player.CustomProperties["player_num"]);
                    }
                }
                player_nums.Sort();

                int playerNum = 1;
                foreach (int num in player_nums)
                {
                    if (playerNum == num) { playerNum++; }
                    else { break; }
                }

                Debug.Log("player_num assigned of " + playerNum);

                //int playerNum = PhotonNetwork.CurrentRoom.PlayerCount;

                ExitGames.Client.Photon.Hashtable properties =
                        PhotonNetwork.LocalPlayer.CustomProperties;

                if (!properties.ContainsKey("running"))
                {
                    properties.Add("running", false);
                } 
                properties.Add("player_num", playerNum);

                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

                if (properties.ContainsKey("player_name"))
                {
                    player.name = (string)properties["player_name"];
                    name = ((string)properties["player_name"]) + " Container";
                }
                else 
                {
                    player.name = "Player " + playerNum;
                    name = "Player " + playerNum + " Container";
                }

                Debug.Log("Assigned player num: " + playerNum);
            }
            curr_fly = max_fly; // put here as well so gets reset based on customization

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

        if (jumpable && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0) || jump_buffer))
        {
            jump_buffer = false;
            StartCoroutine("Jump");
        }

        if (!boosting && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton2)))
        {
            StartCoroutine("Boost");
        }

        float z_diff = player.transform.localPosition.z - player_local_z_start;
        if (z_diff > 0.0f)
        {
            if (gravity_on)
            {
                RaiseLowerPlayer(-1.0f * fall_speed * Time.deltaTime);
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
                    && z_diff < (player_local_z_start * jump_buffer_percent))
                {
                    jump_buffer = true;
                    // Put jump buffer stuff here
                }
            }
        }
        else
        {
            jumpable = true;
        }
    }

    void RaiseLowerPlayer(float amount)
    {
        Vector3 lp = player.transform.localPosition;
        player.transform.localPosition = new Vector3(lp.x, lp.y, lp.z + amount);
    }

    IEnumerator Jump()
    {
        VarietyPlay(jump_audio_source);

        jumpable = false;
        gravity_on = false;
        float seconds = 0.3f;
        float iters = 50 * seconds;
        float short_hop_thresh_start = 0.6f; // can short hop this % of the way through
        float short_hop_thresh_end = 0.85f; // stuck in full jump after this % of the way through
        
        bool short_hop = false;
        int i = 0;
        while (i < iters)
        {
            // Allow for varying heights based on how long pressing
            if (i > iters * short_hop_thresh_start && i < iters * short_hop_thresh_end
                && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.JoystickButton0))
            {
                short_hop = true;
                break;
            }
            // Completely stop if this player is dead
            if (death.IsDead()) { yield break; }
            // Raise player by correct amount
            RaiseLowerPlayer(jump_amount / iters);
            // Wait + increment so that this all happens overtime
            yield return new WaitForFixedUpdate();
            i++;
        }
        Debug.Log("Reached height of " + (player.transform.localPosition.z - player_local_z_start));

        if (!short_hop && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton0)))
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
        VarietyPlay(fly_audio_source);
        //steam_effect.SetActive(true);
        while ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton0)) && curr_fly > 0.0f)
        {
            curr_fly -= fly_cost;
            yield return new WaitForFixedUpdate();
        }
        fly_audio_source.Stop();
        //steam_effect.SetActive(false);
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

                float z_diff = player.transform.localPosition.z - player_local_z_start;
                float diff = max_fly - curr_fly;
                if (diff > 0.0f && z_diff <= jump_amount * 0.1f) // && && z_diff == 0 means not recharging while flying/falling/rising
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
        VarietyPlay(boost_audio_source);

        boosting = true;

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
            if (i == time_iters - 15 && !death.IsDead())
            {
                VarietyPlay(boost_rev_audio_source);
            }
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

        boosting = false;
    }

    public void ResetFly()
    {
        curr_fly = max_fly;
    }

    private void VarietyPlay(AudioSource sfx)
    {
        sfx.pitch = UnityEngine.Random.Range(0.8f, 1.3f);
        sfx.Play();
    }
}
