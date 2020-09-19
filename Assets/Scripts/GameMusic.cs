using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    [SerializeField]
    AudioSource[] game_music_sources;

    void Awake()
    {
        int choice = Random.Range(0, game_music_sources.Length);
        game_music_sources[choice].loop = true;
        game_music_sources[choice].Play();
    }
}
