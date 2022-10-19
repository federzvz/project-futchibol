using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kick : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    //var heading = target.position - player.position;
    public Rigidbody pelota;
    public float m_Thrust = 20f;
    public float power = 1;
    public Vector3 vectorPlayerPelota = new Vector3();
    public Text PowerPlayer1;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //pelota = GameObject.Find("Soccer Ball").GetComponent<Rigidbody>();
        power = 1;
        PowerPlayer1.text = "";
    }
    void actulizarPotenciaActual()
    {
        if (power > 1)
        {
            if (power <= 1.1)
            {
                PowerPlayer1.text = "-";
            }
            else if (power <= 1.15)
            {
                PowerPlayer1.text = "--";
            }
            else if (power <= 1.2)
            {
                PowerPlayer1.text = "----";
            }
            else if (power <= 1.25)
            {
                PowerPlayer1.text = "------";
            }
            else if (power <= 1.3)
            {
                PowerPlayer1.text = "--------";
            }
            else if (power <= 1.35)
            {
                PowerPlayer1.text = "----------";
            }
            else if (power <= 1.4)
            {
                PowerPlayer1.text = "------------";
            }
            else if (power <= 1.45)
            {
                PowerPlayer1.text = "--------------";
            }
            else if (power <= 1.5)
            {
                PowerPlayer1.text = "----------------";
            } 
            else if (power <= 1.55)
            {
                PowerPlayer1.text = "------------------";
            }
            else if (power <= 1.6)
            {
                PowerPlayer1.text = "--------------------";
            }
            else if (power <= 1.65)
            {
                PowerPlayer1.text = "----------------------";
            }
            else if (power <= 1.7)
            {
                PowerPlayer1.text = "------------------------";
            }
            else if (power <= 1.75)
            {
                PowerPlayer1.text = "--------------------------";
            }
            else if (power <= 1.8)
            {
                PowerPlayer1.text = "----------------------------";
            }
            else if (power <= 1.85)
            {
                PowerPlayer1.text = "------------------------------";
            }
            else if (power <= 1.9)
            {
                PowerPlayer1.text = "--------------------------------";
            }
            else if (power <= 1.95)
            {
                PowerPlayer1.text = "----------------------------------";
            }
            else if (power <= 2)
            {
                PowerPlayer1.text = "------------------------------------";
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        Debug.Log("Potencia de disparo: " + power);
        if (Input.GetButton("kick"))
        {

            if(power < 2)
                power += 0.02f;
            actulizarPotenciaActual();
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            //m_Rigidbody.AddForce(transform.up * m_Thrust);
            Debug.Log((pelota.position - m_Rigidbody.position).magnitude);
            //pelota.AddForce()
        }
        if (Input.GetKeyUp(KeyCode.Space))
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
                pelota.AddForce(vectorPlayerPelota * m_Thrust);
            }
            else
            {
                Debug.Log("Fuera del rango de disparo");
            }

            PowerPlayer1.text = "";

            //print("Space key was released");
            power = 1f;
        }
    }
}
