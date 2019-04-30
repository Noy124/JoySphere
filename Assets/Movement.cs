using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    float dirX;
    float dirY;
    Rigidbody2D rb;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameControl.instance.gameOver)
        {
            dirX = Input.GetAxis("Horizontal");
            dirY = Input.GetAxis("Vertical");
        }
        else
        {
            speed = 0;
        }
    }

    private void FixedUpdate()
    {
            rb.velocity = new Vector2(dirX * speed + GameControl.instance.scrollSpeed, dirY * speed);
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Obstacle>())
        {
            rb.velocity = Vector2.zero;

        }
    }
}
