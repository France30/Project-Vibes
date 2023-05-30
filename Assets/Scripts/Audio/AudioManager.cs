using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{ 
    [SerializeField] private Sound[] _sounds;

    protected override void Awake()
    {
        base.Awake();
        foreach(Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found!");
            return false;
        }
        return s.source.isPlaying;
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
}
