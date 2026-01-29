using System.Collections;
using UnityEngine;

public class FishInWaterBehaviour : MonoBehaviour
{
    Vector3 directionToBobber;
    public GameObject fishingRod;
    public GameObject bobber;
    public float speed = 2f;
    public bool bited = false;
    void OnEnable()
    {
        FishingRodBehaviour.rodUncast += destroyFish;
    }
    void OnDisable()
    {
        FishingRodBehaviour.rodUncast -= destroyFish;
    }
    void Start()
    {
        bobber = fishingRod.GetComponent<FishingRodBehaviour>().fishingRodBobber;
    }

    // Update is called once per frame
    void Update()
    {
        if (bobber == null)
        {
            return;
        }
        else
        {
            directionToBobber = (bobber.transform.position - transform.position).normalized;
            transform.position += directionToBobber * speed * Time.deltaTime;
            transform.LookAt(bobber.transform);
            if (Vector3.Distance(transform.position, bobber.transform.position) < 0.5f && !bited)
            {
                bited = true;
                fishingRod.GetComponent<FishingRodBehaviour>().FishBite();
            }
        }
    }
    void destroyFish()
    {
        Destroy(this.gameObject);
    }
}
