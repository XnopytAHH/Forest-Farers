/*
* TransitionManager.cs
* Author: Lim En Xu Jayson
* Date Created: 19/01/2026
* Description: Manages scene transitions with a step effect using a shader material.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TransitionManager : MonoBehaviour
{
   public bool startedStep = false;
   Material transitionMaterial;
   [SerializeField]
   float transitionSpeed = 0.02f;
   public static TransitionManager Instance;
   [SerializeField]
   Image image;

   private void Awake() {

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            transitionMaterial = image.material;
            transitionMaterial.SetFloat("_Step", 0f);
         }
         else
         {
             Destroy(gameObject);
         }
         
   }
   private void Update() {
         if(startedStep)
         {
              startedStep = false;
              ChangeScene("GameScene");
         }
   }
   public void ChangeScene(string sceneName)
    {
        StopAllCoroutines();
        StartCoroutine(stepTransition(sceneName));
    }

    IEnumerator stepTransition(string sceneName)
    {
        AsyncOperation sceneLoader= SceneManager.LoadSceneAsync(sceneName);
        sceneLoader.allowSceneActivation = false;
        transitionMaterial.SetFloat("_Step", 0f);
        float stepValue = 0f;
        while (stepValue < 1f)
        {
            stepValue += 0.01f;
            transitionMaterial.SetFloat("_Step", stepValue);
            yield return new WaitForSeconds(transitionSpeed);
        }
        sceneLoader.allowSceneActivation = true;
        yield return new WaitUntil(() => sceneLoader.isDone);
        Debug.Log("Scene Loaded");
        stepValue = 1f;
        while (stepValue > 0f)
        {
            stepValue -= 0.01f;
            transitionMaterial.SetFloat("_Step", stepValue);
            yield return new WaitForSeconds(transitionSpeed);
        }
        
    }

}
