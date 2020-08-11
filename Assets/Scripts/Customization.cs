using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customization : MonoBehaviour
{
    [SerializeField]
    private GameObject points_left_obj;
    [SerializeField]
    private GameObject speed_levels_obj;
    [SerializeField]
    private GameObject turn_levels_obj;
    [SerializeField]
    private GameObject cap_levels_obj;
    [SerializeField]
    private GameObject regen_levels_obj;

    private PlayerHandler my_player;

    private int point_max = 5;

    private Dictionary<string, GameObject> level_objs;

    // Start is called before the first frame update
    void Start()
    {
        my_player = transform.parent.parent.GetComponent<PlayerHandler>();

        if (Globals.customization_points == null)
        {
            Globals.customization_points = new Dictionary<string, int>();
            Globals.customization_points.Add("speed", 0);
            Globals.customization_points.Add("turn", 0);
            Globals.customization_points.Add("cap", 0);
            Globals.customization_points.Add("regen", 0);
        }

        level_objs = new Dictionary<string, GameObject>();
        level_objs.Add("speed", speed_levels_obj);
        level_objs.Add("turn", turn_levels_obj);
        level_objs.Add("cap", cap_levels_obj);
        level_objs.Add("regen", regen_levels_obj);

        UpdatePointsLeftText();
        UpdateLevels("speed");
        UpdateLevels("turn");
        UpdateLevels("cap");
        UpdateLevels("regen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void point_up(string key)
    {
        if (Globals.customization_points[key] < point_max && Globals.points_left > 0)
        {
            Globals.customization_points[key]++;
            UpdateLevels(key);

            Globals.points_left--;
            UpdatePointsLeftText();

            if (key.Equals("speed")) { my_player.run_speed += Globals.speed_amount; }
            if (key.Equals("turn")) { my_player.turn_speed += Globals.turn_amount; }
            if (key.Equals("cap")) { my_player.max_fly += Globals.cap_amount; }
            if (key.Equals("regen")) { my_player.fly_gain += Globals.regen_amount; }
        }
    }

    public void point_down(string key)
    {
        if (Globals.customization_points[key] > 0)
        {
            Globals.customization_points[key]--;
            UpdateLevels(key);

            Globals.points_left++;
            UpdatePointsLeftText();

            if (key.Equals("speed")) { my_player.run_speed -= Globals.speed_amount; }
            if (key.Equals("turn")) { my_player.turn_speed -= Globals.turn_amount; }
            if (key.Equals("cap")) { my_player.max_fly -= Globals.cap_amount; }
            if (key.Equals("regen")) { my_player.fly_gain -= Globals.regen_amount; }
        }
    }

    private void UpdateLevels(string key) 
    {
        //Debug.Log("Customization.UpdateLevels(" + key + ")");
        level_objs[key].GetComponent<CustomLevelDisplay>().Display(Globals.customization_points[key]);
    }

    private void UpdatePointsLeftText()
    {
        //Debug.Log("Customization.UpdatePointsLeftText()");
        points_left_obj.GetComponent<Text>().text = "Points Left: " + Globals.points_left.ToString();
    }
}
