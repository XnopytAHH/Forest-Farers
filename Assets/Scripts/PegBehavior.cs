using UnityEditor;
using UnityEngine;

public class PegBehavior : MonoBehaviour
{
    public float progress = 0f;
    public GameObject pegAnchorPoint;
    public void DrivePeg()
    {
        if (progress < 3f)
        {
            progress += 1f;
            Change();
        }
        else
        {
            Debug.Log("Peg is fully driven down.");
        }
    }
    private void Change()
    {
        Debug.Log("Peg progress: " + progress);
        if (progress == 1f)
        {
            
            pegAnchorPoint.transform.position = pegAnchorPoint.transform.position + new Vector3(0f, -0.05f, 0f);
        }
        else if (progress == 2f)
        {
            
            pegAnchorPoint.transform.position = pegAnchorPoint.transform.position + new Vector3(0f, -0.05f, 0f);
        }
        else if (progress >= 3f)
        {
            
            pegAnchorPoint.transform.position = pegAnchorPoint.transform.position + new Vector3(0f, -0.05f, 0f);
        }
    }
}
