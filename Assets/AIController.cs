using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AIController : Agent
{
    public float speedMove;
    public float speedRotate;
    public GameObject cps;

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(100, 580, 0);
        cps = Instantiate(cps);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.DiscreteActions[0]);
        float movex = actions.ContinuousActions[0];
        float movey = actions.ContinuousActions[1];
        float rotate = actions.ContinuousActions[2];

        transform.localPosition += new Vector3(movex, movey, 0).normalized * Time.deltaTime * speedMove;
        transform.Rotate(0f, 0f, rotate * speedRotate, Space.Self);

        //Use controller to move
    }

    //public override void CollectObservations(VectorSensor sensor)
    //{
    //    sensor.AddObservation(this);   

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
            Debug.Log("collided with " + collision.name);
            AddReward(-1f);
            EndEpisode();
        }
        if (collision.gameObject.tag == "cp")
        {
            Debug.Log("collided with " + collision.name);
            AddReward(2f);
            Destroy(collision.gameObject);
            Destroy(cps.gameObject);
            //collision.enabled = false;
        }
    }
}
