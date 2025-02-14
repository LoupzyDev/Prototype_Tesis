using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Material mColor;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                UpdateParticleSystem(hit.point, Color.yellow);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                UpdateParticleSystem(hit.point, Color.red);
            }
        }
    }

    private void UpdateParticleSystem(Vector3 position, Color color)
    {
        mColor.color = color;
        _particleSystem.transform.position = position;
        _particleSystem.Play();
    }
}
