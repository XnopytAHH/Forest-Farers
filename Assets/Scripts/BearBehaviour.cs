using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BearBehaviour : MonoBehaviour
{
    [SerializeField] private Animator BearAnimator; // Reference to the bear's animator component
    [SerializeField] private GameObject ItemToGive; // The item that the bear will give to the player
    [SerializeField] private Transform ItemSpawnPoint; // Point where the item will be spawned
    [SerializeField] private float ItemGiveDelay = 1.0f; // Delay before giving the item

    /// <summary>
    /// Initializes the bear's animator and sets the initial state.
    /// </summary>
    private void Start()
    {
        if (BearAnimator == null)
        {
            BearAnimator = GetComponent<Animator>();
        }
        BearAnimator.SetBool("GivenItem", false); // Ensure the bear starts without having given the item
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
    }
}
