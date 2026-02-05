/*
* File Name: OptionsMenu.cs
* Author: Emilie Tee Jing Hui
* Date Created: 3/2/2026
* Description: Manages the sound settings in the menu UI
*
* Last Edited By: Lim En Xu Jayson
* Last Edited: 4/2/2026
*/
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Toggle amsToggle;
    
    
    public void Start()
    {
        bgmSlider.value = GameManager.Instance.currentUser.music;
        sfxSlider.value = GameManager.Instance.currentUser.sfx;
        amsToggle.isOn = GameManager.Instance.currentUser.antiMotionSickness;
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Sets the BGM (background music) volume level in the audio mixer.
    /// </summary>
    public void SetBGMVolume()
    {
        bgmGroup.audioMixer.SetFloat("BGM", bgmSlider.value);
        GameManager.Instance.currentUser.music = (int)bgmSlider.value;
    }
    

    /// <summary>
    /// Sets the SFX (sound effects) volume level in the audio mixer.
    /// </summary>
    /// <param name="volume">The volume level to set for SFX.</param>
    public void SetSFXVolume()
    {
        sfxGroup.audioMixer.SetFloat("SFX", sfxSlider.value);
        GameManager.Instance.currentUser.sfx = (int)sfxSlider.value;
    }

    public void ToggleAMSMode()
    {
        GameManager.Instance.currentUser.antiMotionSickness = amsToggle.isOn;
    }
    
}

