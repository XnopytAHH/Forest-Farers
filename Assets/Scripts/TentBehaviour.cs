/*
* File Name: TentBehaviour.cs
* Author: Jayson Lim En Xu
* Date Created: 04/02/2026
* Description: Manages the behavior of a tent in the game.
*/
using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TentBehaviour : MonoBehaviour
{
    /// <summary>
    /// Array of XRSocketInteractors representing the peg points of the tent.
    /// </summary>
    [SerializeField]
    XRSocketInteractor[] pegPoints;
    /// <summary>
    /// Array to track completion status of each peg.
    /// </summary>
    int[] completion = new int[4];
    /// <summary>
    /// Subscribes to the pegDriven event when enabled to check tent badge completion.
    /// </summary>
    void OnEnable()
    {
        PegBehavior.pegDriven += () => CampRun.Instance.EndCampingTask(CheckTentBadge().ToString());
    }
    /// <summary>
    /// Unsubscribes from the pegDriven event when disabled.
    /// </summary>
    void OnDisable()
    {
        PegBehavior.pegDriven -= () => CampRun.Instance.EndCampingTask(CheckTentBadge().ToString());
    }
    /// <summary>
    /// Checks the tent badge completion based on the driven pegs.
    /// </summary>
    /// <returns></returns>
    public int CheckTentBadge()
    {
        int count = 0;
        foreach (XRSocketInteractor peg in pegPoints)
        {
            if (peg.GetOldestInteractableSelected() == null)
            {
                completion[count] = 0;
            }
            else
            {
                GameObject pegObject = peg.GetOldestInteractableSelected().transform.gameObject;
                completion[count] = (int)pegObject.GetComponent<PegBehavior>().progress;
            }
            count++;
        }
         int total = 0;
        for (int i = 0; i < completion.Length; i++)
        {
           
            if (completion[i] == 0)
            {
                Debug.Log("Tent peg " + i + " is not driven in."); 

                return 1;
            }
            else
            {
                total += completion[i];
            }
        }
        Debug.Log("Tent peg completion total: " + total);
        if (total ==12)
        {
            return 3;
        }
        else if (total >= 8)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
}
