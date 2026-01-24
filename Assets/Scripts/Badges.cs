using System.Collections.Generic;
using UnityEngine.XR.ARFoundation.VisualScripting;

[System.Serializable]
public class Badges
{
    public int angler;
    public int backwoodsman;
    public int camper;
    public int cook;
    public Badges()
    {
        angler = 0;
        backwoodsman = 0;
        camper = 0;
        cook = 0;
    }
}
