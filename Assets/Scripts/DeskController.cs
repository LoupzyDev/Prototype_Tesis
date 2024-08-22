using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskController : MonoBehaviour {
    private bool isOccupied = false;

    // Método para que el NPC reserve el escritorio
    public bool TryReserveDesk() {
        if (isOccupied) {
            return false; // No se puede reservar porque ya está ocupado
        }
        isOccupied = true;
        return true;
    }

    // Método para liberar el escritorio
    public void ReleaseDesk() {
        isOccupied = false;
    }

    // Método para verificar si está ocupado
    public bool IsOccupied() {
        return isOccupied;
    }
}
