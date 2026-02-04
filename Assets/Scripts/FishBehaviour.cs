
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Animations;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using NUnit.Framework;

public class FishBehaviour : MonoBehaviour
{
    bool isHooked = true;
    private Coroutine cookingCoroutine;
    public float cookingProgress = 0f;
    public float[] cookingDuration; // 0: perfectStart, 1: perfectEnd, 2: maxCookingTime
    public Slider cookingSlider;
    public Canvas cookingCanvas;
    public bool onStick = false;
    [SerializeField] private float depthOffset = 0.1f;
    [SerializeField] private RectTransform successArea;
    [SerializeField] Material[] fishMaterials; // 0: raw, 1: cooked, 2: burnt
    
    private void Start()
    {
        hideUI();
        cookingSlider.value = 0f;
        cookingSlider.maxValue = cookingDuration[2];
        successArea.anchorMin = new Vector2(cookingDuration[0] / cookingDuration[2], successArea.anchorMin.y);
        successArea.anchorMax = new Vector2(cookingDuration[1] / cookingDuration[2], successArea.anchorMax.y);
        

    }
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
    public void Unhook()
    {
        isHooked = false;
        transform.SetParent(null);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Campfire"))
        {
            cookingCoroutine = StartCoroutine(StartCookingCoroutine());
        }
    }
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
    public void showUI()
    {
        cookingCanvas.enabled = true;
        Debug.Log("Showing UI");
    }
    public void hideUI()
    {
        cookingCanvas.enabled = false;
        Debug.Log("Hiding UI");
    }

    IEnumerator StartCookingCoroutine()
    {
        while (cookingProgress < cookingDuration[2])
        {
            cookingProgress += 0.1f;
            yield return new WaitForSeconds(0.1f);
            updateUI(cookingProgress);
        }

    }
    
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
    
    public void Escape()
    {
        Destroy(this.gameObject);
    }
}