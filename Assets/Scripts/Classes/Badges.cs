/*
* File Name: Badges.cs
* Author: Lim En Xu Jayson
* Date Created: 24/01/2026
* Description: Custom Badge class to hold badge information for users.
*/
using System.Collections.Generic;
[System.Serializable]
public class Badges
{
    /// <summary>
    /// Angler Badge - Awarded for fishing activities.
    /// </summary>
    public int angler;
    /// <summary>
    /// Backwoodsman Badge - Awarded for Campfire Activities.
    /// </summary>
    public int backwoodsman;
    /// <summary>
    /// Camper Badge - Awarded for completing tent tasks.
    /// </summary>
    public int camper;
    /// <summary>
    /// Cook Badge - Awarded for cooking activities.
    /// </summary>
    public int cook;
    /// <summary>
    /// List to hold all badge values for easy access.
    /// </summary>
    public List<int> badgeValues;

    public Badges()
    {
        angler = 0;
        backwoodsman = 0;
        camper = 0;
        cook = 0;

        badgeValues = new List<int> { angler, backwoodsman, camper, cook };
    }

    public void UpdateBadgeValues()
    {
        badgeValues = new List<int> { angler, backwoodsman, camper, cook };
    }

}
