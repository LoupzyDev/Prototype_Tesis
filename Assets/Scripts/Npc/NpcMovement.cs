using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] private Camera mainCamera;
    public LayerMask ground;
    [SerializeField] private GameObject door;

    private void Start() {
        
    }
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)) {
                agent.SetDestination(hit.point);
            }
        }
    }
    private void OnDrawGizmos() {
        if (agent.hasPath) {
            for (var i = 0; i < agent.path.corners.Length - 1; i++) {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.blue);
            }
        }
    }
    void goToDoor() {
        agent.SetDestination(door.transform.position);
    }
}
