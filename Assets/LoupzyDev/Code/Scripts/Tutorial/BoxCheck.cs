using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxCheck : MonoBehaviour
{
    public static BoxCheck _instance;
    [SerializeField] private List<Vector3> boxPositions;
    [SerializeField] private TextMeshProUGUI textScoreMission;
    public GameObject panelMission;
    public bool isEnd;
    private int boxIndex;

    private void Awake() {
        _instance = this;
    }
    private void Start() {
        isEnd = false;
        boxIndex = 0;
        panelMission.SetActive(true);
        this.gameObject.transform.position = boxPositions[boxIndex];
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Npc") && boxIndex < 2 ) {
            boxIndex++;
            UpdateText();
            this.gameObject.transform.position = boxPositions[boxIndex];
        } 
        else if(other.gameObject.CompareTag("Npc") && boxIndex == 2) {
            boxIndex++;
            UpdateText();
            isEnd = true;
            this.gameObject.SetActive(false);

        }
    }

    void UpdateText() {

        if(boxIndex < 3) {
            textScoreMission.text = "Llegar a tu destino: " + boxIndex.ToString() + "/3";
        } else {
            textScoreMission.text = "Terminaste!";
        }
        
    }
}
    