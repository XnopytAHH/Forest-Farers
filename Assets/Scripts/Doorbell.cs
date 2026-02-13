/*
* File Name: Doorbell.cs
* Author: Andre Lim Zhe Kai
* Date Created: 08/02/2026
* Description: Handles the doorbell button functionality to skip to the end of the day.
*/
using UnityEngine;

public class Doorbell : MonoBehaviour
{
    private bool isPressed = false;
    public void SetCampfireButton()
    {
        if (isPressed) return;
        isPressed = true;
        gameObject.GetComponent<AudioPlayer>().PlayAudioClip("doorbell");
        CampRun.Instance.currentTime= CampRun.Instance.dayDuration -2f;
    }
}
