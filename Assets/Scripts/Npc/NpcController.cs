using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NpcState {
    None,
    Playing,
    Working,
    Walking,
    Sleeping
}

public class NpcController : MonoBehaviour {

    private NpcState state;

    public NavMeshAgent agent;

    [SerializeField] private GameObject[] workObjects;
    [SerializeField] private GameObject[] funObjects;
    private GameObject nearestWorkObj;
    private GameObject nearestFunObj;
    [SerializeField] private GameObject door;

    private DeskController currentDesk; // Referencia al escritorio actual

    [SerializeField] private NpcTask npcTask;

    private int stamina = 100;
    private float timer = 0f;
    public float interval = 1f;

    private void Start() {
        // Inicia en el estado que consideres adecuado
        ChangeGameState(NpcState.Walking); // Ejemplo para iniciar en Walking
    }

    private void Update() {


        workObjects = GameObject.FindGameObjectsWithTag("Work");
        funObjects = GameObject.FindGameObjectsWithTag("Fun");

        if (agent.hasPath) {
            MoveAndRotate();
            CheckIfDestinationReached();
        }

        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.Z)) {
            ChangeGameState(NpcState.None);
            SetDestination();
        }

        timer += Time.deltaTime;

        if (timer >= interval) {
            timer = 0f; 

            if (state == NpcState.Working) {
                stamina -= 1;
            } else if (state == NpcState.Playing) {
                stamina += 1;
            }
            stamina = Mathf.Clamp(stamina, 0, 100);
        }


    }

    public void ChangeGameState(NpcState newState) {
        // Liberar el escritorio actual si el NPC cambia de estado
        if ((state == NpcState.Working || state == NpcState.Playing) && currentDesk != null) {
            currentDesk.ReleaseDesk();
            currentDesk = null;
        }

        state = newState;

        switch (state) {
            case NpcState.None:
                break;
            case NpcState.Playing:
                FindNearestObject(funObjects, out nearestFunObj);
                if (nearestFunObj != null) {
                    agent.SetDestination(nearestFunObj.transform.position);
                    //Debug.Log("Ready to Play");
                } else {
                    Debug.Log("No available Fun object found");
                    ChangeGameState(NpcState.Walking);
                }
                break;
            case NpcState.Working:
                FindNearestObject(workObjects, out nearestWorkObj);
                if (nearestWorkObj != null) {
                    agent.SetDestination(nearestWorkObj.transform.position);
                    //Debug.Log("Ready to Work");
                } else {
                    Debug.Log("No available Work object found");
                    ChangeGameState(NpcState.Walking);
                }
                break;
            case NpcState.Walking:
                StartCoroutine(WalkRandomly());
                break;
            case NpcState.Sleeping:
                agent.SetDestination(door.transform.position);
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
            DeskController deskController = obj.GetComponent<DeskController>();
            if (deskController != null && !deskController.IsOccupied()) { // Verifica si no está ocupado
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance) {
                    nearestDistance = distance;
                    nearestObject = obj;
                    currentDesk = deskController; // Asigna el escritorio actual
                }
            }
        }

        // Reserva el escritorio si se encontró uno
        if (nearestObject != null) {
            currentDesk.TryReserveDesk();
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

    // Corutina para caminar aleatoriamente
    private IEnumerator WalkRandomly() {
        while (state == NpcState.Walking) {
            Vector3 randomDirection = Random.insideUnitSphere * 6; // Ajusta el rango si es necesario
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 10, NavMesh.AllAreas)) {
                agent.SetDestination(hit.position);
            }

            yield return new WaitForSeconds(Random.Range(2, 5)); // Tiempo aleatorio antes de elegir una nueva dirección
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
    public void StartTask(float duration, NpcState taskState) {
        StartCoroutine(TaskRoutine(duration, taskState));
    }

    private IEnumerator TaskRoutine(float duration, NpcState taskState) {
        ChangeGameState(taskState); // Cambia al estado de la tarea (Work o Play)

        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            // Actualiza la barra de tiempo
            npcTask.UpdateTimeBar(duration, elapsedTime);
            yield return null;
        }
        npcTask.offTimeBar();
        ChangeGameState(NpcState.Walking); // Cambia a Walking después de completar la tarea
    }

    public int getStamina() {
        return stamina;
    }
}
    