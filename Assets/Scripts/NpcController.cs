using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public enum NpcState {
    Quiet,
    Working,
    Resting
}

public class NpcController : Agent {
    NpcState state;

    [SerializeField] float stamina = 10f;
    [SerializeField] float health = 10f;

    [SerializeField] bool isWorking = false;
    [SerializeField] bool isTired = false;
    [SerializeField] bool isRested = false;

    private float staminaDecreaseRate = 1f;
    private float staminaIncreaseRate = 1f;
    private float healthDecreaseRate = 0.5f;

    public void ChangeGameState(NpcState newState) {
        state = newState;
        switch (state) {
            case NpcState.Quiet:
                isWorking = false;
                isRested = false;
                isTired = false;
                break;
            case NpcState.Working:
                isWorking = true;
                isRested = false;
                isTired = false;
                break;
            case NpcState.Resting:
                isWorking = false;
                isRested = true;
                isTired = false;
                break;
        }
    }

    private void Update() {
        Debug.Log(state.ToString());
    }

    public override void OnEpisodeBegin() {
        state = NpcState.Working;
        stamina = 10f;
        health = 10f;
    }

    public override void OnActionReceived(ActionBuffers actions) {
        switch (actions.DiscreteActions[0]) {
            case 0:
                ChangeGameState(NpcState.Quiet);
                break;
            case 1:
                ChangeGameState(NpcState.Working);
                break;
            case 2:
                ChangeGameState(NpcState.Resting);
                break;
        }

        switch (state) {
            case NpcState.Working:
                Work();
                break;
            case NpcState.Resting:
                Rest();
                break;
            case NpcState.Quiet:
                break;
        }

        AddReward(CalculateReward());
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(stamina);
        sensor.AddObservation(health);
        sensor.AddObservation((int)state);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = (int)state;
    }

    private void Work() {
        if (isWorking) {
            stamina -= staminaDecreaseRate * Time.deltaTime;
            if (stamina < 2f) {
                health -= healthDecreaseRate * Time.deltaTime;
            }
            if (stamina <= 0) {
                stamina = 0;
                ChangeGameState(NpcState.Resting);
            }
        }
    }

    private void Rest() {
        if (isRested) {
            stamina += staminaIncreaseRate * Time.deltaTime;
            if (stamina >= 10f) {
                stamina = 10f;
                ChangeGameState(NpcState.Quiet);
            }
        }
    }

    private float CalculateReward() {
        float reward = 0f;

        if (health <= 0) {
            reward -= 20f;  
            EndEpisode();   
        } else {
            if (state == NpcState.Working) {
                if (stamina > 9f) {
                    reward += 10f;  
                } else if (stamina > 7f) {
                    reward += 0.4f;  
                } else if (stamina > 2f) {
                    reward += 0.1f;  
                }
            }

            if (state == NpcState.Resting && stamina < 10f) {
                reward += 0.05f;
            }

            if (state == NpcState.Working && stamina <= 2f) {
                reward -= 0.1f;
            }
        }

        return reward;
    }

    public void ChangeStateFromButton(int newState) {
        ChangeGameState((NpcState)newState);
    }
}
