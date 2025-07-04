using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum NpcState {
    None,
    Playing,
    Working,
    Walking,
    Sleeping
}

public class Npc : MonoBehaviour {


    public NpcState state;

    private TaskData taskData;

    private NPCData currentNpcData;

    [SerializeField] private NpcsSO npcDataSO;

    [Header("Propiedades")]
    [Space(10)]

    public int npcIndex = 0;
    public float hp;
    public float stamina;
    public float moral;
    public float happiness;
    public int speedTask;
    public int quality;
    public string role;

    public int workLoad;
    public int minWorkLoad;

    [Header("Objetos")]
    [Space(10)]

    GameObject actuallyDesk;
    NpcSelectionManager selectionManager;

    [Header("Ui")]
    [Space(10)]

    [SerializeField] private Image _imageState;
    [SerializeField] private Canvas _npcCanvas;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private List<Sprite> _imageSprite;

    public NpcMovement npcMovement;

    [Header("Unity")]
    [Space(10)]
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isTutorial;

    [Header("Effects")]
    [Space(10)]
    [SerializeField] private ParticleSystem npcParticleS;

    private void Awake() {
        InitializeNpc();
    }
    private void Start() {

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        NpcSelectionManager._instance.allNpcList.Add(gameObject);
        if (isTutorial) {
            ChangeGameState(NpcState.None);
        } else {
            ChangeGameState(NpcState.Walking);
        }

    }

    private void Update()
    {
        _npcCanvas.transform.rotation=_mainCamera.transform.rotation;

        if (state == NpcState.Working) {
            stamina -= Time.deltaTime * 6;
            stamina = Mathf.Clamp(stamina, 0, 100);
        }
        if(state == NpcState.Playing) {
            stamina += Time.deltaTime * 3;
            stamina = Mathf.Clamp(stamina, 0, 100);
        }

        if (npcMovement.CheckDistance() && state == NpcState.Walking) {
            ChangeGameState(NpcState.None);
        }

        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) {
            animator.SetBool("isMoving",true);
        } else {
            animator.SetBool("isMoving", false);
        }
    }

    private void InitializeNpc() {

        currentNpcData = npcDataSO.npcList[npcIndex];

        gameObject.name = currentNpcData.npcName;

        hp = currentNpcData.HP;
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
        state = newState;

        switch (state) {
            case NpcState.None:
                npcMovement.isFinishTask = false;
                GetComponent<NpcMovement>().enabled = false;
                break;
            case NpcState.Playing:
                _imageState.sprite = _imageSprite[2];
                break;
            case NpcState.Working:
                npcMovement.isFinishTask = false;
                npcMovement.agent.ResetPath();
                npcMovement.agent.isStopped = true;
                npcMovement.enabled = false;
                _imageState.sprite= _imageSprite[0];
                StartCoroutine(WorkingRoutine());
                npcParticleS.Play();
                break;
            case NpcState.Walking:
                npcMovement.isFinishTask = true;
                GetComponent<NpcMovement>().enabled = true;
                npcMovement.agent.isStopped = false;
                _imageState.sprite = _imageSprite[1];
                npcMovement.GoToPosition(npcMovement.initialPosition);
                break;
            case NpcState.Sleeping:
                npcMovement.isFinishTask = true;
                _imageState.sprite = _imageSprite[3];
                npcMovement.goToDoor();
                break;
        }
    }

    private IEnumerator WorkingRoutine() {
        if (taskData != null) {
            taskData.IsAssigned = true; // Marcar como asignada
            yield return new WaitForSeconds(5); // Simulaci�n de trabajo
            if (actuallyDesk != null) {
                Desk desk = actuallyDesk.GetComponent<Desk>();
                if (desk != null) {
                    desk.ReleaseDesk();
                }
                actuallyDesk = null;
            }
            taskData.IsComplete = true; // Marcar como completada
            TaskManager._instance.RemoveCompletedTasks();
            AudioManager._instance.PlayAudio(AudioState.TaskComplete);
            npcParticleS.Stop();
            ChangeGameState(NpcState.Walking);
            Debug.Log("Acab� el trabajo en: " + taskData.Name);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Work")) {
            if (stamina <= 0) {
                Debug.Log("NPC est� demasiado cansado para trabajar.");
                return; 
            }
            actuallyDesk = collision.gameObject;
            Desk desk = collision.gameObject.GetComponent<Desk>();

            if (desk != null && desk.TryReserveDesk()) {
                // Verificar si hay una tarea disponible para este NPC
                TaskData task = TaskManager._instance.GetAvailableTask(role);
                if (task != null) {
                    // Asignar la tarea al NPC
                    taskData = task; // Guardamos la tarea en el NPC
                    ChangeGameState(NpcState.Working);
                    NpcSelectionManager._instance.DeselectAll();
                    //GetComponent<NpcMovement>().enabled = false;
                    Debug.Log("NPC ha reservado el escritorio y ha comenzado a trabajar en: " + task.Name);
                } else {
                    Debug.Log("No hay tareas disponibles para el rol de este NPC.");
                    desk.ReleaseDesk();  // Libera el escritorio si no hay tarea
                }
            } else {
                Debug.Log("El escritorio ya est� ocupado. El NPC no puede comenzar a trabajar.");
            }
        }
        if (collision.gameObject.CompareTag("Fun")) {
            ChangeGameState(NpcState.Playing);

        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Fun")) {
            ChangeGameState(NpcState.Walking);
        }
    }

    void SetColor(float r, float g, float b) {
        _imageState.color = new Color(r / 255f, g / 255f, b / 255f, 1f);
    }

}