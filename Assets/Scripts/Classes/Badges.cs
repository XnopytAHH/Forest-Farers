using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.XR.ARFoundation.VisualScripting;

[System.Serializable]
public class Badges
{
    public int angler;
    public int backwoodsman;
    public int camper;
    public int cook;

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
