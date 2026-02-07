/*
* File Name: GameManager.cs
* Author: Lim En Xu Jayson
* Date Created: 21/01/2026
* Description: Overall game manager to handle cross scene requirements.
*/
using Firebase.Database;
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
    public int[] endDayScores;
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
    public void EndDay(float totalScore, int cookingBadge, int tentBadge, int fishingBadge, int campfireBadge)
    {
        Debug.Log("Ending day and returning to main menu.");
        GameManager.Instance.currentUser.campCount += 1;
        endDayScores = new int[5];
        endDayScores[0] = (int)totalScore;
        endDayScores[1] = tentBadge;
        endDayScores[2] = campfireBadge;
        endDayScores[3] = fishingBadge;
        endDayScores[4] = cookingBadge;
        if ((int)totalScore > currentUser.highscore)
        {
            currentUser.highscore = (int)totalScore;
        }
        if (cookingBadge > currentUser.badges.cook)
        {
            currentUser.badges.cook = cookingBadge;
        }
        if (tentBadge > currentUser.badges.camper)
        {
            currentUser.badges.camper = tentBadge;
        }
        if (fishingBadge > currentUser.badges.angler)
        {
            currentUser.badges.angler = fishingBadge;
        }
        if (campfireBadge > currentUser.badges.backwoodsman)
        {
            currentUser.badges.backwoodsman = campfireBadge;
        }
        DatabaseManager.Instance.UpdateUserData(currentPlayerID, currentUser);
        SceneManager.LoadScene("DayEndScene");

    }

}
