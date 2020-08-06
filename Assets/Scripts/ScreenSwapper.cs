using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwapper : MonoBehaviour
{
    [SerializeField]
    private GameObject first_screen;
    [SerializeField]
    private GameObject second_screen;
    [SerializeField]
    private GameObject third_screen;

    public void ActivateFirst()
    {
        first_screen.SetActive(true);
        second_screen.SetActive(false);
        third_screen.SetActive(false);
    }

    public void ActivateSecond()
    {
        first_screen.SetActive(false);
        second_screen.SetActive(true);
        third_screen.SetActive(false);
    }

    public void ActivateThird()
    {
        first_screen.SetActive(false);
        second_screen.SetActive(false);
        third_screen.SetActive(true);
    }
}
