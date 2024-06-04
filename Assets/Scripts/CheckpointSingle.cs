using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<AIController>(out AIController player))
        {
            //Debug.Log(other);
            trackCheckpoints.DroneThroughCheckpoint(this,other.transform);
        }
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }
}
