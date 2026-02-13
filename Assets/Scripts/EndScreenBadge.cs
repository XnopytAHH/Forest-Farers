/*
* File Name: EndScreenBadge.cs
* Author: Lim En Xu Jayson
* Date Created: 08/02/2026
* Description: End screen badge class to handle badge UI elements.
*/
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class EndScreenBadge : MonoBehaviour
{
    /// <summary>
    /// Name of the badge.
    /// </summary>
    private string badgeName;
    /// <summary>
    /// Image representing the badge.
    /// </summary>
    [SerializeField]
    Sprite badgeImage;
    /// <summary>
    /// Sprites for different badge levels.
    /// </summary>
    [SerializeField]
    Sprite[] badgeSprites; // 0: Bronze, 1: Silver, 2: Gold

    /// <summary>
    /// Image component for the stitch.
    /// </summary>
    Image stitch;
    /// <summary>
    /// Image component for the badge.
    /// </summary>
    Image image;
    /// <summary>
    /// Prefab for the badge UI display.
    /// </summary>
    [SerializeField]
    GameObject badgeUIPrefab;
    /// <summary>
    /// Active badge UI instance.
    /// </summary>
    GameObject activeBadgeUI;
    /// <summary>
    /// Level of the badge earned.
    /// </summary>
    public int badgeLevel;
    /// <summary>
    /// Level name of the badge (e.g., Bronze, Silver, Gold).
    /// </summary>
    private string badgeLevelName;
    /// <summary>
    /// Initializes the badge UI elements based on the badge type and level.
    /// </summary>
    private void Start()
    {
        stitch = transform.GetChild(0).GetComponent<Image>();
        image = gameObject.GetComponent<Image>();
        if (gameObject.name.Contains("Camper")) {
            badgeLevel= GameManager.Instance.endDayScores[1];
            badgeName = "Camper";
        }
        if (gameObject.name.Contains("Backwoodsman")) {
            badgeLevel= GameManager.Instance.endDayScores[2];
            badgeName = "Backwoodsman";
        }
        if (gameObject.name.Contains("Angler")) {
            badgeLevel= GameManager.Instance.endDayScores[3];
            badgeName = "Angler";
        }
        if (gameObject.name.Contains("Cook")) {
            badgeLevel= GameManager.Instance.endDayScores[4];
            badgeName = "Cook";
        }
        if (badgeLevel == 1)
            {
                badgeLevelName = "Bronze"; // Bronze
            }
            else if (badgeLevel == 2)
            {
                badgeLevelName = "Silver"; // Silver
            }
            else if (badgeLevel == 3)
            {
                badgeLevelName = "Gold"; // Gold
            }
            else
            {
                badgeLevelName = "None";
            }
        SetBadgeImage();
    }
    /// <summary>
    /// Sets the badge image and stitch based on the badge level.
    /// </summary>
    public void SetBadgeImage()
    {
        if (badgeLevel >= 1 && badgeLevel <= 3)
        {
            stitch.sprite = badgeSprites[badgeLevel - 1];
            image.sprite = badgeImage;
        }
        else
        {
            Debug.LogWarning("Invalid badge level: " + badgeLevel);
        }
    }
    /// <summary>
    /// Activates the badge UI display with relevant information.
    /// </summary>
    public void activateBadgeUI()
    {
        activeBadgeUI = Instantiate(badgeUIPrefab, transform.position, Quaternion.identity, transform);
        activeBadgeUI.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        activeBadgeUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = badgeName + " Badge";
        activeBadgeUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = badgeLevelName;
        activeBadgeUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = DatabaseManager.Instance.GetHintForBadge(badgeName, badgeLevel);
    }
    /// <summary>
    /// Deactivates the badge UI display.
    /// </summary>
    public void deactivateBadgeUI()
    {
        Destroy(activeBadgeUI);
    }

    

}
