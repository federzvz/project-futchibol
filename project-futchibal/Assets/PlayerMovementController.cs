using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public float m_Thrust = 20f;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.drag = 3;
    }
    void FixedUpdate()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");
        //Debug.Log(xDirection);
        //Debug.Log(zDirection);
        m_Rigidbody.AddForce(transform.right * xDirection * m_Thrust);
        m_Rigidbody.AddForce(transform.forward * zDirection * m_Thrust);

        //if (Input.GetButton("Jump"))
        //{
        //    //Apply a force to this Rigidbody in direction of this GameObjects up axis
        //    m_Rigidbody.AddForce(transform.up * m_Thrust);
        //}

    }
}
