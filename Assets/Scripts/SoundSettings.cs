/*
* File Name: SoundSettings.cs
* Author: Emilie Tee Jing Hui
* Date Created: 3/2/2026
* Description: Manages the sound settings in the menu UI
*/
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;

    /// <summary>
    /// Sets the BGM (background music) volume level in the audio mixer.
    /// </summary>
    /// <param name="volume">The volume level to set for BGM.</param>
    public void SetBGMVolume(float volume)
    {
        bgmGroup.audioMixer.SetFloat("BGMVolume", volume);
    }

    /// <summary>
    /// Sets the SFX (sound effects) volume level in the audio mixer.
    /// </summary>
    /// <param name="volume">The volume level to set for SFX.</param>
    public void SetSFXVolume(float volume)
    {
        sfxGroup.audioMixer.SetFloat("SFXVolume", volume);
    }
}

