/*
* File Name: MenuManager.cs
* Author: Emilie Tee Jing Hui
* Date Created: 3/2/2026
* Description: Manages the menu UI and interactions
*/

using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        TransitionManager.Instance.ChangeScene("GameScene");
        DatabaseManager.Instance.UpdateUserData(GameManager.Instance.currentPlayerID, GameManager.Instance.currentUser);
    }
}
