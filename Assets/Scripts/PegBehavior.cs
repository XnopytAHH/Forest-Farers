using UnityEditor;
using UnityEngine;

public class PegBehavior : MonoBehaviour
{
    float progress = 0f;
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
        if (progress == 1f)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (progress == 2f)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (progress >= 3f)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
