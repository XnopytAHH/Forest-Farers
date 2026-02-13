/*
* File Name: DatabaseManager.cs
* Author: Lim En Xu Jayson
* Date Created: 24/01/2026
* Description: Manager to handle database interactions.
*/
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class DatabaseManager : MonoBehaviour
{
    /// <summary>
    /// Firebase Database reference.
    /// </summary>
    FirebaseDatabase db;
    /// <summary>
    /// Singleton instance of DatabaseManager.
    /// </summary>
    public static DatabaseManager Instance;
    /// <summary>
    /// Camping hints retrieved from the database.
    /// </summary>
    public string[] campingHints;
    /// <summary>
    /// Fishing hints retrieved from the database.
    /// </summary>
    public string[] fishingHints;
    /// <summary>
    /// Cooking hints retrieved from the database.
    /// </summary>
    public string[] cookingHints;
    /// <summary>
    /// Campfire hints retrieved from the database.
    /// </summary>
    public string[] campfireHints;

    /// <summary>
    /// Initializes the DatabaseManager and sets up database listeners.
    /// </summary>
    public void Start()
    {
        db = FirebaseDatabase.DefaultInstance;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        db.RootReference.ValueChanged += dbValueChanged;
        RetrieveHints();
    }
    /// <summary>
    /// Retrieves hints from the database and stores them in local arrays.
    /// </summary>
    public void RetrieveHints()
    {
        db.RootReference.Child("hints").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to retrieve hints.");
                return;
            }
            DataSnapshot snapshot = task.Result;
            campingHints = new string[4]{snapshot.Child("Camper").Child("0").GetRawJsonValue().ToString(),
                                        snapshot.Child("Camper").Child("1").GetRawJsonValue().ToString(),
                                        snapshot.Child("Camper").Child("2").GetRawJsonValue().ToString(),
                                        snapshot.Child("Camper").Child("3").GetRawJsonValue().ToString()};
            fishingHints = new string[4]{snapshot.Child("Angler").Child("0").GetRawJsonValue().ToString(),
                                        snapshot.Child("Angler").Child("1").GetRawJsonValue().ToString(),
                                        snapshot.Child("Angler").Child("2").GetRawJsonValue().ToString(),
                                        snapshot.Child("Angler").Child("3").GetRawJsonValue().ToString()};
            cookingHints = new string[4]{snapshot.Child("Cook").Child("0").GetRawJsonValue().ToString(),
                                        snapshot.Child("Cook").Child("1").GetRawJsonValue().ToString(),
                                        snapshot.Child("Cook").Child("2").GetRawJsonValue().ToString(),
                                        snapshot.Child("Cook").Child("3").GetRawJsonValue().ToString()};
            campfireHints = new string[4]{snapshot.Child("Backwoodsman").Child("0").GetRawJsonValue().ToString(),
                                        snapshot.Child("Backwoodsman").Child("1").GetRawJsonValue().ToString(),
                                        snapshot.Child("Backwoodsman").Child("2").GetRawJsonValue().ToString(),
                                        snapshot.Child("Backwoodsman").Child("3").GetRawJsonValue().ToString()};
            
        });
    }
    /// <summary>
    /// Gets a hint for a specific badge and level.
    /// </summary>
    /// <param name="badgeName"></param>
    /// <param name="badgeLevel"></param>
    /// <returns></returns>
    public string GetHintForBadge(string badgeName, int badgeLevel)
    {
        if (badgeName=="Camper")
        {
            return campingHints[badgeLevel];
        }
        else if (badgeName=="Angler")
        {
            return fishingHints[badgeLevel];
        }
        else if (badgeName=="Cook")
        {
            return cookingHints[badgeLevel];
        }
        else if (badgeName=="Backwoodsman")
        {
            return campfireHints[badgeLevel];
        }
        return "No hint available.";
    }
    /// <summary>
    /// Creates a new user in the database.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="username"></param>
    public void CreateUser(string userId, string username)
    {
        User newUser = new User(username);
        string json = JsonUtility.ToJson(newUser);
        db.RootReference.Child("players").Child(userId).SetRawJsonValueAsync(json);
    }
    /// <summary>
    /// Retrieves user data from the database.
    /// </summary>
    /// <param name="userId"></param>
    public void RetrieveUser(string userId)
    {
        db.RootReference.Child("players").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to retrieve user data.");
                return;
            }
            DataSnapshot snapshot = task.Result;
            string json = snapshot.GetRawJsonValue();
            GameManager.Instance.currentUser = JsonUtility.FromJson<User>(json);
            GameManager.Instance.currentUser.badges.UpdateBadgeValues();
        });
    }
    /// <summary>
    /// Updates user data in the database.
    /// </summary>
    public void UpdateUserData(string userId, User userData)
    {
        if (userData == null || userId == null || userId == "" ) return;
        string json = JsonUtility.ToJson(userData);
        db.RootReference.Child("players").Child(userId).SetRawJsonValueAsync(json);
    }
    /// <summary>
    /// Handles database value changes and updates the current user data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void dbValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError("Database error: " + args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;
        if (GameManager.Instance.currentPlayerID == null) return;
        snapshot = snapshot.Child("players").Child(GameManager.Instance.currentPlayerID);
        string json = snapshot.GetRawJsonValue();
        GameManager.Instance.currentUser = JsonUtility.FromJson<User>(json);
        GameManager.Instance.currentUser.badges.UpdateBadgeValues();
        
    }
}
