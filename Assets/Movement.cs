using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class Movement : MonoBehaviour
{

    public float dirX;
    public float dirY;
    public Rigidbody2D rb;
    public Transform transform;
    public float speed = 2f;
    public Camera camera;
    public int speedScale = 1;

    private float minMaxPos;
    private SerialPort sp;
    private int velocity;
    private float degree;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        Debug.Log(camera.pixelWidth);
        minMaxPos = camera.scaledPixelWidth/2;


        //sp = new SerialPort("COM4", 38400);
        //if (!sp.IsOpen)
        //{
        //    sp.Open();
        //    sp.ReadTimeout = 1;
        //    sp.Handshake = Handshake.None;
        //}

    }

    // Update is called once per frame
    void Update() {

        //string data = null;
        //if (sp.IsOpen)
        //{
        //    try
        //    {
        //        data = sp.ReadLine();
        //    }
        //    catch (System.TimeoutException e)
        //    { }
        //    sp.BaseStream.Flush();


        //    if (data != null)
        //    {
        //        string[] msg = data.ToString().Split(' ');

        //        Debug.Log(msg[0] + " " + msg[1]);
        //        velocity = System.Convert.ToInt32(msg[0]);
        //        degree = System.Convert.ToSingle(msg[1]);
        //    }

        //}

        //speed = velocity * speedScale;
        //dirX = Mathf.Cos(degree) * velocity;
        //dirY = Mathf.Sin(degree) * velocity;
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
        Vector3 posCameraSpace = camera.WorldToViewportPoint(transform.position);
        if(posCameraSpace.x<=0 && rb.velocity.x<=GameControl.instance.scrollSpeed || posCameraSpace.x >= 1 && rb.velocity.x > 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (posCameraSpace.y <= 0 && rb.velocity.y < 0 || posCameraSpace.y >= 1 && rb.velocity.x > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x,0);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Obstacle>())
        {
            rb.velocity = Vector2.zero;

        }
    }
}
