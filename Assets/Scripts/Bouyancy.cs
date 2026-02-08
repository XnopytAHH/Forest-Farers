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
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float bouyancyForce = 10f;
    [SerializeField]
   GameObject waterPlane;
    float waterLevel;
    Rigidbody rb;
    public bool isUnderwater = false;
    public bool aroundWater = false;
    public float waterOffset = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        waterLevel = waterPlane.transform.position.y;
    }

    // Update is called once per frame
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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == waterPlane)
        {
            aroundWater = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == waterPlane)
        {
            aroundWater = false;
        }
    }
}
