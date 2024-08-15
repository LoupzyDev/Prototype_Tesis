using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private List<NpcController> npcControllers; 

    public void NpcsState(int state) {
        for (int i = 0; i < npcControllers.Count; i++) {
            npcControllers[i].ChangeStateFromButton(state);
        }
    }
}
