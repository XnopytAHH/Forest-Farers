using System;
using Unity.VisualScripting;
using UnityEngine;

public class CampRun : MonoBehaviour
{
    public float dayDuration = 600f; // Duration of a day in seconds
    public float currentTime = 0f; // Current time in the day
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    GameObject sunSource;
    [SerializeField]
    Material skyboxMaterial;
    [SerializeField]
    public float nightThreshold = 180f;
    public float cookingFinishTime = 0f;
    public float fishingFinishTime = 0f;
    public float tentFinishTime = 0f;
    public float campfireFinishTime = 0f;
    public int cookingBadge = 0;
    public int fishingBadge = 0;
    public int tentBadge = 0;
    public int campfireBadge = 0;
    public static CampRun Instance;
    public TentBehaviour tentBehaviour;

    void Start()
    {
        Instance = this;
        skyboxMaterial.SetFloat("_Blend", 0f);
        currentTime = 0f;
    }
    // Update is called once per frame
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
    void LerpSunPosition()
    {
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
    void LerpSkyboxColor()
    {
        float nightTime = dayDuration - nightThreshold;
        float normalizedTime = (currentTime - nightTime) / nightThreshold; 
        skyboxMaterial.SetFloat("_Blend", normalizedTime);
    }
    void FinishDayCycle()
    {
        // Calculate cooking score
        float badgeMultiplier = 1f;
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
        if (tentBehaviour.gameObject.activeSelf)
        {
            tentBadge = tentBehaviour.CheckTentBadge();
        }
        else
        {
            tentBadge = 0;
        }
        
        tentFinishTime = currentTime;
         badgeMultiplier = 1f;
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
        badgeMultiplier = 1f;
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
        badgeMultiplier = 1f;
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
    public void EndCookingTask(string badgeType)
    {
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
    public void EndCampfireTask(string badgeType)
    {
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
    public void EndFishingTask(string badgeType)
    {
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
