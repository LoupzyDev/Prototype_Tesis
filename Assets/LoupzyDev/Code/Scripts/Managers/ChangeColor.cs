using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;


public class ChangeColor : MonoBehaviour {

    [SerializeField] private List<Renderer> objectsRenderers;
    private List<MaterialPropertyBlock> propertyBlocks;
    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private float globalSaturation = 1.0f;
    private MaterialPropertyBlock propertyBlock;


    public Volume componentVolume;
    private Vignette vignette;

    private bool toggle = false;

    private void Awake() {
        propertyBlocks = new List<MaterialPropertyBlock>();

        foreach (var renderer in objectsRenderers) {
            propertyBlocks.Add(new MaterialPropertyBlock());
        }

        AddRenderersByTag("Work");
        AddRenderersByTag("Fun");
        AddRenderersByTag("World");
        AddRenderersByTag("Wall");
    }
    private void Start() {

        componentVolume = GetComponent<Volume>();
        if (componentVolume.profile.TryGet(out vignette)) {
            vignette.intensity.value = 0f;
        }
    }
    private void AddRenderersByTag(string tag) {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objects) {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null) {
                objectsRenderers.Add(renderer);
                propertyBlocks.Add(new MaterialPropertyBlock());
            }
        }
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            toggle = !toggle;
        }

        float targetSaturation;

        if (toggle) {
            targetSaturation = 0f;
            StartCoroutine(AdjustVignetteIntensity(0.3f));
        } else {
            targetSaturation = 1f;
            StartCoroutine(AdjustVignetteIntensity(0f));
        }

        globalSaturation = Mathf.Lerp(globalSaturation, targetSaturation, transitionSpeed * Time.deltaTime);
        globalSaturation = Mathf.Clamp(globalSaturation, 0, 1);

        for (int i = 0; i < objectsRenderers.Count; i++) {
            objectsRenderers[i].GetPropertyBlock(propertyBlocks[i]);
            propertyBlocks[i].SetFloat("_Saturation", globalSaturation);
            objectsRenderers[i].SetPropertyBlock(propertyBlocks[i]);
        }
    }
    private System.Collections.IEnumerator AdjustVignetteIntensity(float targetIntensity) {
        float currentIntensity = vignette.intensity.value;
        float startTime = Time.time;


        while (Mathf.Abs(vignette.intensity.value - targetIntensity) > 0.01f) {
            vignette.intensity.value = Mathf.Lerp(currentIntensity, targetIntensity, (Time.time - startTime) * transitionSpeed);
            yield return null;
        }
        vignette.intensity.value = targetIntensity;
    }
}

