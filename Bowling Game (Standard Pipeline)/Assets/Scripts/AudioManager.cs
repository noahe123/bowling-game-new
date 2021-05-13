using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = null;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.spatialBlend = s.spatialBlend;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();

        //audio replay stuff
        if (s.recordSound)
        {
            if (GetComponent<TimeBody>() != null)
            {
                GetComponent<TimeBody>().audioName = name;
                GetComponent<TimeBody>().audioManager = GetComponent<AudioManager>();
                GetComponent<TimeBody>().audioVol = s.source.volume;
                GetComponent<TimeBody>().audioPlay = true;
            }
        }
    }

    public void Play(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = volume;
        s.source.Play();

        //audio replay stuff
        if (s.recordSound)
        {
            if (GetComponent<TimeBody>() != null)
            {
                GetComponent<TimeBody>().audioName = name;
                GetComponent<TimeBody>().audioManager = GetComponent<AudioManager>();
                GetComponent<TimeBody>().audioVol = s.source.volume;
                GetComponent<TimeBody>().audioPlay = true;
            }
        }
    }

    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();


        //audio replay stuff
        if (s.recordSound)
        {
            if (GetComponent<TimeBody>() != null)
            {
                GetComponent<TimeBody>().audioName = name;
                GetComponent<TimeBody>().audioManager = GetComponent<AudioManager>();
                GetComponent<TimeBody>().audioStop = true;
            }
        }
    }
}
