/*
* File Name: AMSMode.cs
* Author: Lim En Xu Jayson
* Date Created: 09/02/2026
* Description: Anti-Motion Sickness Mode class to handle vignette effects.
*/
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class AMSMode : MonoBehaviour
{
    /// <summary>
    /// Reference to the XR Camera's transform.
    /// </summary>
    public Transform head; 
    /// <summary>
    /// Reference to the Volume component.
    /// </summary>
    public Volume volume;
    /// <summary>
    /// Maximum vignette intensity.
    /// </summary>
    public float maxVignette = 0.4f;
    /// <summary>
    /// Sensitivity factor for vignette intensity based on angular speed.
    /// </summary>
    public float sensitivity = 0.5f;
    /// <summary>
    /// Smoothing speed for vignette intensity transitions.
    /// </summary>
    public float smoothSpeed = 5f;
    /// <summary>
    /// Reference to the Vignette effect.
    /// </summary>
    private Vignette vignette;
    /// <summary>
    /// Last frame's head rotation.
    /// </summary>
    private Quaternion lastRotation;
    /// <summary>
    /// Current vignette intensity.
    /// </summary>
    private float currentIntensity;
    /// <summary>
    /// Singleton instance of AMSMode.
    /// </summary>
    public static AMSMode Instance;

    /// <summary>
    /// Initializes the AMSMode instance and sets up references.
    /// </summary>
    void Start()
    {
        head = Camera.main.transform;
        volume = gameObject.GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        lastRotation = head.rotation;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    /// <summary>
    /// Updates the vignette intensity based on head movement.
    /// </summary>
    void Update()
    {
        if (GameManager.Instance.currentUser.antiMotionSickness == false)
        {
            vignette.intensity.value = 0f;
            return;
        }
        // Calculate angular difference
        Quaternion delta = head.rotation * Quaternion.Inverse(lastRotation);
        float angle;
        Vector3 axis;
        delta.ToAngleAxis(out angle, out axis);

        float angularSpeed = angle / Time.deltaTime;

        // Map speed to vignette
        float targetIntensity = Mathf.Clamp01(angularSpeed * sensitivity) * maxVignette;

        // Smooth transition
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * smoothSpeed);

        vignette.intensity.value = currentIntensity;

        lastRotation = head.rotation;
    }
    /// <summary>
    /// Handles scene loading to reset references.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        head = Camera.main.transform;
        volume = gameObject.GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        lastRotation = head.rotation;
    }
}


