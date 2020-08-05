using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputEnter : MonoBehaviour
{
    public GameObject button_obj = null;

    private Button b;

    private string for_syd = "bum";
    // Start is called before the first frame update
    void Start()
    {
        if (button_obj == null)
        {
            Destroy(this);
        }
        b = button_obj.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            b.onClick.Invoke();
        }
    }
}
