using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour {
    public TextMeshProUGUI Reloj;
    public Light directionalLight;

    [SerializeField] private float segundos;
    [SerializeField] private int minutos;
    [SerializeField] private int horas;
    public int timeVelocity = 720; // Velocidad base del tiempo
    public int timeVelocityNightMultiplier = 10; // Multiplicador de velocidad por la noche
    public bool isNight = false;
    public bool canSkipNight = false; // Control para permitir al jugador acelerar la noche

    public int today;

    [SerializeField] private GameObject door;
    [SerializeField] private GameObject buttonSkipNight;

    void Start() {
        GameManager._instance.updateTask(today);
    }

    void Update() {

        float velocidadActual;
        if (isNight && canSkipNight) {
            velocidadActual = timeVelocity * timeVelocityNightMultiplier;
        } else {
            velocidadActual = timeVelocity;
        }

        segundos += Time.deltaTime * velocidadActual;

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
            GameManager._instance.playerMoney += 100;
            GameManager._instance.UpdateMoneyUI();
        }

        // Transición a la noche
        if (horas == 22 && !isNight) {
            CambiarANoche();
        }

        // Transición al día
        if (horas == 7 && isNight) {
            CambiarADia();
        }

        AjustarIntensidadDeLuz();
        ModificarTextoDelTiempo();
    }

    void CambiarANoche() {
        isNight = true;
        door.GetComponent<Collider>().enabled = true;
        buttonSkipNight.gameObject.SetActive(true);
        GameManager._instance.SetAllNpcsToSleeping();
    }

    void CambiarADia() {
        isNight = false;
        EnableSkipNight(false);
        door.GetComponent<Collider>().enabled = false;
        buttonSkipNight.gameObject.SetActive(false);
        GameManager._instance.SetAllNpcsToWakeUp();
    }

    void AjustarIntensidadDeLuz() {
        if (directionalLight != null) {
            directionalLight.intensity = (horas >= 22 || horas < 7) ? 0.2f : 1f;
        }
    }

    public void EnableSkipNight(bool enable) {
        canSkipNight = enable;
    }

    public void SkipNight() {
        if (isNight && canSkipNight) {
            horas = 7;
            minutos = 0;
            segundos = 0;
            CambiarADia();
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
