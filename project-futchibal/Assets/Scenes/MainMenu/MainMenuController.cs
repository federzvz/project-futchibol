using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;

    public GameObject multijugadorLocal, multijugadorOnline, estadisticas, opciones, salir;
    public GameObject partidoRapido, volverMainMenu, campeonato;

    public GameObject partidoRapidoPanel;
    public GameObject btnElegirCancha1, btnElegirCancha2;

    private string stadiumSelected = "";

    public void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    public void OnMultijugadorLocalClick() {
        partidoRapido.SetActive(true);
        campeonato.SetActive(true);
    }

    public void OnPartidoRapidoClick() {
        partidoRapido.SetActive(false);
        campeonato.SetActive(false);
        multijugadorLocal.SetActive(false);
        estadisticas.SetActive(false);
        multijugadorOnline.SetActive(false);
        opciones.SetActive(false);
        salir.SetActive(false);

        partidoRapidoPanel.SetActive(true);
    }

    public void OnBtnCancha1Selected() {
        stadiumSelected = "FedeScene";
    }
    public void OnBtnCancha2Selected()
    {
        stadiumSelected = "MiltonScene";
    }

    public void OnBtnPlayMatch() {
        if (stadiumSelected.Equals("")) {
            int rInt = Random.Range(0, 1);
            if (rInt == 0)
            {
                stadiumSelected = "FedeScene";
            }
            else {
                stadiumSelected = "MiltonScene";
            }
        }
        SceneManager.LoadScene(stadiumSelected);
    }

    public void OnQuitClick() {
        Application.Quit();
    }


}
