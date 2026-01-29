using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    bool isHooked = true;
    public void Unhook()
    {
        isHooked = false;
        transform.SetParent(null);
    }
}
