/*
* File Name: FishInWaterBehaviour.cs
* Author: Lim En Xu Jayson
* Date Created: 08/02/2026
* Description: Behaviour for fish swimming towards the bobber.
*/
using System.Collections;
using UnityEngine;

public class FishInWaterBehaviour : MonoBehaviour
{
    /// <summary>
    /// Direction vector towards the bobber.
    /// </summary>
    Vector3 directionToBobber;
    /// <summary>
    /// Reference to the fishing rod GameObject.
    /// </summary>
    public GameObject fishingRod;
    /// <summary>
    /// Reference to the bobber GameObject.
    /// </summary>
    public GameObject bobber;
    /// <summary>
    /// Speed of the fish swimming towards the bobber.
    /// </summary>
    public float speed = 2f;
    /// <summary>
    /// Flag to indicate if the fish has bitten the bobber.
    /// </summary>
    public bool bited = false;
    /// <summary>
    /// Subscribes to the rodUncast event to destroy the fish when the rod is uncast.
    /// </summary>
    void OnEnable()
    {
        FishingRodBehaviour.rodUncast += destroyFish;
    }
    /// <summary>
    /// Unsubscribes from the rodUncast event when disabled.
    /// </summary>
    void OnDisable()
    {
        FishingRodBehaviour.rodUncast -= destroyFish;
    }
    /// <summary>
    /// On Start, gets the bobber reference from the fishing rod.
    /// </summary>
    void Start()
    {
        bobber = fishingRod.GetComponent<FishingRodBehaviour>().fishingRodBobber;
    }

    /// <summary>
    /// Updates the fish's position towards the bobber each frame.
    /// </summary>
    void Update()
    {
        if (bobber == null)
        {
            return;
        }
        else
        {
            directionToBobber = (bobber.transform.position - transform.position).normalized;
            transform.position += directionToBobber * speed * Time.deltaTime;
            transform.LookAt(bobber.transform);
            if (Vector3.Distance(transform.position, bobber.transform.position) < 0.5f && !bited)
            {
                bited = true;
                fishingRod.GetComponent<FishingRodBehaviour>().FishBite();
            }
        }
    }
    /// <summary>
    /// Destroys the fish object when the rod is uncast.
    /// </summary>
    public void destroyFish()
    {
        Destroy(this.gameObject);
    }
}
