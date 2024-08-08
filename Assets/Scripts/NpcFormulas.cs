using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFormulas : MonoBehaviour
{
    // Variables para el NPC
    public float felicidad = 50f;
    public float moral = 50f;
    public float salud = 100f;
    public float stamina = 100f;
    public float cargaTrabajo = 0f;
    public float estres = 0f;
    public float tiempoDescanso = 0f;
    public float tiempoDiversion = 0f;
    public float calidadBase = 50f;
    public float calidadTrabajo = 50f;
    public float factorReduccion = 0.1f;
    public float tasaBase = 1f;
    public float tasaAumento = 0.2f;
    public float factorPenalizacion = 0.05f;

    void Start() {
        
        RandomizarVariables();
    }

    public void RandomizarVariables() {
        felicidad = Random.Range(0f, 100f);
        moral = Random.Range(0f, 100f);
        salud = Random.Range(0f, 100f);
        stamina = Random.Range(0f, 100f);
        cargaTrabajo = Random.Range(0f, 50f);
        estres = Random.Range(0f, 50f);
        tiempoDescanso = Random.Range(0f, 10f);
        tiempoDiversion = Random.Range(0f, 10f);
        calidadBase = Random.Range(30f, 70f);
        calidadTrabajo = calidadBase;

        EjecutarFormulasNPC();
    }

    public void EjecutarFormulasNPC() {
        Debug.Log("Felicidad después de actividades de diversión: " + FormulaNPC_FelicidadActividadesDiversion());
        Debug.Log("Felicidad después de estrés: " + FormulaNPC_ReduccionFelicidadPorEstres());
        Debug.Log("Productividad después de descanso: " + FormulaNPC_AumentoProductividadConDescanso());
        Debug.Log("Velocidad de trabajo con moral: " + FormulaNPC_EfectoMoralEnVelocidadTrabajo());
        Debug.Log("Salud después de pausas: " + FormulaNPC_SaludRecuperacionEnPausas());
        Debug.Log("Satisfacción general con el trabajo: " + FormulaNPC_SatisfaccionGeneral());
        Debug.Log("Calidad del trabajo basada en moral: " + FormulaNPC_CalidadTrabajo());
        Debug.Log("Probabilidad de enfermarse: " + FormulaNPC_ProbabilidadEnfermarse() + "%");
        Debug.Log("Felicidad reducida por carga de trabajo: " + FormulaNPC_FelicidadPorCargaTrabajo());
        Debug.Log("Stamina reiniciada al inicio del día: " + FormulaNPC_ReinicializacionStamina());
    }



    float FormulaNPC_FelicidadActividadesDiversion() {
        return felicidad + (tiempoDiversion * tasaAumento);
    }

    float FormulaNPC_ReduccionFelicidadPorEstres() {
        return felicidad - (estres * factorReduccion);
    }

    float FormulaNPC_AumentoProductividadConDescanso() {
        return 1f + (tiempoDescanso * tasaAumento);
    }

    float FormulaNPC_EfectoMoralEnVelocidadTrabajo() {
        return 1f * (1 + (moral - 50) / 100);
    }

    float FormulaNPC_SaludRecuperacionEnPausas() {
        return salud + (tiempoDescanso * 0.1f);
    }

    float FormulaNPC_SatisfaccionGeneral() {
        return 50f - (cargaTrabajo * factorPenalizacion) + (tiempoDiversion * tasaAumento);
    }

    float FormulaNPC_CalidadTrabajo() {
        return calidadBase * (moral > 50 ? 1.2f : (moral < 50 ? 0.8f : 1f));
    }

    float FormulaNPC_ProbabilidadEnfermarse() {
        return 100f - salud;
    }

    float FormulaNPC_FelicidadPorCargaTrabajo() {
        return felicidad - (cargaTrabajo * factorReduccion);
    }

    float FormulaNPC_ReinicializacionStamina() {
        return 100f; 
    }
}
