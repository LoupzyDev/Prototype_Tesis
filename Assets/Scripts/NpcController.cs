using System.Collections;
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

    private bool isColliding = false;  // Flag to check if NPC is in collision
    private float collisionTimer = 0f; // Timer to manage reward intervals
    private float rewardInterval = 0.2f; // Time interval for reward in seconds

    private float stateTimer = 0f; // Timer to track time in the current state
    private float statePenaltyTime = 5f; // Time limit before penalizing

    private Renderer npcRenderer;
    private bool canMove = true; // Flag to control movement

    public override void Initialize() {
        rb = GetComponent<Rigidbody>();
        npcRenderer = GetComponent<Renderer>();
        ChangeGameState(NpcState.None);
    }

    public void ChangeGameState(NpcState newState) {
        state = newState;
        stateTimer = 0f; // Reset the state timer whenever the state changes
        canMove = true;  // Allow movement when state changes

        switch (state) {
            case NpcState.None:
                break;
            case NpcState.Playing:
                canMove=true;
                Debug.Log("Ready to Play");
                break;
            case NpcState.Working:
                Debug.Log("Ready to Work");
                canMove = true;
                break;
        }
    }

    public override void OnEpisodeBegin() {
        ChangeGameState(NpcState.None);
        collisionTimer = 0f;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        // Update the state timer
        stateTimer += Time.deltaTime;

        // Penalize if the state timer exceeds the limit without collision
        if (stateTimer >= statePenaltyTime && !isColliding) {
            if (state == NpcState.Working) {
                AddReward(-10f);
                Debug.Log("Penalized for not working");
            } else if (state == NpcState.Playing) {
                AddReward(-10f);
                Debug.Log("Penalized for not playing");
            }
            stateTimer = 0f; // Reset the timer after penalizing
        }

        // Move only if canMove is true
        if (canMove) {
            float moveRotate = actions.ContinuousActions[0];
            float moveForward = actions.ContinuousActions[1];

            rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
            transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);
        }
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
        isColliding = true;  // Set flag to true when collision starts
    }

    private void OnCollisionStay(Collision collision) {
        if (isColliding) {
            collisionTimer += Time.deltaTime;
            if (collisionTimer >= rewardInterval) {
                collisionTimer = 0f;  // Reset timer after each interval
                if (collision.gameObject.CompareTag("Work")) {
                    if (state == NpcState.Working) {
                        npcRenderer.material.color = Color.black;
                        AddReward(10f);
                        Debug.Log("Trabajando");
                        canMove = false; // Stop movement when working correctly
                    } else {
                        AddReward(-15f);
                        npcRenderer.material.color = Color.white;
                    }
                }
                if (collision.gameObject.CompareTag("Fun")) {
                    if (state == NpcState.Playing) {
                        npcRenderer.material.color = Color.green;
                        AddReward(10f);
                        Debug.Log("Jugando");
                        canMove = false; // Stop movement when playing correctly
                    } else {
                        AddReward(-15f);
                        npcRenderer.material.color = Color.white;
                    }
                }
                if (collision.gameObject.CompareTag("Wall")) {
                    npcRenderer.material.color = Color.red;
                    AddReward(-30f);
                    EndEpisode();
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        isColliding = false;  // Reset flag when collision ends
        collisionTimer = 0f;  // Reset timer when collision ends
        //canMove = true;       // Allow movement again when collision ends
    }
}
