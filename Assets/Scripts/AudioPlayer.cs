using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]
    AudioClip[] audioClips;
    AudioClip currentClip; 

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudioClip(string clipName)
    {
        foreach (var clip in audioClips)
        {
            if (clip.name == clipName)
            {
                currentClip = clip;
                audioSource.clip = currentClip;
                audioSource.Play();
                return;
            }
        }
        Debug.LogWarning($"Audio clip '{clipName}' not found!");
    }
    public void StopAudio()
    {
        audioSource.Stop();
    }
}
