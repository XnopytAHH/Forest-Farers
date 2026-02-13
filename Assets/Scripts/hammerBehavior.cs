/*
* File Name: hammerBehavior.cs
* Author: Lim En Xu Jayson
* Date Created: 23/01/2026
* Description: Handles hammer behavior for driving pegs into the ground.
*/
using UnityEngine;

public class hammerBehavior : MonoBehaviour
{
    /// <summary>
    /// Reference to the hammer head GameObject.
    /// </summary>
    [SerializeField]
    GameObject hammerHead;
    /// <summary>
    /// Offset of the hammer head relative to the hammer handle.
    /// </summary>
    Vector3 offset;
    /// <summary>
    /// Flag to indicate if the hammer can drive a peg.
    /// </summary>
    bool canDrive = false;
    /// <summary>
    /// Flag to indicate if the hammer is currently contacting a peg.
    /// </summary>
    bool isContactingPeg = false;
    /// <summary>
    /// Flag to indicate if tracking is enabled.
    /// </summary>
    bool isTrackingEnabled = false;
    /// <summary>
    /// Speed threshold to determine if the hammer is in driving position.
    /// </summary>
    [SerializeField]
    float speedThreshold = 5f;

    /// <summary>
    /// Initializes the hammer behavior and sets up references.
    /// </summary>
    void Start()
    {
        if (hammerHead == null)
        {
            Debug.LogError("Hammer head not assigned in the inspector.");
        }
        offset = hammerHead.transform.localPosition;
    }
    /// <summary>
    /// Enables tracking for the hammer.
    /// </summary>
    public void enableTracking()
    {
        Debug.Log("Enabling tracking");
        isTrackingEnabled = true;
    }
    /// <summary>
    /// Disables tracking for the hammer.
    /// </summary>
    public void disableTracking()
    {
        Debug.Log("Disabling tracking");
        isTrackingEnabled = false;
    }
    /// <summary>
    /// Updates the hammer head position and checks for driving conditions.
    /// </summary>
    void FixedUpdate()
    {
        hammerHead.GetComponent<Rigidbody>().MovePosition(gameObject.transform.TransformPoint(offset));
        hammerHead.GetComponent<Rigidbody>().MoveRotation(gameObject.transform.rotation);
        if (isTrackingEnabled)
        {
            Vector3 hammerVelocity = hammerHead.GetComponent<Rigidbody>().linearVelocity;
            float speed = Vector3.Dot(hammerVelocity, -transform.right);
            
            if (speed > speedThreshold && !canDrive)
            {
                canDrive = true;
                Debug.Log("Hammer is in driving position with speed: " + speed);
            }
            if (speed <= speedThreshold && canDrive)
            {
                canDrive = false;
                Debug.Log("Hammer is no longer in driving position.");
            }
        }
    }

    /// <summary>
    /// Handles collision with pegs to drive them into the ground.
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Peg") && canDrive)
        {
            if(!isContactingPeg)
            {
                isContactingPeg = true;
                collision.gameObject.GetComponent<PegBehavior>().DrivePeg();
                Debug.Log("Hammer collided with peg while in driving position.");
            }
            
        }
    }
    /// <summary>
    /// Handles collision exit with pegs to prevent multiple drives.
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Peg"))
        {
            isContactingPeg = false;
        }
    }
}
