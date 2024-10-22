using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour {
    public TextMeshProUGUI Reloj;
    public Light directionalLight;

    [SerializeField] private float segundos;
    [SerializeField] private int minutos;
    [SerializeField] private int horas;
    public int velocidaddeltiempo = 1;
    public bool isNigth=false;

    public int today = -1;

    void Update() {
        segundos += Time.deltaTime * velocidaddeltiempo;

        if (segundos >= 60) {
            minutos++;
            segundos = 0;
        }
        if (minutos >= 60) {
            horas++;
            minutos = 0;
        }
        if (horas >= 24) {
            horas = 0;
            today++;
            GameManager._instance.updateTask(today);
        }
        if(horas == 22 && !isNigth) {
            GameManager._instance.SetAllNpcsToSleeping();
            isNigth = true;
        }
        if (horas == 7 && isNigth) {
            isNigth = false;
            GameManager._instance.SetAllNpcsToWakeUp();
        }
        AjustarIntensidadDeLuz();
        ModificarTextoDelTiempo();
    }
    void AjustarIntensidadDeLuz() {
        if (directionalLight != null) {
            if (horas >= 22 || horas < 7) {
                directionalLight.intensity = 0.2f;
            } else {
                directionalLight.intensity = 1f;
            }
        }
    }


    void ModificarTextoDelTiempo() {
        int horas12 = horas % 12; 
        if (horas12 == 0) horas12 = 12; 

        string periodo = horas < 12 ? "AM" : "PM"; 

        Reloj.text = AgregarUnCeroAdelanteSiEsNecesario(horas12) + ":"
                   + AgregarUnCeroAdelanteSiEsNecesario(minutos) + " " + periodo;
    }

    string AgregarUnCeroAdelanteSiEsNecesario(int n) {
        return n < 10 ? "0" + n : n.ToString();
    }
}
