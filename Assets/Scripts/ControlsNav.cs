using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsNav : MonoBehaviour
{
    [SerializeField]
    private GameObject first_screen;

    [SerializeField]
    private GameObject controls_screen;

    public void GoToControlsScreen()
    {
        first_screen.SetActive(false);
        controls_screen.SetActive(true);
    }

    public void GoToFirstScreen()
    {
        controls_screen.SetActive(false);
        first_screen.SetActive(true);
    }
}
