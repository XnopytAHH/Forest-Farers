/*
* File Name: Bouyancy.cs
* Author: Lim En Xu Jayson
* Date Created: 26/01/2026
* Description: Gives bouyancy to objects in water
*/
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
[RequireComponent(typeof(Rigidbody))]
public class Bouyancy : MonoBehaviour
{
    /// <summary>
    /// Drag applied when the object is underwater.
    /// </summary>
    public float underWaterDrag = 3f;
    /// <summary>
    /// Angular drag applied when the object is underwater.
    /// </summary>
    public float underWaterAngularDrag = 1f;
    /// <summary>
    /// Drag applied when the object is in air.
    /// </summary>
    public float airDrag = 0f;
    /// <summary>
    /// Angular drag applied when the object is in air.
    /// </summary>
    public float airAngularDrag = 0.05f;
    /// <summary>
    /// Bouyancy force applied to the object when underwater.
    /// </summary>
    public float bouyancyForce = 10f;
    /// <summary>
    /// Reference to the water plane GameObject.
    /// </summary>
    [SerializeField]
   GameObject waterPlane;
   /// <summary>
   /// Water level Y position.
   /// </summary>
    float waterLevel;
    /// <summary>
    /// Reference to the Rigidbody component.
    /// </summary>
    Rigidbody rb;
    /// <summary>
    /// Flag to indicate if the object is currently underwater.
    /// </summary>
    public bool isUnderwater = false;
    /// <summary>
    /// Flag to indicate if the object is around water.
    /// </summary>
    public bool aroundWater = false;
    /// <summary>
    /// Offset to adjust water level calculations.
    /// </summary>
    public float waterOffset = 0f;
    /// <summary>
    /// Initializes the Bouyancy component and sets up references.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        waterLevel = waterPlane.transform.position.y;
    }

    /// <summary>
    /// FixedUpdate is called at a fixed interval and is used to apply physics-based updates.
    /// </summary>
    private void FixedUpdate() {
        float levelDifference = transform.position.y - waterLevel + waterOffset;
        if (levelDifference < 0 && aroundWater) // Object is underwater
        {
            rb.AddForceAtPosition(Vector3.up * bouyancyForce * Mathf.Abs(levelDifference), transform.position, ForceMode.Force);
            if (!isUnderwater)
            {
                isUnderwater = true;
                SwitchState(true);
                
            }
        }
        else if (isUnderwater) // Object is above water
        {
            isUnderwater = false;
            SwitchState(false);
        }
    }
    /// <summary>
    /// Switches the object's state between underwater and air, adjusting drag values accordingly.
    /// </summary>
    /// <param name="toUnderwater"></param>
    void SwitchState(bool toUnderwater)
    {
        if (toUnderwater)
        {
            gameObject.GetComponent<AudioPlayer>()?.PlayAudioClip("splash");
            rb.linearDamping = underWaterDrag;
            rb.angularDamping = underWaterAngularDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
            rb.angularDamping = airAngularDrag;
        }
    }
/// <summary>
/// Called when entering a trigger collider. Sets aroundWater flag if entering water plane.
/// </summary>
/// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == waterPlane)
        {
            aroundWater = true;
        }
    }
    /// <summary>
    /// Called when exiting a trigger collider. Unsets aroundWater flag if exiting water plane.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == waterPlane)
        {
            aroundWater = false;
        }
    }
}
