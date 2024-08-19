using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NpcState {
    None,
    Playing,
    Working
}

public class NpcController : MonoBehaviour {

    private NpcState state;

    public NavMeshAgent agent;

    [SerializeField] private GameObject[] workObjects;
    [SerializeField] private GameObject[] funObjects;
    private GameObject nearestWorkObj;
    private GameObject nearestFunObj;

    private void Start() {
        workObjects = GameObject.FindGameObjectsWithTag("Work");
        funObjects = GameObject.FindGameObjectsWithTag("Fun");
    }

    private void Update() {

        if (agent.hasPath) {
            MoveAndRotate();
            CheckIfDestinationReached();
        }

        if (Input.GetMouseButton(0)) {
            SetDestination();
        }
    }

    public void ChangeGameState(NpcState newState) {
        state = newState;

        switch (state) {
            case NpcState.None:
                break;
            case NpcState.Playing:
                FindNearestObject(funObjects, out nearestFunObj);
                if (nearestFunObj != null) {
                    agent.SetDestination(nearestFunObj.transform.position);
                    Debug.Log("Ready to Play");
                }
                break;
            case NpcState.Working:
                FindNearestObject(workObjects, out nearestWorkObj);
                if (nearestWorkObj != null) {
                    agent.SetDestination(nearestWorkObj.transform.position);
                    Debug.Log("Ready to Work");
                }
                break;
        }
    }

    public void ChangeStateFromButton(int newState) {
        ChangeGameState((NpcState)newState);
    }

    // Método para encontrar el objeto más cercano de un array de objetos
    private void FindNearestObject(GameObject[] objects, out GameObject nearestObject) {
        nearestObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var obj in objects) {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < nearestDistance) {
                nearestDistance = distance;
                nearestObject = obj;
            }
        }
    }

    // Método para mover y rotar el NPC
    private void MoveAndRotate() {
        var dir = (agent.steeringTarget - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);
        agent.Move(dir * agent.speed * Time.deltaTime);
    }

    // Método para verificar si el NPC ha llegado a su destino
    private void CheckIfDestinationReached() {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            agent.ResetPath();
        }
    }

    // Método para establecer un nuevo destino
    private void SetDestination() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            agent.SetDestination(hit.point);
        }
    }

    // Dibujar la ruta del agente en la escena para depuración
    private void OnDrawGizmos() {
        if (agent.hasPath) {
            for (var i = 0; i < agent.path.corners.Length - 1; i++) {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.blue);
            }
        }
    }
}
