using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskController : MonoBehaviour {
    private bool isOccupied = false;

    // M�todo para que el NPC reserve el escritorio
    public bool TryReserveDesk() {
        if (isOccupied) {
            return false; // No se puede reservar porque ya est� ocupado
        }
        isOccupied = true;
        return true;
    }

    // M�todo para liberar el escritorio
    public void ReleaseDesk() {
        isOccupied = false;
    }

    // M�todo para verificar si est� ocupado
    public bool IsOccupied() {
        return isOccupied;
    }
}
