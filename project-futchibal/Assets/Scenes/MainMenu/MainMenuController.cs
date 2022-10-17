using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;

    public GameObject multijugadorLocal, multijugadorOnline, estadisticas, opciones, salir;
    public GameObject partidoRapido, volverMainMenu, volverMultijugadorLocal, partidoClasico, campeonato, partidoFantasioso;

    public void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    public void OnMultijugadorLocalClick() {
        multijugadorLocal.SetActive(false);
        multijugadorOnline.SetActive(false);
        estadisticas.SetActive(false);
        opciones.SetActive(false);
        salir.SetActive(false);

        partidoRapido.SetActive(true);
        volverMainMenu.SetActive(true);
        campeonato.SetActive(true);
    }

    public void OnPartidoRapidoClick() {
        partidoRapido.SetActive(false);
        volverMainMenu.SetActive(false);
        campeonato.SetActive(false);

        partidoClasico.SetActive(true);
        partidoFantasioso.SetActive(true);
        volverMultijugadorLocal.SetActive(true);
    }
    public void OnVolverMultijugadorLocalClick() {
        partidoClasico.SetActive(false);
        partidoFantasioso.SetActive(false);
        volverMultijugadorLocal.SetActive(false);

        partidoRapido.SetActive(true);
        volverMainMenu.SetActive(true);
        campeonato.SetActive(true);
    }

    public void OnVolverMainMenuClick() {
        partidoRapido.SetActive(false);
        volverMainMenu.SetActive(false);
        campeonato.SetActive(false);
        partidoClasico.SetActive(false);
        partidoFantasioso.SetActive(false);

        multijugadorLocal.SetActive(true);
        multijugadorOnline.SetActive(true);
        estadisticas.SetActive(true);
        opciones.SetActive(true);
        salir.SetActive(true);
    }
}
