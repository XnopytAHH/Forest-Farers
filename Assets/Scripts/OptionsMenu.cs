/*
* File Name: OptionsMenu.cs
* Author: Emilie Tee Jing Hui
* Date Created: 3/2/2026
* Description: Manages the sound settings in the menu UI
*
* Last Edited By: Lim En Xu Jayson
* Last Edited: 4/2/2026
*/

using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;
    public Slider[] bgmSlider;
    public Slider[] sfxSlider;
    public Toggle[] amsToggle;

    void Start()
    {
        if (gameObject.CompareTag("OptionsMenu"))
        {
            gameObject.SetActive(false);
        }
    }
    void OnEnable()
    {
        updateOptionsMenu();
    }
    /// <summary>
    /// Sets the BGM (background music) volume level in the audio mixer.
    /// </summary>
    public void SetBGMVolume(Slider currentSlider)
    {
        bgmGroup.audioMixer.SetFloat("BGM", currentSlider.value);
        GameManager.Instance.currentUser.music = (int)currentSlider.value;
        foreach (Slider slider in bgmSlider)
        {
            if (slider != null)
            slider.value = currentSlider.value;
        }
        
    }
    

    /// <summary>
    /// Sets the SFX (sound effects) volume level in the audio mixer.
    /// </summary>
    /// <param name="volume">The volume level to set for SFX.</param>
    public void SetSFXVolume(Slider currentSlider)
    {
        sfxGroup.audioMixer.SetFloat("SFX", currentSlider.value);
        GameManager.Instance.currentUser.sfx = (int)currentSlider.value;
        foreach (Slider slider in sfxSlider)
        {
            if (slider != null)
            slider.value = currentSlider.value;
        }
        
    }

    public void ToggleAMSMode(Toggle currentToggle)
    {
        GameManager.Instance.currentUser.antiMotionSickness = currentToggle.isOn;
        foreach (Toggle toggle in amsToggle)
        {
            if (toggle == null) continue;
            toggle.isOn = GameManager.Instance.currentUser.antiMotionSickness;
        }
    }
    public void CloseOptionsMenu()
    {
        gameObject.SetActive(false);
    }
    public void OpenOptionsMenu()
    {
        gameObject.SetActive(true);
    }
    public void AddHeight()
    {
        GameManager.Instance.currentUser.height += 1;
        foreach (GameObject text in GameObject.FindGameObjectsWithTag("Height"))
        {
            text.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.currentUser.height.ToString() + " cm";
        }
    }
    public void SubtractHeight()
    {
        GameManager.Instance.currentUser.height -= 1;
        foreach (GameObject text in GameObject.FindGameObjectsWithTag("Height"))
        {
            text.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.currentUser.height.ToString() + " cm";
        }
    }
    public void updateOptionsMenu()
    {
        
        sfxSlider = new Slider[2];
        bgmSlider = new Slider[2];
        amsToggle = new Toggle[2];
        foreach (GameObject toggle in GameObject.FindGameObjectsWithTag("AMS"))
        {
            amsToggle[amsToggle.ToList().FindIndex(x => x == null)] = toggle.GetComponent<Toggle>();
        }
        if (GameObject.FindGameObjectWithTag("MenuBGM")!= null)
        {
            sfxSlider[0] = GameObject.FindGameObjectWithTag("MenuSFX").GetComponent<Slider>();
            bgmSlider[0] = GameObject.FindGameObjectWithTag("MenuBGM").GetComponent<Slider>();
        }
        if (GameObject.FindGameObjectWithTag("BookBGM")!= null)
        {
            sfxSlider[1] = GameObject.FindGameObjectWithTag("BookSFX").GetComponent<Slider>();
            bgmSlider[1] = GameObject.FindGameObjectWithTag("BookBGM").GetComponent<Slider>();
        }
        foreach (Slider slider in bgmSlider)
        {
            
            if (slider != null)
            slider.value = GameManager.Instance.currentUser.music;
            
        }
        foreach (Slider slider in sfxSlider)
        {
            if (slider != null) 
            slider.value = GameManager.Instance.currentUser.sfx;
        }
        foreach (Toggle toggle in amsToggle)
        {
            if (toggle != null)
            toggle.isOn = GameManager.Instance.currentUser.antiMotionSickness;
        }
        foreach (TextMeshProUGUI text in GameObject.FindGameObjectsWithTag("Height").Select(obj => obj.GetComponent<TextMeshProUGUI>()))
        {
            text.text = GameManager.Instance.currentUser.height.ToString() + " cm";
        }
        foreach (GameObject text in GameObject.FindGameObjectsWithTag("Height"))
        {
            text.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.currentUser.height.ToString() + " cm";
        }
    }
    
}

