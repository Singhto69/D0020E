using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        //Allowing for consistency across scenes
        if(instance != null) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);
        instance = this;

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            
            sound.source.loop = sound.loop;
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }

        Play("Theme");
    }

    void Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Tried to play sound but can't find - " + name + " - in sounds array");
            return;
        }

        sound.source.Play();
    }

    void Stop(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Tried to play sound but can't find - " + name + " - in sounds array");
            return;
        }

        sound.source.Stop();
    }
}
