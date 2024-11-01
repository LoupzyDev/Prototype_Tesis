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

    private DeskController currentDesk;
    [SerializeField] private NpcTask npcTask;

 
    [SerializeField] private NpcsSO npcDataSO;
    private NPCData currentNpcData;

    public int npcIndex=0;
    public int hp;
    public float stamina;
    public int moral;
    public int happiness;
    public int speedTask ;
    public int quality ;

    public string role;

    public int workLoad ;
    public int minWorkLoad ;


    private float timer = 0f;
    public float interval = 1f;

    private void Awake() {
        InitializeNpc();
    }
    private void Start() {

        ChangeGameState(NpcState.Walking);
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


    private void InitializeNpc() {

        currentNpcData = npcDataSO.npcList[npcIndex]; 

        gameObject.name = currentNpcData.npcName;

        hp =currentNpcData.HP;
        stamina = currentNpcData.Stamina;
        moral = currentNpcData.Moral;
        happiness = currentNpcData.Happiness;
        speedTask = currentNpcData.Speed;
        quality = currentNpcData.Quality;
        role = currentNpcData.Role.ToString();
        workLoad = currentNpcData.WorkLoad;
        minWorkLoad = currentNpcData.MinWorkLoad;


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
                } else {
                    Debug.Log("No available Fun object found");
                    ChangeGameState(NpcState.Walking);
                }
                break;
            case NpcState.Working:
                FindNearestObject(workObjects, out nearestWorkObj);
                if (nearestWorkObj != null) {
                    agent.SetDestination(nearestWorkObj.transform.position);
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

    private void FindNearestObject(GameObject[] objects, out GameObject nearestObject) {
        nearestObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var obj in objects) {
            DeskController deskController = obj.GetComponent<DeskController>();
            if (deskController != null && !deskController.IsOccupied()) {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance) {
                    nearestDistance = distance;
                    nearestObject = obj;
                    currentDesk = deskController;
                }
            }
        }

        if (nearestObject != null) {
            currentDesk.TryReserveDesk();
        }
    }

    private void MoveAndRotate() {
        var dir = (agent.steeringTarget - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);
        agent.Move(dir * agent.speed * Time.deltaTime);
    }

    private void CheckIfDestinationReached() {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            agent.ResetPath();
        }
    }

    private void SetDestination() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            agent.SetDestination(hit.point);
        }
    }

    private IEnumerator WalkRandomly() {
        while (state == NpcState.Walking) {
            Vector3 randomDirection = Random.insideUnitSphere * 6;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 10, NavMesh.AllAreas)) {
                agent.SetDestination(hit.position);
            }

            yield return new WaitForSeconds(Random.Range(2, 5));
        }
    }

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
        ChangeGameState(taskState);

        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            while (stamina <= 0) {
                stamina += Time.deltaTime;
            }

            elapsedTime += Time.deltaTime * speedTask;
            npcTask.UpdateTimeBar(duration, elapsedTime);
            yield return null;
        }
        npcTask.offTimeBar();
        ChangeGameState(NpcState.Walking);
    }

    public float getStamina() {
        return stamina;
    }
}
