using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimulatedGravity : MonoBehaviour
{
    [SerializeField]
    private float gravity = 9.81f;
    private Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ProcessGravity();
    }

    private void ProcessGravity()
    {
        rb2D.AddForce(Vector2.down * gravity);
    }
}
