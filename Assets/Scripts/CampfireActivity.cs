using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class CampfireActivity : MonoBehaviour
{
    [SerializeField] private List<XRSocketInteractor> sockets; // List of socket interactors to monitor
    [SerializeField] private List<XRSocketInteractor> secondGroupSockets; // Second group of sockets to trigger
    [SerializeField] private CampfireRows[] campfireRows; // Array of CampfireRows to track progress
    [SerializeField] private ParticleSystem fireEffect; // Particle system for fire effect

    private bool firstGroupCompleted = false; // Flag to track if the first group is completed
    private bool activityCompleted = false; // Flag to track if the activity is completed
    private bool usedWetLogs = false; // Flag to track if wet logs were used

    /// <summary>
    /// Called when the script is first initialized. Sets up initial state.
    /// </summary>
    private void Start()
    {
        foreach (CampfireRows row in campfireRows) // For each socket in the second group
        {
            foreach (var socket in row.socketInteractors)
            {
                socket.gameObject.SetActive(false); // Deactivate the socket at the start
            }
        }
        foreach (var socket in campfireRows[0].socketInteractors)
        {
            socket.gameObject.SetActive(true); // Activate the first row of sockets at the start
        }

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
            Debug.Log("Object placed in socket: " + args.interactableObject.transform.name);
            args.interactorObject.transform.GetComponent<MeshRenderer>().enabled = false; // Make the socket non-interactable

            Debug.Log("Object tag: " + grabInteractable.tag);
            if (grabInteractable.tag == "Log")
            {
                grabInteractable.interactionLayers = InteractionLayerMask.GetMask("NonInteractable"); // Change its interaction layer to NonInteractable
                Debug.Log("Changed interaction layer to NonInteractable for " + args.interactableObject.transform.name);
            }
            else if (grabInteractable.tag == "WetLog")
            {
                grabInteractable.interactionLayers = InteractionLayerMask.GetMask("NonInteractable"); // Change its interaction layer to NonInteractable
                Debug.Log("Changed interaction layer to NonInteractable for " + args.interactableObject.transform.name);
                usedWetLogs = true;
            }
            else if (grabInteractable.tag == "LogSkinny")
            {
                grabInteractable.interactionLayers = InteractionLayerMask.GetMask("NonInteractableSkinny"); // Change its interaction layer to NonInteractable
                Debug.Log("Changed interaction layer to NonInteractable for " + args.interactableObject.transform.name);
            }
            else if (grabInteractable.tag == "WetLogSkinny")
            {
                grabInteractable.interactionLayers = InteractionLayerMask.GetMask("NonInteractableSkinny"); // Change its interaction layer to NonInteractable
                Debug.Log("Changed interaction layer to NonInteractable for " + args.interactableObject.transform.name);
                usedWetLogs = true;
            }
        }
        activityCompleted = CheckCompletionStatus(); // Update the activity completion status based on campfire rows
        
    }

    /// <summary>
    /// Checks if all sockets in the primary list are filled.
    /// </summary>
    private bool CheckAllSocketsFilled(XRSocketInteractor[] socketGroup)
    {
        foreach (var socket in socketGroup) // Check each socket in the list 
        {
            if (!socket.hasSelection) // If any socket is empty, return false
                return false;

        }
        return true; // All sockets are filled
    }

    /// <summary>
    /// Checks if all campfire rows are completed.
    /// </summary>
    private bool CheckCompletionStatus()
    {
        foreach (var row in campfireRows)
        {
            if (!row.isCompleted)
            {
                row.isCompleted = CheckAllSocketsFilled(row.socketInteractors);
                if (!row.isCompleted)
                {
                    return false; // If any row is not completed, return false
                }
                else
                {
                    int currentIndex = System.Array.IndexOf(campfireRows, row);
                    if (currentIndex + 1 < campfireRows.Length)
                    foreach (var socket in campfireRows[currentIndex + 1].socketInteractors)
                    {
                        socket.gameObject.SetActive(true); // Activate the next row of sockets
                    }
                    else
                    {
                        fireEffect.gameObject.SetActive(true); // Activate the fire effect game object
                        if (usedWetLogs)
                        {
                            CampRun.Instance.EndCampfireTask("Bronze");
                        }
                        else
                        {
                            if(CampRun.Instance.currentTime <= CampRun.Instance.dayDuration - CampRun.Instance.nightThreshold)
                            {
                                CampRun.Instance.EndCampfireTask("Gold");
                            }
                            else
                            {
                                CampRun.Instance.EndCampfireTask("Silver");
                            }
                        }
                    } 
                    
                }

            }

        }
        return true;
    }

    
}
