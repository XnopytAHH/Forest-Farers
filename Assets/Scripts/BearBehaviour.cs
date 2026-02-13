/*
* File Name: BearBehaviour.cs
* Author: Emilie Tee Jing Hui
* Date Created: 4/2/2026
* Description: Handles the behaviour of the bear in the tutorial scene.
*/
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BearBehaviour : MonoBehaviour

{
    /// <summary>
    /// Animator component for controlling bear animations.
    /// </summary>
    [SerializeField] private Animator BearAnimator; 
    /// <summary>
    /// The item that the bear will give to the player.
    /// </summary>
    [SerializeField] private GameObject ItemToGive; 
    /// <summary>
    /// The point where the item will be spawned.
    /// </summary>
    [SerializeField] private Transform ItemSpawnPoint;
    /// <summary>
    /// Delay before giving the item.
    /// </summary>
    [SerializeField] private float ItemGiveDelay = 1.0f;
    /// <summary>
    /// Reference to the options menu for updating settings.
    /// </summary> 
    [SerializeField] private OptionsMenu optionsMenu;

    /// <summary>
    /// Initializes the bear's animator and sets the initial state.
    /// </summary>
    private void Start()
    {
        if (BearAnimator == null)
        {
            BearAnimator = GetComponent<Animator>();
        }
        BearAnimator.SetBool("GivenItem", GameManager.Instance.currentUser.finishedTutorial); // Ensure the bear starts without having given the item
    }

    /// <summary>
    /// Called when another collider enters the trigger collider attached to this object.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BearAnimator.SetBool("PlayerNear", true); // Trigger the "PlayerNear" animation state
            if (!BearAnimator.GetBool("GivenItem"))
            {
                StartCoroutine(SpawnItemAfterDelay());
            }
        }
    }

    /// <summary>
    /// Called when another collider exits the trigger collider attached to this object.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BearAnimator.SetBool("PlayerNear", false); // Reset the "PlayerNear" animation state
        }
    }

    /// <summary>
    /// Spawns the item after a specified delay.
    /// </summary>
    private IEnumerator SpawnItemAfterDelay()
    {
        yield return new WaitForSeconds(ItemGiveDelay);
        Instantiate(ItemToGive, ItemSpawnPoint.position, ItemSpawnPoint.rotation);
        BearAnimator.SetBool("GivenItem", true); // Item has been given
        GameManager.Instance.currentUser.finishedTutorial = true;
        optionsMenu.updateOptionsMenu();
    }
}
