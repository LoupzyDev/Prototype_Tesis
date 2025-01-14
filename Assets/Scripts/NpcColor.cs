using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcColor : MonoBehaviour {
    [SerializeField] private Image emote;
    [SerializeField] private Color colorlessEmote;
    [SerializeField] private Material NpcMaterial;

    private Color originalColor;
    [SerializeField] private Color colorlessColor;
    [SerializeField] private float transitionSpeed = 2f; // Velocidad de la transición

    private bool toggle = false;
    [SerializeField] KeyCode numberNpc;

    private void Start() {
        NpcMaterial = GetComponent<Renderer>().material;
        originalColor = NpcMaterial.color;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P) && Input.GetKeyDown(numberNpc)) {
            toggle = !toggle;
        }

        if (toggle) {
            emote.color = Color.Lerp(emote.color, colorlessEmote, transitionSpeed * Time.deltaTime);
            NpcMaterial.color = Color.Lerp(NpcMaterial.color, colorlessColor, transitionSpeed * Time.deltaTime);
        }
       
        else {
            emote.color = Color.Lerp(emote.color, Color.white, transitionSpeed * Time.deltaTime);
            NpcMaterial.color = Color.Lerp(NpcMaterial.color, originalColor, transitionSpeed * Time.deltaTime);
        }
    }
}
