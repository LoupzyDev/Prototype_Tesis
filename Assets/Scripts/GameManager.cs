using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager _instance;

    private List<NpcController> npcControllers;

    private void Awake() {
        _instance = this;
        InitializeNpcControllers();
    }

    // Busca todos los NPCs al inicio del juego y los almacena en la lista
    private void InitializeNpcControllers() {
        npcControllers = new List<NpcController>();
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("Npc");
        foreach (GameObject npc in npcs) {
            NpcController npcController = npc.GetComponent<NpcController>();
            if (npcController != null) {
                npcControllers.Add(npcController);
            }
        }
    }

    // Cambia el estado de todos los NPCs a un estado específico
    public void NpcsState(int state) {
        foreach (NpcController npcController in npcControllers) {
            npcController.ChangeStateFromButton(state);
        }
    }

    // Cambia el estado de un NPC específico por su índice en la lista
    public void ChangeNpcStateByIndex(int index, int state) {
        if (index >= 0 && index < npcControllers.Count) {
            npcControllers[index].ChangeStateFromButton(state);
        } else {
            Debug.LogWarning("Índice de NPC fuera de rango.");
        }
    }

    // Cambia el estado de un NPC específico por referencia al objeto NPC
    public void ChangeNpcStateByReference(NpcController npc, int state) {
        if (npcControllers.Contains(npc)) {
            npc.ChangeStateFromButton(state);
        } else {
            Debug.LogWarning("El NPC no está en la lista.");
        }
    }
}
