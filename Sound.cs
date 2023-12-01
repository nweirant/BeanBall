using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public float volume;

    public bool looping;

    [HideInInspector]
    public AudioSource source;

}
