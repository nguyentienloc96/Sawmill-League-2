using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance = null;

    public Sound[] sounds;

    public Slider sliderSound;

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("Volume Sound"))
        {
            PlayerPrefs.SetFloat("Volume Sound", 1f);
        }

        float volume = PlayerPrefs.GetFloat("Volume Sound");

        foreach (Sound s in sounds)
        {
            s.volume = volume;
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }

    public void Start()
    {
        Play("Menu");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound :" + name + "not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound :" + name + "not found!");
            return;
        }
        s.source.Stop();
    }

    public void Mute(string name, bool mute)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound :" + name + "not found!");
            return;
        }
        s.source.mute = mute;
    }

    public void MuteAll(bool mute)
    {
        foreach (Sound s in sounds)
        {
            s.source.mute = mute;
        }
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void SliderVolume(Slider _slider)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = _slider.value;
        }

        PlayerPrefs.SetFloat("Volume Sound", _slider.value);
    }
}