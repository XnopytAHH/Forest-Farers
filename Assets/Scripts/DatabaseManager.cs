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
    FirebaseDatabase db;
    
    public static DatabaseManager Instance;
    public string[] campingHints;
    public string[] fishingHints;
    public string[] cookingHints;
    public string[] campfireHints;
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
    public void CreateUser(string userId, string username)
    {
        User newUser = new User(username);
        string json = JsonUtility.ToJson(newUser);
        db.RootReference.Child("players").Child(userId).SetRawJsonValueAsync(json);
    }
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
    public void UpdateUserData(string userId, User userData)
    {
        if (userData == null || userId == null || userId == "" ) return;
        string json = JsonUtility.ToJson(userData);
        db.RootReference.Child("players").Child(userId).SetRawJsonValueAsync(json);
    }
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
