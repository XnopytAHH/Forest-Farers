using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PegSocketBehaviour : MonoBehaviour
{
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
