using UnityEngine;
using TMPro;
using UnityEditorInternal;
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

