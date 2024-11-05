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

public class Npc : MonoBehaviour {

    private NpcState state;



    [SerializeField] private NpcTask npcTask;

    [SerializeField] private NpcsSO npcDataSO;
    private NPCData currentNpcData;

    public int npcIndex=0;
    public int hp;
    public int stamina;
    public int moral;
    public int happiness;
    public int speedTask ;
    public int quality ;

    public string role;

    public int workLoad ;
    public int minWorkLoad ;



    private void Awake() {
        InitializeNpc();
    }
    private void Start() {
        NpcSelectionManager._instance.allNpcList.Add(gameObject);
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
        state = newState;

        switch (state) {
            case NpcState.None:
                break;
            case NpcState.Playing:
                break;
            case NpcState.Working:
                break;
            case NpcState.Walking:
                break;
            case NpcState.Sleeping:

                break;
        }
    }




    public void StartTask(float duration, NpcState taskState) {
        StartCoroutine(TaskRoutine(duration, taskState));
    }

    private IEnumerator TaskRoutine(float duration, NpcState taskState) {
        ChangeGameState(taskState);

        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            npcTask.UpdateTimeBar(duration, elapsedTime);
            yield return null;
        }
        npcTask.offTimeBar();
        ChangeGameState(NpcState.Walking);
    }

}
