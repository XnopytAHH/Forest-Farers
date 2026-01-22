/*
* File Name: CookingBehaviour.cs
* Author: Katriel Wong Shu Ning
* Date Created: 22/01/2026
* Description: Cooking behaviour including cooking timer.
*/
using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class CookingBehaviour : MonoBehaviour
{
    /// <summary>
    /// Total cooking time in seconds.
    /// </summary>
    [SerializeField]
    float cookingTime = 10f;

    /// <summary>
    /// Remaining cooking time in seconds.  
    /// </summary>
    [SerializeField]
    float remainingTime = 10f;

    /// <summary>
    /// Reference to the cooking timer UI text.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI cookingTimer;

    private WaitForSeconds waitOneSecond = new WaitForSeconds(1f);

    public void cookingCountdown()
    {
        if (cookingTimer == null)
        {
            Debug.LogError("Cooking timer UI text is not assigned!");
            return;
        }
        remainingTime = cookingTime;
        StartCoroutine(CookingTimer());
    }

    IEnumerator CookingTimer()
    {
        while (remainingTime > 0)
        {
            cookingTimer.text = remainingTime.ToString("F1");
            yield return waitOneSecond;
            remainingTime -= 1f;
        }
        cookingTimer.text = "0.0";
    }

    public void stopCookingTimer()
    {
        StopCoroutine(CookingTimer());
        cookingTimer.text = "";
    }
}