/*
* File Name: PegBehavior.cs
* Author: Jayson Lim En Xu
* Date Created: 23/01/2026
* Description: Manages the behavior of a peg in the game.
*/
using System;
using UnityEditor;
using UnityEngine;

public class PegBehavior : MonoBehaviour
{
    /// <summary>
    /// Progress of the peg being driven into the ground.
    /// </summary>
    public float progress = 0f;
    /// <summary>
    /// Anchor point of the peg to adjust its position.
    /// </summary>
    public GameObject pegAnchorPoint;
    /// <summary>
    /// Event triggered when the peg is driven.
    /// </summary>
    public static event Action pegDriven;

    /// <summary>
    /// Drives the peg further into the ground.
    /// </summary>
    public void DrivePeg()
    {
        if (progress < 3f)
        {
            progress += 1f;
            Change();
            pegDriven?.Invoke();
            gameObject.GetComponent<AudioPlayer>()?.PlayAudioClip("hammer");
        }
        else
        {
            Debug.Log("Peg is fully driven down.");
        }
    }
    /// <summary>
    /// Updates the peg's position based on the current progress.
    /// </summary>
    private void Change()
    {
        Debug.Log("Peg progress: " + progress);
        if (progress == 1f)
        {
            
            pegAnchorPoint.transform.position = pegAnchorPoint.transform.position + new Vector3(0f, -0.05f, 0f);
        }
        else if (progress == 2f)
        {
            
            pegAnchorPoint.transform.position = pegAnchorPoint.transform.position + new Vector3(0f, -0.05f, 0f);
        }
        else if (progress >= 3f)
        {
            
            pegAnchorPoint.transform.position = pegAnchorPoint.transform.position + new Vector3(0f, -0.05f, 0f);
        }
    }
}
