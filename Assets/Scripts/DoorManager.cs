using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Npc")) {
            collision.gameObject.SetActive(false);
        }
    }
}
