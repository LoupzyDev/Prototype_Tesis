using System;
using UnityEngine;

public enum State {
    ShowDialogue,
    ClickNpc,
    Move,
    MoveToDesk,
    TaskComplete
}

public class StateManager : MonoBehaviour {
    public static StateManager _instance;
    private State currentState;

    private void Awake() {
        _instance = this;
    }
    void Start() {
        ChangeState(State.ShowDialogue);
    }

    void Update() {
        HandleState();
        if (Input.GetKeyDown(KeyCode.Return) && currentState == State.ShowDialogue) {
            ChangeState(State.ClickNpc);
        } else if (Input.GetKeyDown(KeyCode.Return) && currentState == State.ClickNpc) {

        }
    }

    void ChangeState(State newState) {
        Debug.Log($"Changing state to: {newState}");
        currentState = newState;
    }

    void HandleState() {
        switch (currentState) {
            case State.ShowDialogue:
                DialogueManager._instance.TurnOffOn(true,false);
                DialogueManager._instance.UpdateNarrativeDialogue(0);
                break;

            case State.ClickNpc:
                DialogueManager._instance.TurnOffOn(false, true);
                DialogueManager._instance.UpdateNpcDialogue(0);
                break;

            case State.Move:
                break;

            case State.MoveToDesk:
                break;

            case State.TaskComplete:
                break;
        }
    }

}
