using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckDay : MonoBehaviour
{
    private int today;
    public Clock clock;
    [SerializeField] GameObject imageQR;


    private void Update() {

        today = clock.today;

        if (today == 3) 
        {
            Time.timeScale = 0f;
            imageQR.SetActive(true);

        } else {

            imageQR.SetActive(false);
        }
    }
    public void RestartGame() {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0);
    }
}
