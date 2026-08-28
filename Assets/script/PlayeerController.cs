using System.Collections.Generic;
using UnityEngine;

public class PlayeerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 10f;
    public Rigidbody2D rb;

    private float moveX;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal") * moveSpeed;
        if (moveX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = moveX;
        rb.velocity = velocity;
    }
}
