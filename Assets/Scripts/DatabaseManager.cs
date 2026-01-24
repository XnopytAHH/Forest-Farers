/*
* File Name: DatabaseManager.cs
* Author: Lim En Xu Jayson
* Date Created: 24/01/2026
* Description: Manager to handle database interactions.
*/
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class DatabaseManager : MonoBehaviour
{
    FirebaseDatabase db;
    
    public static DatabaseManager Instance;
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
        });
    }
}
