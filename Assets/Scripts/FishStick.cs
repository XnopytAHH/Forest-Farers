using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FishStick : MonoBehaviour
{
    public void attachFish(XRSocketInteractor socket)
    {
        Debug.Log("Fish attached to stick");
        GameObject fish = socket.GetOldestInteractableSelected().transform.gameObject;
        FishBehaviour fishBehaviour = fish.GetComponent<FishBehaviour>();
        fishBehaviour.onStick = true;
        
        fishBehaviour.showUI();
        
    }
    public void detachFish(XRSocketInteractor socket)
    {
        Debug.Log("Fish detached from stick");
        GameObject fish = socket.GetOldestInteractableSelected().transform.gameObject;
        FishBehaviour fishBehaviour = fish.GetComponent<FishBehaviour>();
        fishBehaviour.onStick = false;
        fishBehaviour.hideUI();
    }
    public void disableTask()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
