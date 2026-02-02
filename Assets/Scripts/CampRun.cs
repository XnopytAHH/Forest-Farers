using System;
using Unity.VisualScripting;
using UnityEngine;

public class CampRun : MonoBehaviour
{
    float dayDuration = 600f; // Duration of a day in seconds
    public float currentTime = 0f; // Current time in the day
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    GameObject sunSource;
    [SerializeField]
    Material skyboxMaterial;
    [SerializeField]
    float nightThreshold = 180f;
    void Start()
    {
        skyboxMaterial.SetFloat("_Blend", 0f);
        currentTime = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        LerpSunPosition();
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

    }
    void LerpSkyboxColor()
    {
        float nightTime = dayDuration - nightThreshold;
        float normalizedTime = (currentTime - nightTime) / nightThreshold; 
        skyboxMaterial.SetFloat("_Blend", normalizedTime);
    }


}
