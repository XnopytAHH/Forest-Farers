using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class CampfireActivity : MonoBehaviour
{
    [SerializeField] private List<XRSocketInteractor> sockets; // List of socket interactors to monitor
    [SerializeField] private List<XRSocketInteractor> secondGroupSockets; // Second group of sockets to trigger
    [SerializeField] private ParticleSystem fireEffect; // Particle system for fire effect

    private bool firstGroupCompleted = false; // Flag to track if the first group is completed
    private bool activityCompleted = false; // Flag to track if the activity is completed

    /// <summary>
    /// Called when the script is first initialized. Sets up initial state.
    /// </summary>
    private void Start()
    {
        // Initially deactivate the second group of sockets
        sockets.ForEach(socket => socket.gameObject.SetActive(true));
        secondGroupSockets.ForEach(socket => socket.gameObject.SetActive(false));

        fireEffect.gameObject.SetActive(false); // Ensure fire effect is off at start
        activityCompleted = false; // Activity is not completed at start
    }

    /// <summary>
    /// Called when the script is enabled. Sets up event listeners for socket interactions.
    /// </summary>
    private void OnEnable()
    {
        foreach (var socket in sockets) // For each socket in the list
        {
            socket.selectEntered.AddListener(OnSocketChanged); // Add listener for when an item is placed in the socket
        }
        foreach (var socket in secondGroupSockets) // For each socket in the list
        {
            socket.selectEntered.AddListener(OnSocketChanged); // Add listener for when an item is placed in the socket
          
        }
    }

    /// <summary>
    /// Called when the script is disabled. Cleans up event listeners for socket interactions.
    /// </summary>
    private void OnDisable() // used to clean up event listeners
    {
        foreach (var socket in sockets)
        {
            socket.selectEntered.RemoveListener(OnSocketChanged); // Remove listener when disabled so that it doesn't take actions when not needed
        }
        foreach (var socket in secondGroupSockets)
        {
            socket.selectEntered.RemoveListener(OnSocketChanged); // Remove listener when disabled so that it doesn't take actions when not needed
        }
    }


    /// <summary>
    /// Called when an item is placed in any of the monitored sockets. Plays fire effect when all sockets are filled.
    /// </summary>
    private void OnSocketChanged(SelectEnterEventArgs args)
    {
        // Change interaction layer on the object placed in the socket, preventing it from being grabbed again
        if (args.interactableObject.transform.TryGetComponent<XRGrabInteractable>(out var grabInteractable)) // Get the grab interactable component of the placed object
        {
            grabInteractable.interactionLayers = InteractionLayerMask.GetMask("NonInteractable"); // Change its interaction layer to NonInteractable
            Debug.Log("Changed interaction layer to NonInteractable for " + args.interactableObject.transform.name);
        }

        if (!firstGroupCompleted)
        {
            if (CheckAllSocketsFilled(sockets)) // When a socket has an item placed inside, check if all sockets are filled
            {
                secondGroupSockets.ForEach(socket => socket.gameObject.SetActive(true)); // Activate the second group of sockets
                firstGroupCompleted = true; // Mark the first group as completed
                sockets.ForEach(socket => socket.gameObject.GetComponent<MeshRenderer>().enabled = false); // Make the first group of sockets non-interactable
            }
        }
        else if (!activityCompleted)
        {
            if (CheckAllSocketsFilled(secondGroupSockets)) // Check if all sockets in the second group are filled
            {
                secondGroupSockets.ForEach(socket => socket.gameObject.GetComponent<MeshRenderer>().enabled = false); // Make the second group of sockets non-interactable
                fireEffect.gameObject.SetActive(true); // Activate the fire effect game object
                activityCompleted = true; // Mark the activity as completed
            }
        }
    }


    
    /// <summary>
    /// Checks if all sockets in the primary list are filled.
    /// </summary>
    private bool CheckAllSocketsFilled(List<XRSocketInteractor> socketGroup)
    {
        foreach (var socket in socketGroup) // Check each socket in the list 
        {
            if (!socket.hasSelection) // If any socket is empty, return false
                return false;
                
        }
        return true; // All sockets are filled
    }
    
}
