using UnityEngine;

public class StopTrack : MonoBehaviour
{
    private BackgroundTrack backgroundTrack;

    private void Awake()
    {
        backgroundTrack = GetComponentInParent<BackgroundTrack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Track>()) backgroundTrack.Rearrange();
    }
}