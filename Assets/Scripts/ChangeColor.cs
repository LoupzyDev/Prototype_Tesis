using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChangeColor : MonoBehaviour {

    public Volume componentVolume;
    private ColorAdjustments colorAdjustments;
    private Vignette vignette;

    public float transitionSpeed = 1f;
    private float targetSaturation = 0f;
    private float currentSaturation;

    private bool toggle = true;

    [Range(0, 100)]
    [SerializeField] float volumeSaturation;

    [Range(-100, 0)]
    [SerializeField] float colorlessovolumeSaturation;

    [Range(0, 1)]
    [SerializeField] float vignetteIntensity;

    private void Start() {
        componentVolume = GetComponent<Volume>();
        if (componentVolume.profile.TryGet(out colorAdjustments)) {
            currentSaturation = colorAdjustments.saturation.value;
            targetSaturation = currentSaturation;
        }
        if (componentVolume.profile.TryGet(out vignette)) {
            vignette.intensity.value = 0f;
        }
    }

    private void Update() {
        
        currentSaturation = Mathf.Lerp(currentSaturation, targetSaturation, transitionSpeed * Time.deltaTime);
        colorAdjustments.saturation.value = currentSaturation;

        if (Input.GetKeyDown(KeyCode.V)) {
            if (toggle) {
                targetSaturation = colorlessovolumeSaturation;
                StartCoroutine(AdjustVignetteIntensity(vignetteIntensity));
            } else {
                targetSaturation = volumeSaturation;
                StartCoroutine(AdjustVignetteIntensity(0f));
            }
            toggle = !toggle;
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
