/*
* File Name: PegSocketBehaviour.cs
* Author: Jayson Lim En Xu
* Date Created: 01/02/2026
* Description: Manages the behavior of a peg socket in the game.
*/
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PegSocketBehaviour : MonoBehaviour
{
    /// <summary>
    /// Handles the event when a peg is added to the socket.
    /// </summary>
    /// <param name="args"></param>
    public void PegAddedToSocket(SelectEnterEventArgs args)
    {
        Debug.Log("Peg added to socket.");
        if (args.interactableObject.transform.TryGetComponent<XRGrabInteractable>(out var grabInteractable))
        {
            Debug.Log("Object placed in socket: " + args.interactableObject.transform.name);

            Debug.Log("Object tag: " + grabInteractable.tag);
            int layerToRemove = InteractionLayerMask.GetMask("Default");
            grabInteractable.interactionLayers &= ~layerToRemove;
            args.interactableObject.transform.GetComponent<PegBehavior>().pegAnchorPoint = gameObject.transform.GetChild(0).gameObject;

        }
        
        
    }
    
        
    
    
    
}
