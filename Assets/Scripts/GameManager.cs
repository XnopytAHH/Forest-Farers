/*
* File Name: GameManager.cs
* Author: Lim En Xu Jayson
* Date Created: 21/01/2026
* Description: Overall game manager to handle cross scene requirements.
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the player object across scenes.
    /// </summary>
    public GameObject player;
    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string currentPlayerID;
    public User currentUser;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += CheckSceneRequirements;
            CheckSceneRequirements(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void CheckSceneRequirements(Scene current, Scene next)
    {
        player = GameObject.FindWithTag("Player");
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Login")
        {
            DisableMovement();
        }
        else
        {
            EnableMovement();
        }
    }
    
    private void DisableMovement()
    {
        GameObject TPInteractor = player.transform.GetChild(0).GetChild(5).GetChild(2).gameObject;
        GameObject moveControl = player.transform.GetChild(1).GetChild(1).gameObject;
        GameObject TPReciever = player.transform.GetChild(1).GetChild(3).gameObject;
        TPInteractor.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>().enabled = false;
        moveControl.GetComponent<UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider>().enabled = false;
        TPReciever.GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>().enabled = false;
        Debug.Log("Movement Disabled");
    }
    private void EnableMovement()
    {
        GameObject TPInteractor = player.transform.GetChild(0).GetChild(5).GetChild(2).gameObject;
        GameObject moveControl = player.transform.GetChild(1).GetChild(1).gameObject;
        GameObject TPReciever = player.transform.GetChild(1).GetChild(3).gameObject;
        TPInteractor.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>().enabled = true;
        moveControl.GetComponent<UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider>().enabled = true;
        TPReciever.GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>().enabled = true;
        Debug.Log("Movement Enabled");
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= CheckSceneRequirements;
    }
}
