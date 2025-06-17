using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    private void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), rb.velocity.y);
    }
}
