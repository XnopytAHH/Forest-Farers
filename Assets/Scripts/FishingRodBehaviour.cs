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
    /// Maximum distance the bobber can be from the rod tip.
    /// </summary>
    [SerializeField]
    float maxDistance = 5.0f;
    private void Start() {
        fishingRodRB = GetComponent<Rigidbody>();
        fishingLine = GetComponent<LineRenderer>();
    }
    bool isTrackingEnabled = false;
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
        Uncast();
    }
    private void Update() {
        fishingLine.SetPosition(0, rodTip.transform.position);
        fishingLine.SetPosition(1, fishingRodBobber.transform.position);
        if (isTrackingEnabled)
        {
            
            Vector3 rodVelocity = fishingRodRB.linearVelocity;
            float speed = Vector3.Dot(rodVelocity, transform.forward);
            float angularSpeed = rodTip.GetComponent<Rigidbody>().angularVelocity.magnitude;
            if (speed < 0)  speed = 0;
            if (speed > castThreshold && !isBobberCast)
            {
                Debug.Log("Casting detected with speed: " + speed);
                Cast();
            }
        }
    }
    void Cast()
    {
        //Get angle of the cast
        Vector3 castDirection = gameObject.transform.forward + fishingRodRB.linearVelocity * 0.5f;
        
        //throw bobber using angle of cast
        Debug.Log("Casting in direction: " + castDirection.normalized);
        fishingRodBobber.GetComponent<Rigidbody>().isKinematic = false;
        fishingRodBobber.GetComponent<Rigidbody>().AddForce(castDirection.normalized * 15, ForceMode.VelocityChange);
        fishingRodBobber.GetComponent<Rigidbody>().useGravity = true;
        fishingRodBobber.transform.parent = null;
        isBobberCast = true;

    }
    void Uncast()
    {
        fishingRodBobber.GetComponent<Rigidbody>().isKinematic = true;
        fishingRodBobber.GetComponent<Rigidbody>().useGravity = false;
        fishingRodBobber.transform.parent = rodTip.transform;
        fishingRodBobber.transform.localPosition = new Vector3(0, 0, 0);
        isBobberCast = false;
    }
}
