using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcDisplay : MonoBehaviour {

    [SerializeField] private List<Image> npcsImage;
    [SerializeField] private List<GameObject> npcList;

    private void Start() {

        for (int i = 0; i < npcsImage.Count; i++) {
            Renderer npcRenderer = npcList[i].GetComponent<Renderer>();
            Color npcColor = npcRenderer.material.color;
            npcsImage[i].color = npcColor;
        }
    }
}
