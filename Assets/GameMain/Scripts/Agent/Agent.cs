using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private Vector3 movement;
    public float speed = 10;
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Movement2D();
    }

    void Movement2D()
    {
        movement = new Vector3(horizontal * Time.deltaTime * speed, vertical * Time.deltaTime * speed, 0);
        transform.Translate(movement);
    }
}
