using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcEnviroment : MonoBehaviour {
    public float happiness = 10f;
    public float sadness = 10f;
    private float checkInterval = 0.5f; 
    private int workCount = 0;
    private int funCount = 0;
    private float workMultiplier = 1f;
    private float funMultiplier = 1f;

    void Start() {
        StartCoroutine(CheckEnvironment());
    }

    IEnumerator CheckEnvironment() {
        while (true) {
            UpdateCounts();
            UpdateMultipliers();


            happiness -= workCount * workMultiplier;
            sadness += workCount * workMultiplier;
            happiness += funCount * funMultiplier;
            sadness -= funCount * funMultiplier;

            yield return new WaitForSeconds(checkInterval);
        }
    }

    void UpdateCounts() {
        workCount = GameObject.FindGameObjectsWithTag("Work").Length;
        funCount = GameObject.FindGameObjectsWithTag("Fun").Length;
    }

    void UpdateMultipliers() {
        workMultiplier = workCount > 0 ? workCount * checkInterval : checkInterval;
        funMultiplier = funCount > 0 ? funCount * checkInterval : checkInterval;
    }
}
