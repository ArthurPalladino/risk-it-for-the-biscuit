using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    List<AudioSource> audioSources = new List<AudioSource>();
    public static AudioManager Instance;
    [SerializeField] float volume = 0.5f;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(Audio clip)
    {
        var curPlayClip = GetAudioSource(clip);
        if (curPlayClip == null)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
            audioSources.Add(audioSource);
            audioSource.clip = clip.Clip;
            audioSource.loop = clip.Loop;
            audioSource.Play();
        }
        else
        {
            curPlayClip.Play();
        }
    }
    public void Resume(Audio clip)
    {
        var audioSource = GetAudioSource(clip);
        if (audioSource != null)
        {
            audioSource.UnPause();
        }
        else
        {
            Play(clip);
        }
    }
    public void Pause(Audio clip)
    {
        var audioSource = GetAudioSource(clip);
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }

    public void StopAudio(Audio clip)
    {
        var audioSource = GetAudioSource(clip);
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSources.Remove(audioSource);
        }
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = volume;
        }
    }   
    
    AudioSource GetAudioSource(Audio clip)
    {
        return audioSources.Find(c => c.clip == clip.Clip);
    }
}
