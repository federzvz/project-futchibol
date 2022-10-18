using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    //var heading = target.position - player.position;
    public Rigidbody pelota;
    public float m_Thrust = 20f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //pelota = GameObject.Find("Soccer Ball").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            if((pelota.position - m_Rigidbody.position).magnitude <= 3) // Distancia requerida
            {
                Debug.Log("Dentro del rango de disparo");
                pelota.AddForce((pelota.position - m_Rigidbody.position) * m_Thrust);
            } else
            {
                Debug.Log("Fuera del rango de disparo");
            }
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            //m_Rigidbody.AddForce(transform.up * m_Thrust);
            //Debug.Log((pelota.position - m_Rigidbody.position).magnitude);
            //pelota.AddForce()
        }
    }
}
