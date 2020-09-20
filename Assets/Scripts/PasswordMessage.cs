using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Globals.wrong_password)
        {
            GetComponent<Text>().enabled = true;
        }
    }
}
