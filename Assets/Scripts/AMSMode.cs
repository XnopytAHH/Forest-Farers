using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class AMSMode : MonoBehaviour
{
    
    public Transform head; // assign XR Camera
    public Volume volume;

    public float maxVignette = 0.4f;
    public float sensitivity = 0.5f;
    public float smoothSpeed = 5f;

    private Vignette vignette;
    private Quaternion lastRotation;
    private float currentIntensity;

    public static AMSMode Instance;


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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        head = Camera.main.transform;
        volume = gameObject.GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        lastRotation = head.rotation;
    }
}


