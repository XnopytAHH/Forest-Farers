using UnityEngine;
using System.Collections;
public class TentBagBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject[] tentItems;
    [SerializeField]
    GameObject tentObject;
    [SerializeField]
    GameObject spaceTester;
    bool thrownDown = false;
    bool waitingToPitch = false;
    void Start()
    {
        tentObject.SetActive(false);
        
        spaceTester.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (thrownDown)
        {
        if (GetComponent<Rigidbody>().linearVelocity.y == 0)
        {
            spaceTester.SetActive(true);
            if (!waitingToPitch)
            {
                waitingToPitch = true;
                StartCoroutine(pitchTentTimer());
            }

        }
        else
        {
            spaceTester.SetActive(false);
        }
        }
    }
    public void ThrowDownTentBag()
    {
        thrownDown = true;
        gameObject.transform.rotation = Quaternion.Euler(0f, gameObject.transform.rotation.eulerAngles.y, 0f);
        
    }
    public void TentBagPickedUp()
    {
        thrownDown = false;
        spaceTester.SetActive(false);
        StopAllCoroutines();
        waitingToPitch = false;
    }
    public void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")&& !other.gameObject.CompareTag("CampfireVolume"))
        {
        Debug.Log("Trigger stay detected with " + other.gameObject.name);
        StopAllCoroutines();
        waitingToPitch = false;
        }
        
    }
    IEnumerator pitchTentTimer()
    {
        yield return new WaitForSeconds(4f);
        tentObject.SetActive(true);
        tentObject.transform.parent = null;
        foreach (GameObject item in tentItems)
            {
                item.SetActive(true);
                GameObject tentItem=Instantiate(item, transform.position + new Vector3(3f, 0f, 0f), Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f));
                tentItem.SetActive(true);
            }
        gameObject.SetActive(false);
        waitingToPitch = false;
    }
}
