/*
* File Name: ScoreUpdater.cs
* Author: Jayson Lim En Xu
* Date Created: 01/02/2026
* Description: Updates the score display in the game.
*/
using UnityEngine;
using TMPro;
public class ScoreUpdater : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       scoreText.text = $"Total Score: {GameManager.Instance.endDayScores[0]} pts";
    }
    public void ReturnToMainMenu()
    {
        TransitionManager.Instance.ChangeScene("MenuScene");
    }
}

