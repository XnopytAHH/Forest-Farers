using UnityEngine;

public class HeightController : MonoBehaviour
{
    GameObject[] UIElements;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIElements = GameObject.FindGameObjectsWithTag("UI Mover");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, GameManager.Instance.currentUser.height / 100f, gameObject.transform.position.z);
        
        foreach (GameObject element in UIElements)
        {
            element.transform.position = new Vector3(element.transform.position.x, gameObject.transform.position.y, element.transform.position.z);
        }
    }
}
