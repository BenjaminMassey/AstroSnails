using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLevelDisplay : MonoBehaviour
{
    List<GameObject> levels;

    // Start is called before the first frame update
    void Start()
    {
        levels = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            levels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void Display(int num)
    {
        for (int i = 0; i < num; i++)
        {
            levels[i].GetComponent<Image>().color = Color.yellow;
        }
        for (int i = num; i < levels.Count; i++)
        {
            levels[i].GetComponent<Image>().color = Color.white;
        }
    }
}
