/*
* File Name: BookBehavior.cs
* Author: Lim En Xu Jayson
* Date Created: 08/02/2026
* Description: Handles the behavior of the instruction book in the game.
*/
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BookBehavior : MonoBehaviour
{
    /// <summary>
    /// Current page index of the book.
    /// </summary>
    public int currentPage =0;
    /// <summary>
    /// Array of page group GameObjects.
    /// </summary>
    public GameObject[] pageGroups;
    /// <summary>
    /// Next page button GameObject.
    /// </summary>
    public GameObject nextButton;
    /// <summary>
    /// Previous page button GameObject.
    /// </summary>
    public GameObject previousButton;
    /// <summary>
    /// Model for the closed book.
    /// </summary>
    [SerializeField] public GameObject bookClosed;
    /// <summary>
    ///   Model for the open book.
    /// </summary>
    [SerializeField] public GameObject bookOpen;
    /// <summary>
    /// Tick mark for camping task.
    /// </summary>
    [SerializeField] public GameObject campingTick;
    /// <summary>
    /// Tick mark for fishing task.
    /// </summary>
    [SerializeField] public GameObject fishingTick;
    /// <summary>
    /// Tick mark for cooking task.
    /// </summary>
    [SerializeField] public GameObject cookingTick;
    /// <summary>
    /// Tick mark for campfire task.
    /// </summary>
    [SerializeField] public GameObject campfireTick;
    /// <summary>
    /// Blocker object to prevent interaction in certain scenes.
    /// </summary>
    [SerializeField] public GameObject Blocker;
    
    void Awake()
    {
        bookClosed.SetActive(true);
        bookOpen.SetActive(false);
    }
    public void PickedUp()
    {
        gameObject.GetComponent<AudioPlayer>()?.PlayAudioClip("pageturn");
        bookClosed.SetActive(false);
        bookOpen.SetActive(true);
        UpdatePages();
        gameObject.GetComponent<OptionsMenu>().updateOptionsMenu();
    }
    public void PutDown()
    {
        bookClosed.SetActive(true);
        bookOpen.SetActive(false);
    }
    void UpdatePages()
    {
        for (int i = 0; i < pageGroups.Length; i++)
        {
            pageGroups[i].SetActive(i == currentPage);
        }
        if (currentPage == 0)
        {
            previousButton.SetActive(false);
            TextMeshProUGUI nameField = GameObject.FindGameObjectWithTag("Name").GetComponent<TextMeshProUGUI>();
            nameField.text = GameManager.Instance.currentUser.username;
            TextMeshProUGUI highscoreField = GameObject.FindGameObjectWithTag("Highscore").GetComponent<TextMeshProUGUI>();
            highscoreField.text = GameManager.Instance.currentUser.highscore.ToString();
            TextMeshProUGUI heightField = GameObject.FindGameObjectWithTag("Height").GetComponent<TextMeshProUGUI>();
            heightField.text = GameManager.Instance.currentUser.height.ToString() + " cm";
            TextMeshProUGUI campCountField = GameObject.FindGameObjectWithTag("CampCount").GetComponent<TextMeshProUGUI>();
            campCountField.text = $"I have been on {GameManager.Instance.currentUser.campCount} camps!";
        }
        else
        {
            previousButton.SetActive(true);
        }
        if (currentPage == pageGroups.Length - 1)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }
        if (currentPage==1)
        {
            if(SceneManager.GetActiveScene().name=="MenuScene") 
            {
                Blocker.SetActive(true);
            }
            else
            {
                Blocker.SetActive(false);
                if (CampRun.Instance.cookingFinished)
            {
                cookingTick.SetActive(true);
            }
            else
            {
                cookingTick.SetActive(false);
            }
            if (CampRun.Instance.fishingFinished)
            {
                fishingTick.SetActive(true);
            }
            else
            {
                fishingTick.SetActive(false);
            }
            if (CampRun.Instance.tentFinished)
            {
                campingTick.SetActive(true);
            }
            else
            {
                campingTick.SetActive(false);
            }
            if (CampRun.Instance.campfireFinished)
            {
                campfireTick.SetActive(true);
            }
            else
            {
                campfireTick.SetActive(false);
            }
            }
            
        }
    }
    public void NextPage()
    {
        gameObject.GetComponent<AudioPlayer>()?.PlayAudioClip("pageturn");
        if (currentPage < pageGroups.Length - 1)
        {
            currentPage++;
            UpdatePages();
        }
    }
    public void PreviousPage()
    {
        gameObject.GetComponent<AudioPlayer>()?.PlayAudioClip("pageturn");
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePages();
        }
    }


}