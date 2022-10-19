using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kick : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    //var heading = target.position - player.position;
    public Rigidbody pelota;
    public float potencia;
    public float power = 1;
    private Vector3 vectorPlayerPelota = new Vector3();
    public Text PowerPlayer;
    public KeyCode kick = KeyCode.Space;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //pelota = GameObject.Find("Soccer Ball").GetComponent<Rigidbody>();
        power = 1;
        PowerPlayer.text = "";
    }
    void actulizarPotenciaActual()
    {
        if (power > 1)
        {
            if (power <= 1.1)
            {
                PowerPlayer.text = "-";
            }
            else if (power <= 1.15)
            {
                PowerPlayer.text = "--";
            }
            else if (power <= 1.2)
            {
                PowerPlayer.text = "----";
            }
            else if (power <= 1.25)
            {
                PowerPlayer.text = "------";
            }
            else if (power <= 1.3)
            {
                PowerPlayer.text = "--------";
            }
            else if (power <= 1.35)
            {
                PowerPlayer.text = "----------";
            }
            else if (power <= 1.4)
            {
                PowerPlayer.text = "------------";
            }
            else if (power <= 1.45)
            {
                PowerPlayer.text = "--------------";
            }
            else if (power <= 1.5)
            {
                PowerPlayer.text = "----------------";
            } 
            else if (power <= 1.55)
            {
                PowerPlayer.text = "------------------";
            }
            else if (power <= 1.6)
            {
                PowerPlayer.text = "--------------------";
            }
            else if (power <= 1.65)
            {
                PowerPlayer.text = "----------------------";
            }
            else if (power <= 1.7)
            {
                PowerPlayer.text = "------------------------";
            }
            else if (power <= 1.75)
            {
                PowerPlayer.text = "--------------------------";
            }
            else if (power <= 1.8)
            {
                PowerPlayer.text = "----------------------------";
            }
            else if (power <= 1.85)
            {
                PowerPlayer.text = "------------------------------";
            }
            else if (power <= 1.9)
            {
                PowerPlayer.text = "--------------------------------";
            }
            else if (power <= 1.95)
            {
                PowerPlayer.text = "----------------------------------";
            }
            else if (power <= 2)
            {
                PowerPlayer.text = "------------------------------------";
            }
        }
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
            Debug.Log((pelota.position - m_Rigidbody.position).magnitude);
            //pelota.AddForce()
        }
        if (Input.GetKeyUp(kick))
        {
            Debug.Log("KICKKKKKKKKK!!!!");
            if ((pelota.position - m_Rigidbody.position).magnitude <= 4) // Distancia requerida
            {
                Debug.Log("Dentro del rango de disparo");
                //float potenciafinal = m_Thrust * power;
                vectorPlayerPelota = pelota.position - (new Vector3(m_Rigidbody.position.x, m_Rigidbody.position.y - (power - 0.5f), m_Rigidbody.position.z));
                Debug.Log(vectorPlayerPelota.magnitude);
                vectorPlayerPelota = vectorPlayerPelota.normalized;
                //Debug.Log(vectorPlayerPelota.magnitude);
                vectorPlayerPelota = vectorPlayerPelota * (power * power);
                Debug.Log(vectorPlayerPelota.magnitude);
                pelota.AddForce(vectorPlayerPelota * potencia);
            }
            else
            {
                Debug.Log("Fuera del rango de disparo");
            }
            PowerPlayer.text = "";
            //print("Space key was released");
            power = 1f;
        }
    }
}
