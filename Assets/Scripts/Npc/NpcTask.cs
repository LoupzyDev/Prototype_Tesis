using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NpcTask : MonoBehaviour
{   
    [SerializeField] private Image _timeBarSprite;
    [SerializeField] private Canvas _timeBarCanvas;

    private void Start() {
        _timeBarCanvas.gameObject.SetActive(false);
    }
    public void UpdateTimeBar(float maxTime, float currentTime) {
        _timeBarCanvas.gameObject.SetActive(true);
        _timeBarSprite.fillAmount = 1 - (currentTime / maxTime);
    }

    public void offTimeBar() {
        _timeBarCanvas.gameObject.SetActive(false);
    }
}
