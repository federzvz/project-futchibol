//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BallKick : MonoBehaviour
//{
//    Rigidbody m_Rigidbody;
//    public float m_Thrust = 20f;
//    Vector3 vector;
//    void Start()
//    {
//        m_Rigidbody = GetComponent<Rigidbody>();
//    }

//    void FixedUpdate()
//    {
//        if ()
//        {

//        }
//    }
//    void OnCollisionEnter(Collision col)
//    {
//        Debug.Log("Colision");
//        if (Input.GetButton("Jump"))
//        {
//            Debug.Log("Colision y Jump");
//            if (col.gameObject.tag == "Player")
//            {
//                //col.transform.position.x
//                //var heading = col.position - m_Rigidbody.position;
//                Debug.Log("Colision y Jump with Player");
//                vector = new Vector3(col.transform.position.x, -2, col.transform.position.z);
//                //Troublesome code
//                //m_Rigidbody.AddForce(col.transform.position * m_Thrust);
//                m_Rigidbody.AddForce(vector * m_Thrust);
                
//            }
//        }
//    }
//}