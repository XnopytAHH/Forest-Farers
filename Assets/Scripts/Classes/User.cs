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
    public bool antiMotionSickness;
    public int height;
    public int music;
    public int sfx;

    public User(string Username)
    {
        username = Username;
        campCount = 0;
        finishedTutorial = false;
        highscore = 0;
        badges = new Badges();
        antiMotionSickness = false;
        height = 0;
        music = 0;
        sfx = 0;
    }
}