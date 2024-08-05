using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public enum NpcState {
    None,
    Playing,
    Working
}

public class NpcController : Agent {
    private NpcState state;
    private Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;

    public override void Initialize() {
        rb = GetComponent<Rigidbody>();
        ChangeGameState(NpcState.None); 
    }

    public void ChangeGameState(NpcState newState) {
        state = newState;
        switch (state) {
            case NpcState.None:
                break;
            case NpcState.Playing:
                Debug.Log("P");
                break;
            case NpcState.Working:
                Debug.Log("W");
                break;
        }
    }

    public override void OnEpisodeBegin() {
        ChangeGameState(NpcState.None);
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rb.velocity); 
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    public void ChangeStateFromButton(int newState) {
        ChangeGameState((NpcState)newState);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Work")) {
            if (state == NpcState.Working) {
                AddReward(10f);
                Debug.Log("Trabajando");
            } else {
                AddReward(-15f);
            }
        }
        if (collision.gameObject.CompareTag("Fun")) {
            if (state == NpcState.Playing) {
                AddReward(10f);
                Debug.Log("Jugando");
            } else {
                AddReward(-15f);
            }
        }
        if (collision.gameObject.CompareTag("Wall")) {
            AddReward(-15f);
            EndEpisode();
        }
    }
}
