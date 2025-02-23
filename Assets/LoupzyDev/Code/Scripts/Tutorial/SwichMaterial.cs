using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterial : MonoBehaviour {
    public static SwitchMaterial _instance;
    [SerializeField] private Material outlineMaterial;
    public bool isSwitched;
    [SerializeField] private List<Material> materialsList = new List<Material>();

    private Renderer renderFurniture;
    private Material[] originalMaterials;

    private void Awake() {
        _instance = this;
    }
    private void Start() {
        isSwitched = false;
        renderFurniture = GetComponent<Renderer>();
        originalMaterials = renderFurniture.materials;
    }

    private void Update() {
        if (isSwitched) {
            ApplyOutlineMaterial();
        } else {
            ResetToOriginalMaterials();
        }
    }

    private void ApplyOutlineMaterial() {
        Material[] modifiedMaterials = new Material[originalMaterials.Length + 1];
        originalMaterials.CopyTo(modifiedMaterials, 0);
        modifiedMaterials[modifiedMaterials.Length - 1] = outlineMaterial;
        renderFurniture.materials = modifiedMaterials;
    }

    private void ResetToOriginalMaterials() {
        renderFurniture.materials = originalMaterials;
    }
}
