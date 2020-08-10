using Photon.Pun;
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

    private int point_max = 5;
    private int points_left_start = 10;

    private int points_left;

    private Dictionary<string, int> points;
    private Dictionary<string, GameObject> level_objs;

    // Start is called before the first frame update
    void Start()
    {
        points_left = points_left_start;
        UpdatePointsLeftText();

        points = new Dictionary<string, int>();
        points.Add("speed", 0);
        points.Add("turn", 0);
        points.Add("cap", 0);
        points.Add("regen", 0);

        level_objs = new Dictionary<string, GameObject>();
        level_objs.Add("speed", speed_levels_obj);
        level_objs.Add("turn", turn_levels_obj);
        level_objs.Add("cap", cap_levels_obj);
        level_objs.Add("regen", regen_levels_obj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void point_up(string key)
    {
        if (points[key] < point_max && points_left > 0)
        {
            points[key]++;
            level_objs[key].GetComponent<CustomLevelDisplay>().Display(points[key]);

            points_left--;
            UpdatePointsLeftText();
        }
    }

    public void point_down(string key)
    {
        if (points[key] > 0)
        {
            points[key]--;
            level_objs[key].GetComponent<CustomLevelDisplay>().Display(points[key]);

            points_left++;
            UpdatePointsLeftText();
        }
    }

    private void UpdatePointsLeftText()
    {
        points_left_obj.GetComponent<Text>().text = "Points Left: " + points_left.ToString();
    }
}
