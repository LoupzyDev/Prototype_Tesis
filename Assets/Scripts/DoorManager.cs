using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] Clock clock;


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Npc") && clock.isNigth) {
            other.gameObject.SetActive(false);
        }
    }
}
