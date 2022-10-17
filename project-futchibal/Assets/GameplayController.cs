using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public Rigidbody pelotaRigidbody;
    public List<Collider> paredes;
    public GameObject pelota;
    private int scoreTeam1, scoreTeam2;
    private bool isGol = false;
    public Text textoScoreJugador1, textoScoreJugador2, timerScoreBoard;
    private bool isJuegoDetenido = false;
    public float coordenadaGolArcoJugador1, coordenadaGolArcoJugador2;
    private List<Vector3> team1StartPositions = new List<Vector3>();
    private List<Vector3> team2StartPositions = new List<Vector3>();
    public Vector3 pelotaPosicionInicial;
    public List<GameObject> team1;
    public List<GameObject> team2;
    private float timer = 0;
    private float minutes, seconds;

    private void Start()
    {
        SaveInitialPlayersPositions();
        DesactivarColisionesJugadorParedes();
        scoreTeam1 = 0;
        scoreTeam2 = 0;
    }

    public void Update()
    {
        TimerManager();
        CheckearGol();
        if (isJuegoDetenido) {
            ComenzarJuego();
        }
    }

    public void DesactivarColisionesJugadorParedes() {
        for (int i = 0; i < paredes.Count; i++)
        {
            if (paredes[i])
            {
                for (int j = 0; j < team1.Count; j++) {
                    Physics.IgnoreCollision(team1[j].GetComponent<Collider>(), paredes[i]);
                }
                for (int j = 0; j < team2.Count; j++)
                {
                    Physics.IgnoreCollision(team2[j].GetComponent<Collider>(), paredes[i]);
                }

            }
        }
    }

    public void CheckearGol() {
        if (isGol == false)
        {
            if (pelota.transform.localPosition.x >= coordenadaGolArcoJugador1)
            {
                scoreTeam1++;
                textoScoreJugador1.text = scoreTeam1.ToString();
                isGol = true;
                isJuegoDetenido = true;
            }
            if (pelota.transform.localPosition.x <= coordenadaGolArcoJugador2)
            {
                scoreTeam2++;
                textoScoreJugador2.text = scoreTeam2.ToString();
                isGol = true;
                isJuegoDetenido = true;
            }
        }
        else {
            ResetarJuego();
        }
    }

    public void ResetarJuego() {
        if (team1.Count != 0) { 
            for (int j = 0; j < team1.Count; j++)
            {
                team1[j].transform.position = team1StartPositions[j];
            }
        }
        if (team2.Count != 0)
        {
            for (int j = 0; j < team2.Count; j++)
            {
                team2[j].transform.position = team2StartPositions[j];
            }
        }
        pelota.transform.position = pelotaPosicionInicial;
        pelotaRigidbody.isKinematic = true;
        isGol = false;
    }

    public void ComenzarJuego() {
        pelotaRigidbody.isKinematic = false;
    }

    public void SaveInitialPlayersPositions() {
        if (team1.Count != 0) {
            for (int j = 0; j < team1.Count; j++)
            {
                team1StartPositions.Add(team1[j].transform.position);
            }
        }
        if (team2.Count != 0)
        {
            for (int j = 0; j < team2.Count; j++)
            {
                team2StartPositions.Add(team2[j].transform.position);
            }
        }

    }

    public void TimerManager() {
        timer += Time.deltaTime;
        minutes = Mathf.FloorToInt(timer / 60);
        seconds = Mathf.FloorToInt(timer % 60);
        timerScoreBoard.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
