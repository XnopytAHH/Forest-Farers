/*
* File Name: FishingRodBehaviour.cs
* Author: Lim En Xu Jayson
* Date Created: 21/01/2026
* Description: Fishing rod behaviour including casting mechanics.
*/
using UnityEngine;
using System.Collections;
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
    /// Threshold angular speed to detect a cast action.
    /// </summary>
    [SerializeField]
    float angularCastThreshold = 1.0f;
    /// <summary>
    /// Array to store previous positions of the rod tip for casting direction calculation.
    /// </summary>
    public Vector3[] previousPositions = new Vector3[4];
    /// <summary>
    /// Reference to the fishing rod bobber object.
    /// </summary>
    [SerializeField]
    GameObject fishingRodBobber;
    /// <summary>
    /// Bool to check if the bobber is currently cast.
    /// </summary>
    public bool isBobberCast = false;
    /// <summary>
    /// Bool to check if the bobber is currently reeling in.
    /// </summary>
    public bool isReelingIn = false;
    /// <summary>
    /// Maximum distance the bobber can be from the rod tip.
    /// </summary>
    [SerializeField]
    float maxDistance;
    Vector3 offset;
    bool isTrackingEnabled = false;
    private void Start() {
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
        previousPositions = new Vector3[4];
        Debug.Log("Disabling tracking");
        isTrackingEnabled = false;
        ReturnToRod();
    }
    private void FixedUpdate() {
        rodTip.GetComponent<Rigidbody>().MovePosition(gameObject.transform.TransformPoint(offset));
        rodTip.GetComponent<Rigidbody>().MoveRotation(gameObject.transform.rotation);
    }
    private void Update() {
        fishingLine.SetPosition(0, rodTip.transform.position);
        fishingLine.SetPosition(1, fishingRodBobber.transform.position);
        if (isTrackingEnabled && !isBobberCast && !isReelingIn)
        {
            
            Vector3 rodVelocity = fishingRodRB.linearVelocity;
            float speed = Vector3.Dot(rodVelocity, transform.forward);
            if (speed < 0)  speed = 0;
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
            if (speed > 0)  speed = 0;
            
            if (-speed > castThreshold)
            {
                Debug.Log("Casting detected with speed: " + speed);
                Uncast();
            }
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

    }
    void Uncast()
    {
        Vector3 returnDirection = (rodTip.transform.position - fishingRodBobber.transform.position).normalized;
        isReelingIn = true;
        StartCoroutine(ReelInBobber());
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

}
