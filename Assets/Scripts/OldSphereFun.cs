using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class OldSphereFun : MonoBehaviour
{
    private float MoveSpeed = 8.0f;
    private bool debug_on = false;

    private Text t;
    private GameObject world;

    private float my_height;
    private float world_height;

    private Vector3 start_pos;
    private Quaternion start_rot;

    private GameObject debug_spot;

    private Vector3 radius_spot;
    private Vector3 movement_addition;

    private Vector3 last_rot;
    private Vector3 curr_rot;

    private bool swapped;

    private void Start()
    {
        t = GameObject.Find("Text").GetComponent<Text>();
        world = GameObject.Find("Sphere");

        my_height = transform.localScale.y / 2.0f;
        world_height = world.transform.localScale.y / 2.0f;

        start_pos = transform.position;
        start_rot = transform.rotation;

        if (debug_on)
        {
            debug_spot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debug_spot.transform.localScale *= 0.25f;
            debug_spot.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.yellow);
            debug_spot.name = "debug_spot";
        }
        else
        {
            t.text = "";
        }

        radius_spot = Vector3.zero;
        movement_addition = Vector3.zero;

        last_rot = transform.rotation.eulerAngles;
        curr_rot = transform.rotation.eulerAngles;

        swapped = false;
    }

    private void Update()
    {
        transform.LookAt(world.transform);
        RadiusSet();
        SwapFix();
        Movement();
        transform.position = radius_spot + movement_addition;
        if (Input.GetKeyDown(KeyCode.Space)) 
        { 
            transform.position = start_pos;
            transform.rotation = start_rot;
        }
    }

    private void FixedUpdate() {}

    private void SwapFix()
    {
        last_rot = curr_rot;
        curr_rot = transform.rotation.eulerAngles;

        if (debug_on)
        {
            Debug.Log("last_rot: " + last_rot + " | curr_rot: " + curr_rot);
        }

        float result = Mathf.Abs(curr_rot.y - last_rot.y);
        
        if (result > 170.0f && result < 190.0f)
        {
            Debug.Log("SWAP!!!");
            swapped = !swapped;
        }
    }

    private void RadiusSet()
    {
        Vector3 p = transform.position;
        float r = Mathf.Sqrt(Mathf.Pow(p.x, 2.0f) + Mathf.Pow(p.y, 2.0f) + Mathf.Pow(p.z, 2.0f));
        if (r == world_height) { return; }
        bool inside = r < world_height;

        int mask = inside ? 1 << 11 : 1 << 10;
        Vector3 direction = inside ? transform.forward * -1.0f : transform.forward;
        
        RaycastHit rch;
        bool hit = Physics.Raycast(transform.position, direction, out rch, Mathf.Infinity, mask);
        if (hit)
        {
            if (debug_on)
            {
                t.text = "Raycast\n" + rch.collider.gameObject.name + "\n" + Mathf.Round(rch.distance * 100.0f) / 100.0f + "m";
                debug_spot.transform.position = rch.point;
            }
            radius_spot = rch.point;
        }
    }

    private void Movement()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        float XFACTOR = swapped ? -1.0f : 1.0f;
        Vector3 move = (transform.right * input.x) + (transform.up * input.y * XFACTOR);
        move *= Time.deltaTime;
        move *= MoveSpeed;

        movement_addition = move;
    }
}
