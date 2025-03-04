using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {
    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private int numberOfParticleS;
    [SerializeField] private Camera mainCamera;

    private List<ParticleSystem> particlePool = new List<ParticleSystem>();
    private Queue<ParticleSystem> availableParticles = new Queue<ParticleSystem>();

    private void Start() {
        for (int i = 0; i < numberOfParticleS; i++) {
            ParticleSystem ps = Instantiate(particlePrefab);
            ps.transform.parent = gameObject.transform;
            ps.gameObject.SetActive(false);
            particlePool.Add(ps);
            availableParticles.Enqueue(ps);
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            if (availableParticles.Count > 0) {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                    Color selectedColor;

                    if (Input.GetMouseButtonDown(0)) {
                        selectedColor = Color.yellow;
                    } else {
                        selectedColor = Color.red;
                    }

                    ActivateParticleSystem(hit.point, selectedColor);
                }
            }
        }
    }

    private void ActivateParticleSystem(Vector3 position, Color color) {
        if (availableParticles.Count > 0) {
            ParticleSystem ps = availableParticles.Dequeue();
            ps.transform.position = position;

            Renderer renderer = ps.GetComponent<Renderer>();
            renderer.material.color = color;

            var mainModule = ps.main;
            mainModule.startColor = color;
            ps.gameObject.SetActive(true);
            ps.Play();
            StartCoroutine(DeactivateAfterTime(ps, mainModule.duration));
        }
    }

    private IEnumerator<WaitForSeconds> DeactivateAfterTime(ParticleSystem ps, float duration) {
        yield return new WaitForSeconds(duration);
        ps.Stop();
        ps.gameObject.SetActive(false);
        availableParticles.Enqueue(ps);
    }
}
