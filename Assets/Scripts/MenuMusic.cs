using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    [SerializeField]
    AudioSource menu_audio_source;
    
    void Awake()
    {
        menu_audio_source.loop = true;
        menu_audio_source.Play();
    }
}
