using System.Collections.Generic;
using UnityEngine.XR.ARFoundation.VisualScripting;

[System.Serializable]
public class User 
{
    public string username;
    public int campCount;
    public bool finishedTutorial;
    public int highscore;
    public Badges badges;
    public User(string Username)
    {
        username = Username;
        campCount = 0;
        finishedTutorial = false;
        highscore = 0;
        badges = new Badges();
    }
}
