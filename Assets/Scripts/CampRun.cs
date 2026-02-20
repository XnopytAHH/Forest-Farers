/*
* File Name: CampRun.cs
* Author: Lim En Xu Jayson
* Date Created: 02/02/2026
* Description: Handles the camp game loop including day-night cycle and task completion.
*/
using System;
using UnityEngine;

public class CampRun : MonoBehaviour
{
    /// <summary>
    /// Duration of the day in seconds.
    /// </summary>
    public float dayDuration = 600f; 
    /// <summary>
    /// Current time elapsed in the day.
    /// </summary>
    public float currentTime = 0f; 
    /// <summary>
    /// Directional light representing the sun.
    /// </summary>
    [SerializeField]
    GameObject sunSource;
    /// <summary>
    /// Skybox material for day-night transition.
    /// </summary>
    [SerializeField]
    Material skyboxMaterial;
    /// <summary>
    /// Threshold time to start night transition.
    /// </summary>
    [SerializeField]
    public float nightThreshold = 180f;
    /// <summary>
    /// Time when cooking task was finished.
    /// </summary>
    public float cookingFinishTime = 0f;
    /// <summary>
    /// Time when fishing task was finished.
    /// </summary>
    public float fishingFinishTime = 0f;
    /// <summary>
    /// Time when tent task was finished.
    /// </summary>
    public float tentFinishTime = 0f;
    /// <summary>
    /// Time when campfire task was finished.
    /// </summary>
    public float campfireFinishTime = 0f;
    /// <summary>
    /// Cooking badge earned by the player.
    /// </summary>
    public int cookingBadge = 0;
    /// <summary>
    /// Fishing badge earned by the player.
    /// </summary>
    public int fishingBadge = 0;
    /// <summary>
    /// Tent badge earned by the player.
    /// </summary>
    public int tentBadge = 0;
    /// <summary>
    /// Campfire badge earned by the player.
    /// </summary>
    public int campfireBadge = 0;
    /// <summary>
    /// Flag indicating if cooking task is finished.
    /// </summary>
    public bool cookingFinished = false;
    /// <summary>
    /// Flag indicating if fishing task is finished.
    /// </summary>
    public bool fishingFinished = false;
    /// <summary>
    /// Flag indicating if tent task is finished.
    /// </summary>
    public bool tentFinished = false;
    /// <summary>
    /// Flag indicating if campfire task is finished.
    /// </summary>
    public bool campfireFinished = false;
    /// <summary>
    /// Singleton instance of CampRun.
    /// </summary>
    public static CampRun Instance;
    /// <summary>
    /// Reference to the TentBehaviour script.
    /// </summary>
    public TentBehaviour tentBehaviour;
    /// <summary>
    /// Initializes the CampRun instance and sets up initial values.
    /// </summary>
    void Start()
    {
        Instance = this;
        skyboxMaterial.SetFloat("_Blend", 0f);
        currentTime = 0f;
        
    }
    /// <summary>
    /// Updates the day-night cycle and handles task completion timing.
    /// </summary>
    void Update()
    {
        if (currentTime <dayDuration)
        {
            currentTime += Time.deltaTime;
            LerpSunPosition();
        };

        
        if (currentTime > dayDuration - nightThreshold)
        {
            LerpSkyboxColor();
        }
    }
    /// <summary>
    /// Lerps the sun's position and intensity based on the current time.
    /// </summary>
    void LerpSunPosition()
    {
        Debug.Log("Lerping sun position. Current time: " + currentTime);
        float normalizedTime = (currentTime % dayDuration) / dayDuration; // Normalize time to [0, 1]
        float sunAngle = normalizedTime * 180f;
        sunSource.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 90f, 0f));
        float intensity = 1 - Mathf.Abs(normalizedTime - 0.5f) * 2;
        sunSource.GetComponent<Light>().intensity = intensity;
        if (currentTime >= dayDuration)
        {
            FinishDayCycle();
        }

    }
    /// <summary>
    /// Lerps the skybox color for day-night transition.
    /// </summary>
    void LerpSkyboxColor()
    {
        float nightTime = dayDuration - nightThreshold;
        float normalizedTime = (currentTime - nightTime) / nightThreshold; 
        skyboxMaterial.SetFloat("_Blend", normalizedTime);
    }
    /// <summary>
    /// Ends the day, calculates scores, and awards badges based on task completion times.
    /// </summary>
    void FinishDayCycle()
    {
        // Calculate cooking score
        float badgeMultiplier = 0f;
        if (cookingBadge == 3)
        {
            badgeMultiplier = 2f;
        }
        else if (cookingBadge == 2)
        {
            badgeMultiplier = 1.5f;
        }
        else if (cookingBadge == 1)
        {
            badgeMultiplier = 1f;
        }
        float totalScore = (dayDuration-cookingFinishTime) * badgeMultiplier;

        // Calculate tent score
         badgeMultiplier = 0f;
        if (tentBadge == 3)
        {
            badgeMultiplier = 2f;
        }
        else if (tentBadge == 2)
        {
            badgeMultiplier = 1.5f;
        }
        else if (tentBadge == 1)
        {
            badgeMultiplier = 1f;
        }
        totalScore += (dayDuration - tentFinishTime) * badgeMultiplier;

        //Calculate fishing score
        badgeMultiplier = 0f;
        if (fishingBadge == 3)
        {
            badgeMultiplier = 2f;
        }
        else if (fishingBadge == 2)
        {
            badgeMultiplier = 1.5f;
        }
        else if (fishingBadge == 1)
        {
            badgeMultiplier = 1f;
        }
        totalScore += (dayDuration - fishingFinishTime) * badgeMultiplier;
        //Calculate campfire score
        badgeMultiplier = 0f;
        if (campfireBadge == 3)
        {
            badgeMultiplier = 2f;
        }
        else if (campfireBadge == 2)
        {
            badgeMultiplier = 1.5f;
        }
        else if (campfireBadge == 1)
        {
            badgeMultiplier = 1f;
        }
        totalScore += (dayDuration - campfireFinishTime) * badgeMultiplier;

        Debug.Log("Day cycle finished! Total Score: " + totalScore);
        Debug.Log("Cooking Badge: " + cookingBadge + ", Tent Badge: " + tentBadge + ", Fishing Badge: " + fishingBadge + ", Campfire Badge: " + campfireBadge);
        GameManager.Instance.EndDay(totalScore, cookingBadge, tentBadge, fishingBadge, campfireBadge);
    }
    /// <summary>
    /// Ends the cooking task and awards the appropriate badge.
    /// </summary>
    /// <param name="badgeType"></param>
    public void EndCookingTask(string badgeType)
    {
        cookingFinished = true;
        cookingFinishTime = currentTime;
        if(badgeType == "Gold")
        {
            cookingBadge = 3;
            Debug.Log("Gold cooking badge awarded");
        }
        else if(badgeType == "Silver")
        {
            cookingBadge = 2;
            Debug.Log("Silver cooking badge awarded");
        }
        else if(badgeType == "Bronze")
        {
            cookingBadge = 1;
            Debug.Log("Bronze cooking badge awarded");
        }
        Debug.Log("Cooking task ended at time: " + cookingFinishTime);
        
    }
    /// <summary>
    /// Ends the camping task and awards the appropriate badge.
    /// </summary>
    /// <param name="badgeType"></param>
    public void EndCampingTask(string badgeType)
    {
        tentFinished = true;
        tentFinishTime = currentTime;
        if(badgeType == "Gold")
        {
            tentBadge = 3;
            Debug.Log("Gold tent badge awarded");
        }
        else if(badgeType == "Silver")
        {
            tentBadge = 2;
            Debug.Log("Silver tent badge awarded");
        }
        else if(badgeType == "Bronze")
        {
            tentBadge = 1;
            Debug.Log("Bronze tent badge awarded");
        }
        Debug.Log("Camping task ended at time: " + tentFinishTime);
        
    }
    /// <summary>
    /// Ends the campfire task and awards the appropriate badge.
    /// </summary>
    /// <param name="badgeType"></param>
    public void EndCampfireTask(string badgeType)
    {
        campfireFinished = true;
        campfireFinishTime = currentTime;
        if(badgeType == "Gold")
        {
            campfireBadge = 3;
            Debug.Log("Gold campfire badge awarded");
        }
        else if(badgeType == "Silver")
        {
            campfireBadge = 2;
            Debug.Log("Silver campfire badge awarded");
        }
        else if(badgeType == "Bronze")
        {
            campfireBadge = 1;
            Debug.Log("Bronze campfire badge awarded");
        }
        Debug.Log("Campfire task ended at time: " + campfireFinishTime);
        
    }
    /// <summary>
    /// Ends the fishing task and awards the appropriate badge.
    /// </summary>
    /// <param name="badgeType"></param>
    public void EndFishingTask(string badgeType)
    {
        fishingFinished = true;
        if(badgeType == "Gold")
        {
            fishingBadge = 3;
            Debug.Log("Gold fishing badge awarded");
        }
        else if(badgeType == "Silver")
        {
            fishingBadge = 2;
            Debug.Log("Silver fishing badge awarded");
        }
        else if(badgeType == "Bronze")
        {
            fishingBadge = 1;
            Debug.Log("Bronze fishing badge awarded");
        }
        fishingFinishTime = currentTime;
        Debug.Log("Fishing task ended at time: " + fishingFinishTime);
    }
    
}
