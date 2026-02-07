using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class EndScreenBadge : MonoBehaviour
{
    [SerializeField]
    Sprite badgeImage;
    [SerializeField]
    Sprite[] badgeSprites; // 0: Bronze, 1: Silver, 2: Gold
    Image stitch;
    Image image;
    [SerializeField]
    GameObject badgeUIPrefab;
    GameObject activeBadgeUI;
    public int badgeLevel;
    private string badgeLevelName;
    private void Start()
    {
        stitch = transform.GetChild(0).GetComponent<Image>();
        image = gameObject.GetComponent<Image>();
        if (gameObject.name.Contains("Camper")) badgeLevel= GameManager.Instance.endDayScores[1];
        if (gameObject.name.Contains("Backwoodsman")) badgeLevel= GameManager.Instance.endDayScores[2];
        if (gameObject.name.Contains("Angler")) badgeLevel= GameManager.Instance.endDayScores[3];
        if (gameObject.name.Contains("Cook")) badgeLevel= GameManager.Instance.endDayScores[4];
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
    public void activateBadgeUI()
    {
        activeBadgeUI = Instantiate(badgeUIPrefab, transform.position, Quaternion.identity, transform);
        activeBadgeUI.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        activeBadgeUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name;
        activeBadgeUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = badgeLevelName;
        activeBadgeUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Badge Achieved!";
    }
    public void deactivateBadgeUI()
    {
        Destroy(activeBadgeUI);
    }

    

}
