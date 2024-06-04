using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


//conda activate mlagents
//mlagents-learn --run-id test6 --force

public class AIController : Agent
{
    public float speedMove;
    public float speedRotate;
    //public GameObject cps;

    //private Rigidbody2D rb;

    [SerializeField] private TrackCheckpoints trackCheckpoints;

    public override void Initialize()
    {
        //rb = GetComponent<Rigidbody2D>();
        trackCheckpoints.OnCorrectCheckpoint += TrackCheckpoints_OnCorrectCheckpoint;
        trackCheckpoints.OnIncorrectCheckpoint += TrackCheckpoints_OnIncorrectCheckpoint;
    }

    private void TrackCheckpoints_OnCorrectCheckpoint(object sender, TrackCheckpoints.CheckpointEventArgs e)
    {
        if (e.droneTransform == transform)
        {
            AddReward(5f);
            Debug.Log("Add Reward 5f");
        }
    }

    private void TrackCheckpoints_OnIncorrectCheckpoint(object sender, TrackCheckpoints.CheckpointEventArgs e)
    {
        if (e.droneTransform == transform)
        {
            AddReward(-3f);
            //EndEpisode();
            Debug.Log("Add Punishment -3f");
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(100, 580, 0);
        transform.forward = Vector3.forward;
        trackCheckpoints.ResetCheckpoint(transform);
        //cps = Instantiate(cps);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.DiscreteActions[0]);
        float movex = actions.ContinuousActions[0];
        //float movey = actions.ContinuousActions[1];
        float rotate = actions.ContinuousActions[2];

        //transform.localPosition += new Vector3(movex, movey, 0).normalized * Time.deltaTime * speedMove;
        //rb.MovePosition(transform.position + transform.forward * movex * speedMove * Time.deltaTime);
        transform.Translate(Vector2.down * (movex + 1) / 2 * speedMove * Time.deltaTime);
        //transform.position += new Vector3((movex+1)/2, 0, 0).normalized * Time.deltaTime * speedMove;
        transform.Rotate(0f, 0f, rotate * speedRotate, Space.Self);
        //transform.Rotate(Vector3.forward, rotate * speedRotate * Time.deltaTime);

        //Use controller to move
    }

    //public override void CollectObservations(VectorSensor sensor)
    //{
    //    sensor.AddObservation(transform.position);   
    //    sensor.AddObservation(trackCheckpoints.GetNextCheckpoint(transform).transform.position);
    //}

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "wall")
        {
            Debug.Log("collided with Wall");
            AddReward(-10f);
            EndEpisode();
        }
        //if (collision.gameObject.tag == "cp")
        //{
        //    Debug.Log("collided with " + collision.name);
        //    AddReward(2f);
        //    Destroy(collision.gameObject);
        //    Destroy(cps.gameObject);
        //    //collision.enabled = false;
        //}
    }
}
