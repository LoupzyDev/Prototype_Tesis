using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] private Camera mainCamera;
    public LayerMask ground;
    [SerializeField] private GameObject door;

    public bool isCommandToMove;
    public bool isFinishTask = false;
    public Vector3 initialPosition;


    private void Update() {
        if (Input.GetMouseButtonDown(1) && !isFinishTask) {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)) {
                isCommandToMove = true;
                agent.SetDestination(hit.point);
            }
        }
        if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance) {
            isCommandToMove = false;
        }
    }

    public bool CheckDistance() {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    private void OnDrawGizmos() {
        if (agent.hasPath) {
            for (var i = 0; i < agent.path.corners.Length - 1; i++) {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.blue);
            }
        }
    }
    public void goToDoor() {
        agent.SetDestination(door.transform.position);
    }
    public void GoToSpecificObject(GameObject desk) {
        if (desk != null) {
            agent.SetDestination(desk.transform.position);
        }
    }
    public void GoToPosition(Vector3 position) {
        agent.SetDestination(position);
    }

}
