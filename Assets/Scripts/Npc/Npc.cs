using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NpcState {
    None,
    Playing,
    Working,
    Walking,
    Sleeping
}

public class Npc : MonoBehaviour {


    private NpcState state;

    private TaskData taskData;

    private NPCData currentNpcData;

    [SerializeField] private NpcsSO npcDataSO;

    [Header("Propiedades")]
    [Space(10)]

    public int npcIndex = 0;
    public int hp;
    public int stamina;
    public int moral;
    public int happiness;
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
    public Npc() { }
    private void Awake() {
        InitializeNpc();
    }
    private void Start() {
        NpcSelectionManager._instance.allNpcList.Add(gameObject);
        _imageState.color = new Color(0, 0, 0, 0);
        ChangeGameState(NpcState.Walking);
    }

    private void Update()
    {
        _npcCanvas.transform.rotation=_mainCamera.transform.rotation;
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
                break;
            case NpcState.Playing:
                break;
            case NpcState.Working:
                _imageState.sprite= _imageSprite[0];
                _imageState.color = new Color(0, 0, 0, 1);
                StartCoroutine(WorkingRoutine()); // Comienza el trabajo en la tarea
                break;
            case NpcState.Walking:
                _imageState.color = new Color(0, 0, 0, 1);
                _imageState.sprite = _imageSprite[1];
                break;
            case NpcState.Sleeping:
                break;
        }
    }
    private IEnumerator WorkingRoutine() {
        if (taskData != null) {
            taskData.IsAssigned = true; // Marcar como asignada
            yield return new WaitForSeconds(5); // Simulación de trabajo
            if (actuallyDesk != null) {
                Desk desk = actuallyDesk.GetComponent<Desk>();
                if (desk != null) {
                    desk.ReleaseDesk();
                }
                actuallyDesk = null;
            }
            taskData.IsComplete = true; // Marcar como completada
            ChangeGameState(NpcState.Walking);
            Debug.Log("Acabé el trabajo en: " + taskData.Name);
            GetComponent<NpcMovement>().enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Work")) {
            actuallyDesk = collision.gameObject;
            Desk desk = collision.gameObject.GetComponent<Desk>();

            if (desk != null && desk.TryReserveDesk()) {
                // Verificar si hay una tarea disponible para este NPC
                TaskData task = TaskManager._instance.GetAvailableTask(role);
                if (task != null) {
                    // Asignar la tarea al NPC
                    taskData = task; // Guardamos la tarea en el NPC
                    ChangeGameState(NpcState.Working);
                    GetComponent<NpcMovement>().enabled = false;
                    Debug.Log("NPC ha reservado el escritorio y ha comenzado a trabajar en: " + task.Name);
                } else {
                    Debug.Log("No hay tareas disponibles para el rol de este NPC.");
                    desk.ReleaseDesk();  // Libera el escritorio si no hay tarea
                }
            } else {
                Debug.Log("El escritorio ya está ocupado. El NPC no puede comenzar a trabajar.");
            }
        }
    }
}