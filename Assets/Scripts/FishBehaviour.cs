/*
* File Name: FishBehaviour.cs
* Author: Lim En Xu Jayson
* Date Created: 29/01/2026
* Description: Fish behaviour script to handle cooking and UI.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Animations;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class FishBehaviour : MonoBehaviour
{
    /// <summary>
    /// Indicates if the fish is hooked.
    /// </summary>
    bool isHooked = true;
    /// <summary>
    /// Coroutine for cooking process.
    /// </summary>
    private Coroutine cookingCoroutine;
    /// <summary>
    /// Cooking progress of the fish.
    /// </summary>
    public float cookingProgress = 0f;
    /// <summary>
    /// Cooking duration parameters.
    /// </summary>
    public float[] cookingDuration; // 0: perfectStart, 1: perfectEnd, 2: maxCookingTime
    
    /// <summary>
    /// Slider UI for cooking progress.
    /// </summary>
    public Slider cookingSlider;
    /// <summary>
    /// Canvas for cooking UI.
    /// </summary>
    public Canvas cookingCanvas;
    /// <summary>
    /// Indicates if the fish is on the stick.
    /// </summary>
    public bool onStick = false;
    /// <summary>
    /// Depth offset for UI positioning.
    /// </summary>
    [SerializeField] private float depthOffset = 0.1f;
    /// <summary>
    /// RectTransform for the success area in the cooking UI.
    /// </summary>
    [SerializeField] private RectTransform successArea;
    /// <summary>
    /// Materials for different cooking states of the fish.
    /// </summary>
    [SerializeField] Material[] fishMaterials; // 0: raw, 1: cooked, 2: burnt
    
    /// <summary>
    /// Initializes the fish behaviour and cooking UI.
    /// </summary>
    private void Start()
    {
        hideUI();
        cookingSlider.value = 0f;
        cookingSlider.maxValue = cookingDuration[2];
        successArea.anchorMin = new Vector2(cookingDuration[0] / cookingDuration[2], successArea.anchorMin.y);
        successArea.anchorMax = new Vector2(cookingDuration[1] / cookingDuration[2], successArea.anchorMax.y);
        

    }
    /// <summary>
    /// Updates the cooking UI position and rotation.
    /// </summary>
    private void Update()
    {
        if(onStick )
        {
            cookingCanvas.transform.position = transform.position + new Vector3(0, 0.1f, 0);
            cookingCanvas.transform.rotation = Quaternion.LookRotation(cookingCanvas.transform.position - Camera.main.transform.position);
            Vector3 dirToPlayer = Camera.main.transform.position - cookingCanvas.transform.position;
            cookingCanvas.transform.rotation = Quaternion.LookRotation(-dirToPlayer);
            cookingCanvas.transform.position += dirToPlayer * depthOffset;
        }
    }
    /// <summary>
    /// Changes bools when unhooking the fish.
    /// </summary>
    public void Unhook()
    {
        isHooked = false;
        transform.SetParent(null);
    }
    /// <summary>
    /// Handles trigger enter events for cooking.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Campfire"))
        {
            cookingCoroutine = StartCoroutine(StartCookingCoroutine());
        }
    }
    /// <summary>
    /// Handles trigger exit events for cooking.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Campfire"))
        {
            if (cookingCoroutine != null)
            {
                StopCoroutine(cookingCoroutine);
                cookingCoroutine = null;
            }
        }
    }
    /// <summary>
    /// Shows the cooking UI.
    /// </summary>
    public void showUI()
    {
        cookingCanvas.enabled = true;
        Debug.Log("Showing UI");
    }
    /// <summary>
    /// Hides the cooking UI.
    /// </summary>
    public void hideUI()
    {
        cookingCanvas.enabled = false;
        Debug.Log("Hiding UI");
    }
    /// <summary>
    /// Coroutine to handle the cooking process.
    /// </summary>
    /// <returns></returns>
    IEnumerator StartCookingCoroutine()
    {
        while (cookingProgress < cookingDuration[2])
        {
            cookingProgress += 0.1f;
            yield return new WaitForSeconds(0.1f);
            updateUI(cookingProgress);
        }

    }
    /// <summary>
    /// Updates the cooking UI and fish material based on cooking progress.
    /// </summary>
    /// <param name="progress"></param>
    void updateUI(float progress)
    {
        cookingSlider.value = progress;
        if (progress >= cookingDuration[0] && progress <= cookingDuration[1])
        {
            // Perfectly cooked
            GetComponent<Renderer>().material = fishMaterials[1];
            gameObject.GetNamedChild("eye_L").GetComponent<Renderer>().material = fishMaterials[1];
            gameObject.GetNamedChild("eye_R").GetComponent<Renderer>().material = fishMaterials[1];
            CampRun.Instance.EndCookingTask("Gold");
        }
        else if (progress > cookingDuration[1] && progress <= cookingDuration[2])
        {
            // Overcooked
            GetComponent<Renderer>().material = fishMaterials[2];
            gameObject.GetNamedChild("eye_L").GetComponent<Renderer>().material = fishMaterials[2];
            gameObject.GetNamedChild("eye_R").GetComponent<Renderer>().material = fishMaterials[2]; 
            CampRun.Instance.EndCookingTask("Silver");
        }
        else if (progress > cookingDuration[2])
        {
            // Burnt
            GetComponent<Renderer>().material = fishMaterials[2];
            gameObject.GetNamedChild("eye_L").GetComponent<Renderer>().material = fishMaterials[2];
            gameObject.GetNamedChild("eye_R").GetComponent<Renderer>().material = fishMaterials[2];
            CampRun.Instance.EndCookingTask("Bronze");
        }
        else
        {
            // Raw
            GetComponent<Renderer>().material = fishMaterials[0];
            gameObject.GetNamedChild("eye_L").GetComponent<Renderer>().material = fishMaterials[0];
            gameObject.GetNamedChild("eye_R").GetComponent<Renderer>().material = fishMaterials[0];
        }
    }
    /// <summary>
    /// Destroys the fish object.
    /// </summary>
    public void Escape()
    {
        Destroy(this.gameObject);
    }
}