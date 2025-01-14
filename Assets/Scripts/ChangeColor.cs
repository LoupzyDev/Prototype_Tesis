using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChangeColor : MonoBehaviour {
    public Volume componentVolume;
    private ColorAdjustments colorAdjustments;

    public float transitionSpeed = 1f; 
    private float targetSaturation = 0f;
    private float currentSaturation;

    private bool toggle = true;

    private void Start() {

        componentVolume = GetComponent<Volume>();
        if (componentVolume.profile.TryGet(out colorAdjustments)) {
            // Asignar la saturación inicial
            currentSaturation = colorAdjustments.saturation.value;
        }
    }

    private void Update() {

        currentSaturation = Mathf.Lerp(currentSaturation, targetSaturation, transitionSpeed * Time.deltaTime);
        colorAdjustments.saturation.value = currentSaturation;

        
        if (Input.GetKeyDown(KeyCode.V)) {
            if (toggle) {
                targetSaturation = -100f; 
            } else {
                targetSaturation = 0f; 
            }
            toggle = !toggle;
        }
    }
}
