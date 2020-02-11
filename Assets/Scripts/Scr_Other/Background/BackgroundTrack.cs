using System.Collections.Generic;
using UnityEngine;

public class BackgroundTrack : MonoBehaviour
{
    [SerializeField] private float Speed = 0.1f;

    [SerializeField] private bool OnActive;

    [SerializeField] private Vector3 Direction = new Vector3();

    [SerializeField] private List<Track> PartsTrack = new List<Track>();

    private void Start()
    {
        foreach (var item in PartsTrack)
        {
            item.TrackSettings(Speed, Direction);
        }
    }

    public void Rearrange()
    {
        var firstTrack = PartsTrack[0];
        firstTrack.NewVector(PartsTrack[PartsTrack.Count - 1].GetPoint());
        for (int i = 1; i < PartsTrack.Count; i++)
        {
            PartsTrack[i - 1] = PartsTrack[i];
        }
        PartsTrack[PartsTrack.Count - 1] = firstTrack;
    }
}