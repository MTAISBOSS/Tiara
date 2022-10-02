using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHolder : MonoBehaviour
{
    [SerializeField] private List<AudioData> sounds = new List<AudioData>();

    public static AudioHolder Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Play(string noteName)
    {
        var s = sounds.Find(o => o.name == noteName);
        if (s == null)
            return;
        // s.source.Play();
        Debug.Log(s.name);
        
    }
}

[Serializable]
public class AudioData
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1f;

    [Range(-3, 3)] public float pitch = 1f;
    [HideInInspector] public AudioSource source;
}