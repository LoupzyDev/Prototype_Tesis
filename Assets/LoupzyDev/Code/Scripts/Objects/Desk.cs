using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour {
    private bool isOccupied = false;

    public bool TryReserveDesk() {
        if (isOccupied) {
            return false;
        }
        isOccupied = true;
        return true;
    }

    public void ReleaseDesk() {
        isOccupied = false;
    }

    public bool IsOccupied() {
        return isOccupied;
    }
}
