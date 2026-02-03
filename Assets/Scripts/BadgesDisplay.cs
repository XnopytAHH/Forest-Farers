/*
* File Name: BadgesDisplay.cs
* Author: Emilie Tee Jing Hui
* Date Created: 3/2/2026
* Description: Displays the badges earned by the user on menu UI
*/
using System.Collections.Generic;
using UnityEngine;

public class BadgesDisplay : MonoBehaviour
{
    [SerializeField] private List <GameObject> BadgesSlots;
    [SerializeField] private List <Sprite> BadgesIcons;
    [SerializeField] private List <Sprite> BadgesRanks;

    /// <summary>
    /// Displays the badge in the corresponding slot with the appropriate icon and rank.
    /// </summary>
    void Start()
    {
        // Access GameManager.Instance directly as it's a static property
        var badges = GameManager.Instance.currentUser.badges;
        Debug.Log(badges);

        // Iterate through each badge and display it
        foreach (var BadgeSlot in BadgesSlots)
        {
            int badgeIndex = BadgesSlots.IndexOf(BadgeSlot);
            if (badgeIndex < badges.badgeValues.Count && badges.badgeValues[badgeIndex] > 0)
            {
                var rank = badges.badgeValues[badgeIndex];
                // Set the icon and rank sprites
                BadgeSlot.transform.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = BadgesIcons[badgeIndex];
                BadgeSlot.transform.GetChild(0).gameObject.SetActive(true);
                BadgeSlot.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = BadgesRanks[rank - 1];
            }
            else
            {
                // If no badge earned, set to default icon and hide rank because there isnt a rank
                BadgeSlot.transform.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = BadgesIcons[4];
                BadgeSlot.transform.GetChild(0).gameObject.SetActive(false);
            }

        }
    }
    
}
