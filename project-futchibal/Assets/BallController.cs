using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject ball, ballShadow;
    public float shadowYAltitude;
    public AudioSource casiGol;
    public AudioSource palo;
    private Rigidbody m_rigidbody;

    private bool checkGoal = false;
    private float xPelotaOnCollision;
    private bool arco;
    private float checkGoalCountdown = 0;
    void Start(){
        m_rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        
        if(checkGoal)
            checkCasiGol();
        ShadowAlwaysUnderBall();
    }
    public void checkCasiGol(){
        if (checkGoalCountdown >= 3) {
            Debug.Log("Se aleja?");
            if(arco == true){ // es el arco local en el que choco
                if(m_rigidbody.position.x > xPelotaOnCollision){ // la pelota se aleja
                    casiGol.Play();
                }
            } else { // es el arco visitante en el que choco
                if(m_rigidbody.position.x < xPelotaOnCollision){ // la pelota se aleja
                    casiGol.Play();
                }
            }
            checkGoal = false;
            checkGoalCountdown = 0;
        } else {
            checkGoalCountdown += 0.1f;
        }
    }

    public void ShadowAlwaysUnderBall() {
        ballShadow.transform.position = new Vector3(ball.transform.position.x, shadowYAltitude, ball.transform.position.z);
    }
    void OnCollisionEnter(Collision collision)
    {
        //GameObject objeto1 = GameObject.Find("objeto1");
        if (collision.collider.tag == "Palo")
        {

            Debug.Log("Pego en el palo");
            // float velocidad = m_rigidbody.velocity
            Debug.Log(m_rigidbody.velocity.magnitude);
            float fuerzaChoque = m_rigidbody.velocity.magnitude;
            if(fuerzaChoque >= 30){
                palo.volume = 1;
            } else {
                palo.volume = fuerzaChoque / 30;
            }
            // palo.volume = (power - 1);
            palo.Play();


            if(collision.collider.transform.position.x > 0){
                Debug.Log("Arco Visitante");
                xPelotaOnCollision = m_rigidbody.position.x;
                arco = false;
                checkGoal = true;
            } else {
                Debug.Log("Arco Local");
                xPelotaOnCollision = m_rigidbody.position.x;
                arco = true;
                checkGoal = true;
            }
            // esperar un segundo y si no va en direccion a gol casiGol.Play();


        }
    }
}
