/*
* File Name: FishStick.cs
* Author: Lim En Xu Jayson
* Date Created: 08/02/2026
* Description: Handles fish cooking stick interactions for attaching and detaching fish.
*/
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FishStick : MonoBehaviour
{
    /// <summary>
    /// Attaches a fish to the stick when placed in the socket. Also shows the fish UI.
    /// </summary>
    /// <param name="socket"></param>
    public void attachFish(XRSocketInteractor socket)
    {
        Debug.Log("Fish attached to stick");
        GameObject fish = socket.GetOldestInteractableSelected().transform.gameObject;
        FishBehaviour fishBehaviour = fish.GetComponent<FishBehaviour>();
        fishBehaviour.onStick = true;
        
        fishBehaviour.showUI();
        
    }
    /// <summary>
    /// Detaches a fish from the stick when removed from the socket. Also hides the fish UI.
    /// </summary>
    /// <param name="socket"></param>
    public void detachFish(XRSocketInteractor socket)
    {
        Debug.Log("Fish detached from stick");
        GameObject fish = socket.GetOldestInteractableSelected().transform.gameObject;
        FishBehaviour fishBehaviour = fish.GetComponent<FishBehaviour>();
        fishBehaviour.onStick = false;
        fishBehaviour.hideUI();
    }
    /// <summary>
    /// Disables the fish stick task UI element.
    /// </summary>
    public void disableTask()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
