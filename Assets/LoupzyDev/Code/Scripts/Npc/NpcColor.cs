using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcColor : MonoBehaviour {

    [SerializeField] private Image emote;
    [SerializeField] private Color colorlessEmote;

    [SerializeField] private float transitionSpeed = 2f;

    [SerializeField] private Renderer npcRenderer;
    [SerializeField] private float saturation = 1.0f;

    private MaterialPropertyBlock propertyBlock;
    private bool toggle = false;
    [SerializeField] KeyCode numberNpc;

    private void Awake() {
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update() {
        if (Input.GetKeyDown(numberNpc)) {
            toggle = !toggle;
        }

        float targetSaturation;
       

        if (toggle) {
            emote.color = Color.Lerp(emote.color, colorlessEmote, transitionSpeed * Time.deltaTime);
            targetSaturation = 0f;
        } else {
            emote.color = Color.Lerp(emote.color, Color.white, transitionSpeed * Time.deltaTime);
            targetSaturation = 1f;
        }

        saturation = Mathf.Lerp(saturation, targetSaturation, transitionSpeed * Time.deltaTime);
        saturation = Mathf.Clamp(saturation, 0, 1);


        npcRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_Saturation", saturation);
        npcRenderer.SetPropertyBlock(propertyBlock);
    }
}
