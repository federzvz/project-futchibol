using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanderinController : MonoBehaviour
{
    public Rigidbody paloBanderinRigidBody;
    public List<Collider> ground;
    public Collider paloBanderinCollider;

    public void Awake()
    {
        if (ground.Count > 0)
        {
            for (int i = 0; i < ground.Count; i++)
            {
                Physics.IgnoreCollision(paloBanderinCollider, ground[i]);
            }
        }
    }

    void Update()
    {
        if (paloBanderinRigidBody) {
            paloBanderinRigidBody.AddForce((-2 * Physics.gravity), ForceMode.Acceleration);
        }
    }
}
