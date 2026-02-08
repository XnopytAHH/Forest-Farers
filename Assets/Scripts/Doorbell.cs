using UnityEngine;

public class Doorbell : MonoBehaviour
{
    private bool isPressed = false;
    public void SetCampfireButton()
    {
        if (isPressed) return;
        isPressed = true;
        gameObject.GetComponent<AudioPlayer>().PlayAudioClip("doorbell");
        CampRun.Instance.currentTime= CampRun.Instance.dayDuration -2f;
    }
}
