using UnityEngine;
using TMPro;
public class TempScoreUpdater : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Total Score: " + GameManager.Instance.endDayScores[0] + "\n" +
           "Cooking Badge: " + GameManager.Instance.endDayScores[1] + "\n" +
           "Tent Badge: " + GameManager.Instance.endDayScores[2] + "\n" +
           "Fishing Badge: " + GameManager.Instance.endDayScores[3] + "\n" +
           "Campfire Badge: " + GameManager.Instance.endDayScores[4]; 
    }

}
