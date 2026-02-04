/*
* File Name: FishingRodBehaviour.cs
* Author: Lim En Xu Jayson
* Date Created: 21/01/2026
* Description: Fishing rod behaviour including casting mechanics.
*/
using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using System;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.Rendering;
public class FishingRodBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the tip of the fishing rod. Used for calculating casting direction.
    /// </summary>
    [SerializeField]
    GameObject rodTip;
    /// <summary>
    /// Rigidbody of the fishing rod for velocity calculations.
    /// </summary>
    Rigidbody fishingRodRB;
    /// <summary>
    /// Line renderer for the fishing line visualization.
    /// </summary>
    LineRenderer fishingLine;
    /// <summary>
    /// Threshold speed to detect a cast action.
    /// </summary>
    [SerializeField]
    float castThreshold = 2.0f;
    /// <summary>
    /// Reference to the fishing rod bobber object.
    /// </summary>
    [SerializeField]
    public GameObject fishingRodBobber;
    /// <summary>
    /// Bool to check if the bobber is currently cast.
    /// </summary>
    public bool isBobberCast = false;
    /// <summary>
    /// Bool to check if the bobber is currently reeling in.
    /// </summary>
    public bool isReelingIn = false;
    ///<summary>
    /// Bool to check if the bobber is waiting for a fish to bite.
    /// </summary>
    public bool isWaitingForBite = false;
    /// <summary>
    /// Maximum distance the bobber can be from the rod tip.
    /// </summary>
    [SerializeField]
    float maxDistance;
    /// <summary>
    /// Prefab of the fish in the water.
    /// </summary>
    [SerializeField]
    GameObject fishInWaterPrefab;
    /// <summary>
    /// Prefab of the fish to spawn when a fish bites.
    /// </summary>
    [SerializeField]
    GameObject fishPrefab;
    /// <summary>
    /// Event triggered when the fishing rod is uncast.
    /// </summary>
    public static event Action rodUncast;

    /// <summary>
    /// Offset Bounds for the fish spawn position.
    /// </summary>
    public Vector2 fishSpawnOffsetBounds;

    Vector3 offset;
    bool isTrackingEnabled = false;
    public GameObject currentFish;
    public GameObject currentFishInWater;
    public Coroutine escapeCoroutine;
    private void Start()
    {
        fishingRodRB = GetComponent<Rigidbody>();
        fishingLine = GetComponent<LineRenderer>();
        offset = rodTip.transform.localPosition;
    }

    public void enableTracking()
    {
        Debug.Log("Enabling tracking");
        isTrackingEnabled = true;
    }
    public void disableTracking()
    {
        Debug.Log("Disabling tracking");
        isTrackingEnabled = false;
        ReturnToRod();
        isWaitingForBite = false;

        rodUncast?.Invoke();
        StopAllCoroutines();
    }
    private void FixedUpdate()
    {
        rodTip.GetComponent<Rigidbody>().MovePosition(gameObject.transform.TransformPoint(offset));
        rodTip.GetComponent<Rigidbody>().MoveRotation(gameObject.transform.rotation);
    }
    private void Update()
    {
        fishingLine.SetPosition(0, rodTip.transform.position);
        fishingLine.SetPosition(1, fishingRodBobber.transform.position);
        if (isTrackingEnabled && !isBobberCast && !isReelingIn)
        {

            Vector3 rodVelocity = fishingRodRB.linearVelocity;
            float speed = Vector3.Dot(rodVelocity, transform.forward);
            if (speed < 0) speed = 0;
            if (speed > castThreshold && !isBobberCast)
            {
                Debug.Log("Casting detected with speed: " + speed);
                Cast(speed);
            }
        }
        if (isTrackingEnabled && isBobberCast && !isReelingIn)
        {
            Vector3 rodVelocity = fishingRodRB.linearVelocity;
            float speed = Vector3.Dot(rodVelocity, transform.forward);
            if (speed > 0) speed = 0;

            if (-speed > castThreshold)
            {
                Uncast();
            }
        }
        if (fishingRodBobber.GetComponent<Bouyancy>().isUnderwater && !isWaitingForBite && isBobberCast)
        {
            isWaitingForBite = true;
            StartCoroutine(SpawnFishAfterDelay(UnityEngine.Random.Range(3f, 8f)));

        }


    }
    void Cast(float velocity)
    {
        //Get angle of the cast
        Vector3 castDirection = gameObject.transform.forward + fishingRodRB.linearVelocity * 0.5f;
        fishingRodBobber.GetComponent<ConfigurableJoint>().linearLimit = new SoftJointLimit { limit = maxDistance };
        //throw bobber using angle of cast
        Debug.Log("Casting in direction: " + castDirection.normalized);
        fishingRodBobber.GetComponent<Rigidbody>().isKinematic = false;
        fishingRodBobber.GetComponent<Rigidbody>().useGravity = true;
        fishingRodBobber.transform.parent = null;
        isBobberCast = true;
        // fishingRodBobber.transform.rotation = quaternion.identity;
        //fishingRodBobber.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

    }
    void Uncast()
    {
        Vector3 returnDirection = (rodTip.transform.position - fishingRodBobber.transform.position).normalized;
        isReelingIn = true;
        isWaitingForBite = false;
        StopAllCoroutines();
        StartCoroutine(ReelInBobber());
        //fishingRodBobber.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        rodUncast?.Invoke();
        if (escapeCoroutine != null)
        {
            StopCoroutine(escapeCoroutine);
        }
        escapeCoroutine = null;
    }
    void ReturnToRod()
    {
        fishingRodBobber.GetComponent<ConfigurableJoint>().linearLimit = new SoftJointLimit { limit = 0.4f };
        isBobberCast = false;

    }
    IEnumerator ReelInBobber()
    {
        float distanceLeft = maxDistance;
        while (distanceLeft > 1)
        {
            distanceLeft -= 0.3f;
            fishingRodBobber.GetComponent<ConfigurableJoint>().linearLimit = new SoftJointLimit { limit = distanceLeft };
            yield return new WaitForSeconds(0.01f);
        }
        ReturnToRod();
        yield return new WaitForSeconds(0.5f);
        isReelingIn = false;
    }

    IEnumerator SpawnFishAfterDelay(float delay)
    {
        // Wait for 1 second to ensure bobber is stable in water
        yield return new WaitForSeconds(1f);
        if (!fishingRodBobber.GetComponent<Bouyancy>().isUnderwater)
        {
            yield break;
        }
        yield return new WaitForSeconds(delay);
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(fishSpawnOffsetBounds.x, fishSpawnOffsetBounds.y), 0, UnityEngine.Random.Range(fishSpawnOffsetBounds.x, fishSpawnOffsetBounds.y));
        Vector3 spawnPosition = fishingRodBobber.transform.position + randomOffset;
        currentFishInWater = Instantiate(fishInWaterPrefab, spawnPosition, Quaternion.identity);
        currentFishInWater.GetComponent<FishInWaterBehaviour>().fishingRod = gameObject;
    }
    public void FishBite()
    {
        currentFish = Instantiate(fishPrefab, fishingRodBobber.transform.position, Quaternion.identity);
        escapeCoroutine = StartCoroutine(escapeTimer());    
    }
    IEnumerator escapeTimer()
    {
        yield return new WaitForSeconds(5f);
        currentFish.GetComponent<FishBehaviour>().Escape();
        currentFishInWater.GetComponent<FishInWaterBehaviour>().destroyFish();
        isWaitingForBite = false;
    }

}
