using System.Collections.Generic;
/*
* File Name: User.cs
* Author: Lim En Xu Jayson
* Date Created: 24/01/2026
* Description: Custom Class to hold user information.
*/

[System.Serializable]
public class User 
{
    /// <summary>
    /// Username of the user.
    /// </summary>
    public string username;
    /// <summary>
    /// Number of camps the user has completed.
    /// </summary>
    public int campCount;
    /// <summary>
    /// Flag to indicate if the user has finished the tutorial.
    /// </summary>
    public bool finishedTutorial;
    /// <summary>
    /// User's highscore.
    /// </summary>
    public int highscore;
    /// <summary>
    /// User's badges.
    /// </summary>
    public Badges badges;
    /// <summary>
    /// User's anti-motion sickness setting.
    /// </summary>
    public bool antiMotionSickness;
    /// <summary>
    /// User's height in cm.
    /// </summary>
    public int height;
    /// <summary>
    /// User's music volume setting.
    /// </summary>
    public int music;
    /// <summary>
    /// User's SFX volume setting.
    /// </summary>
    public int sfx;

    public User(string Username)
    {
        username = Username;
        campCount = 0;
        finishedTutorial = false;
        highscore = 0;
        badges = new Badges();
        antiMotionSickness = false;
        height = 130;
        music = 0;
        sfx = 0;
    }
}