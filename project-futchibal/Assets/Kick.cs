using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Kick : MonoBehaviour
{
    // public AudioSource kickSound;
    public AudioSource sonidoKick;
    public AudioClip[] clips;
    Rigidbody m_Rigidbody;
    //var heading = target.position - player.position;
    public Rigidbody pelota;
    public float potencia;
    public float power = 1;
    private int powerAux = 1, vueltasFor = 1;
    private Vector3 vectorPlayerPelota = new Vector3();
    public Text PowerPlayer;
    private string powerTextAux = "";
    public KeyCode kick;
    public int playerTeamId;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //pelota = GameObject.Find("Soccer Ball").GetComponent<Rigidbody>();
        power = 1;
        PowerPlayer.text = "";
        setUpPlayerControlPrefs();
    }
    void actulizarPotenciaActual()
    {
        powerAux = (int)((power - 1) * 100);
        // Debug.Log(powerAux);
        vueltasFor = (powerAux * 38) / 100;
        for (int i = 0; i < vueltasFor; i++)
        {
            powerTextAux += "-";
        }
        PowerPlayer.text = powerTextAux;
        powerTextAux = "";
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Potencia de disparo: " + power);
        //Cambiar para poder manejar las 2 potencias (Team 1 y Team 2 [En principio]) Y decidir si este sript debe estar en la Escena o en cada Player
        //if (Input.GetButton("kick"))
        if (Input.GetKey(kick))
        {
            if(power < 2)
                power += 0.02f;
            actulizarPotenciaActual();
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            //m_Rigidbody.AddForce(transform.up * m_Thrust);
            // Debug.Log((pelota.position - m_Rigidbody.position).magnitude);
            //pelota.AddForce()
        }
        if (Input.GetKeyUp(kick))
        {
            // Debug.Log("KICKKKKKKKKK!!!!");
            if ((pelota.position - m_Rigidbody.position).magnitude <= 4) // Distancia requerida
            {
                // Debug.Log("Dentro del rango de disparo");
                //float potenciafinal = m_Thrust * power;
                if (pelota.position.y < 1.5) // Distancia maxima del suelo en que la pelota se patea "Normal" (Que se eleva dependiendo de la potencia)
                {
                    vectorPlayerPelota = pelota.position - (new Vector3(m_Rigidbody.position.x, m_Rigidbody.position.y - (power - 0.5f), m_Rigidbody.position.z));
                } else
                { // Totalmente recto
                    vectorPlayerPelota = pelota.position - (new Vector3(m_Rigidbody.position.x, pelota.position.y, m_Rigidbody.position.z));
                }
                

                // Debug.Log(vectorPlayerPelota.magnitude);
                vectorPlayerPelota = vectorPlayerPelota.normalized;
                //Debug.Log(vectorPlayerPelota.magnitude);
                vectorPlayerPelota = vectorPlayerPelota * (power * power);
                // Debug.Log(vectorPlayerPelota.magnitude);
                pelota.AddForce(vectorPlayerPelota * potencia);
                // Random.rango();
                PlaySound();
                // sonidosKick[0].Play();

            }
            else
            {
                // Debug.Log("Fuera del rango de disparo");
            }
            PowerPlayer.text = "";
            //print("Space key was released");
            power = 1f;
        }
    }

    public void setUpPlayerControlPrefs()
    {
        string json = File.ReadAllText(Application.dataPath + "/playerprefs.json");
        CustomPlayerPrefs customPlayerPrefs = JsonUtility.FromJson<CustomPlayerPrefs>(json);
        if (playerTeamId == 1)
        {
            this.kick = customPlayerPrefs.player1Kick;
        }
        if (playerTeamId == 2)
        {
            this.kick = customPlayerPrefs.player2Kick;
        }
    }
    public void PlaySound()
     {
        int rand = Random.Range(0, clips.Length);
        // audioSource.clip = sound[rand];
        // audioSource[rand].volume
        sonidoKick.clip = clips[rand];
        sonidoKick.volume = (power - 1);
        Debug.Log(sonidoKick.volume);
        sonidoKick.Play();
     }
}
