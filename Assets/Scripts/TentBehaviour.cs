using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TentBehaviour : MonoBehaviour
{
    [SerializeField]
    XRSocketInteractor[] pegPoints;
    int[] completion = new int[4];
    void OnEnable()
    {
        PegBehavior.pegDriven += () => CampRun.Instance.EndCampingTask(CheckTentBadge().ToString());
    }
    void OnDisable()
    {
        PegBehavior.pegDriven -= () => CampRun.Instance.EndCampingTask(CheckTentBadge().ToString());
    }
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
