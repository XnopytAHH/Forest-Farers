using System.Collections.Generic;
/*
* File Name: CampfireRows.cs
* Author: Lim En Xu Jayson
* Date Created: 27/01/2026
* Description: Custom Class to hold campfire row information.
*/

[System.Serializable]
public class CampfireRows
{
    /// <summary>
    /// Array of XRSocketInteractors for the campfire row.
    /// </summary>
    public XRSocketInteractor[] socketInteractors;
    /// <summary>
    /// Flag to indicate if the row is completed.
    /// </summary>
    public bool isCompleted;
}
