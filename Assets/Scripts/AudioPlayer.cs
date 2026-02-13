/*
* File Name: AudioPlayer.cs
* Author: Lim En Xu Jayson
* Date Created: 08/02/2026
* Description: Audio Player class to handle playing audio clips.
*/
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    /// <summary>
    /// Audio Source component for playing audio clips.
    /// </summary>
    AudioSource audioSource;
    /// <summary>
    /// Array of audio clips available for playback.
    /// </summary>
    [SerializeField]
    AudioClip[] audioClips;
    /// <summary>
    /// Currently playing audio clip.
    /// </summary>
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
