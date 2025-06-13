using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NpcSelectionManager : MonoBehaviour {

    public static NpcSelectionManager _instance;

    public List<GameObject> allNpcList = new List<GameObject>();
    public List<GameObject> npcsSelect = new List<GameObject>();

    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private bool isTutorial;

    private void Awake() {
        _instance = this;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable)) {

                if (Input.GetKey(KeyCode.LeftShift)) {
                    MultiSelect(hit.collider.gameObject);
                } else {
                    SelectByClicking(hit.collider.gameObject);
                }


            } else {

                if (!Input.GetKey(KeyCode.LeftShift)) {
                    DeselectAll();
                }

            }
        }
        if (Input.GetMouseButtonDown(1) && npcsSelect.Count == 1) {
            AudioManager._instance.PlayAudio(AudioState.MoveNpc);
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)) {

                groundMarker.transform.position = new Vector3(hit.point.x, 0.04f, hit.point.z);

                groundMarker.SetActive(false);
                groundMarker.SetActive(true);

            }
        }
    }

    private void MultiSelect(GameObject npc) {

        if (npcsSelect.Contains(npc) == false) {

            npcsSelect.Add(npc);
            UpdateOutline(npc, true);
            EnableNpcMovement(npc, true);
        } else {

            EnableNpcMovement(npc, false);
            UpdateOutline(npc, false);
            npcsSelect.Remove(npc);
        }
    }

    public void DeselectAll() {

        foreach (var npc in npcsSelect) {
            EnableNpcMovement(npc, false);
            UpdateOutline(npc, false);
        }
        groundMarker.SetActive(false);
        npcsSelect.Clear();
    }

    private void SelectByClicking(GameObject npc) {

        DeselectAll();
        npcsSelect.Add(npc);
        UpdateOutline(npc, true);
        EnableNpcMovement(npc, true);
    }

    private void EnableNpcMovement(GameObject npc, bool shouldMove) {

        npc.GetComponent<NpcMovement>().enabled = shouldMove;
    }

    private void UpdateOutline(GameObject npc, bool isVisible) {

        SkinnedMeshRenderer skinnedMeshRenderer = npc.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        Material outlineMaterial = skinnedMeshRenderer.materials[2];

        if (!isTutorial) {
            if (isVisible) {
                outlineMaterial.SetFloat("_OutlineWidth", 50f);
            } else {
                outlineMaterial.SetFloat("_OutlineWidth", 0f);
            }
        }else {
            outlineMaterial.SetColor("_OutlineColor", Color.green);
            if (isVisible) {
                outlineMaterial.SetFloat("_OutlineWidth", 50f);
            } else {
                outlineMaterial.SetColor("_OutlineColor", Color.red);
            }
        }
    }
}



