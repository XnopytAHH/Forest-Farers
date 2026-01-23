using UnityEngine;

public class hammerBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject hammerHead;
    Vector3 offset;
    bool canDrive = false;
    bool isTrackingEnabled = false;
    void Start()
    {
        if (hammerHead == null)
        {
            Debug.LogError("Hammer head not assigned in the inspector.");
        }
        offset = hammerHead.transform.localPosition;
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
    }
    void FixedUpdate()
    {
        hammerHead.GetComponent<Rigidbody>().MovePosition(gameObject.transform.TransformPoint(offset));
        hammerHead.GetComponent<Rigidbody>().MoveRotation(gameObject.transform.rotation);
    
    }

    void Update()
    {
        if (isTrackingEnabled)
        {
            Vector3 hammerVelocity = hammerHead.GetComponent<Rigidbody>().linearVelocity;
            float speed = Vector3.Dot(hammerVelocity, -transform.right);
            if (speed > 5f && !canDrive)
            {
                canDrive = true;
                Debug.Log("Hammer is in driving position with speed: " + speed);
            }
            else if (speed <= 5f && canDrive)
            {
                canDrive = false;
                Debug.Log("Hammer is no longer in driving position.");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Peg") && canDrive)
        {
            collision.gameObject.GetComponent<PegBehavior>().DrivePeg();
        }
    }
}
