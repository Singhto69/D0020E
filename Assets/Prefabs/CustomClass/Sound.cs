using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop;

    [Range(0f,1f)]
    public float volume = 0.1f;

    [Range(1f, 3f)]
    public float pitch = 1;

    [HideInInspector]
    public AudioSource source;
}
