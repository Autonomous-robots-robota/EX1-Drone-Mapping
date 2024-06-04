using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class TrackCheckpoints : MonoBehaviour
{
    private List<CheckpointSingle> checkpointList;
    private List<int> nextCheckpointSingleIndexList;

    [SerializeField] private List<Transform> droneTransformList;

    public event EventHandler<CheckpointEventArgs> OnCorrectCheckpoint;
    public event EventHandler<CheckpointEventArgs> OnIncorrectCheckpoint;

    public class CheckpointEventArgs : EventArgs
    {
        public Transform droneTransform;
    }

    private void Awake()
    {
        //Transform checkpointsTransform = transform.Find("CheckPointSystem");

        checkpointList = new List<CheckpointSingle>();

        foreach (Transform checkpoint in transform)
        {
            //Debug.Log(checkpoint);
            CheckpointSingle checkpointSingle = checkpoint.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointList.Add(checkpointSingle);
        }

        ResetCheckpoint();
    }

    public void DroneThroughCheckpoint(CheckpointSingle checkpoint, Transform droneTransform)
    {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[droneTransformList.IndexOf(droneTransform)];
        if (checkpointList.IndexOf(checkpoint) == nextCheckpointSingleIndex)
        {
            //Debug.Log("Correct");
            OnCorrectCheckpoint?.Invoke(this, new CheckpointEventArgs { droneTransform = droneTransform });
            nextCheckpointSingleIndexList[droneTransformList.IndexOf(droneTransform)] = (nextCheckpointSingleIndex + 1) % checkpointList.Count;
            Debug.Log(toString(nextCheckpointSingleIndexList));
        }
        else
        {
            OnIncorrectCheckpoint?.Invoke(this, new CheckpointEventArgs { droneTransform = droneTransform });
            //Debug.Log("Incorrect");
        }
    }

    public void ResetCheckpoint()
    {
        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform droneTransform in droneTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void ResetCheckpoint(Transform droneTransform)
    {
        nextCheckpointSingleIndexList[droneTransformList.IndexOf(droneTransform)] = 0;
    }

    public CheckpointSingle GetNextCheckpoint(Transform droneTransform)
    {
        return checkpointList[nextCheckpointSingleIndexList[droneTransformList.IndexOf(droneTransform)]];
    }

    private String toString(List<int> list)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            sb.Append(list[i]);
            sb.Append(" ");
        }
        return sb.ToString();
    }
}