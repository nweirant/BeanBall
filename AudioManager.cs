using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }


    public Sound[] sounds;

   
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); ;
        s.source.Pause();
    }

    public void Mute(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); ;
        s.source.volume = 0;
    }

    public void UnMute(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); ;
        s.source.volume = 0.4f;
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.looping;
        }
    }
}
