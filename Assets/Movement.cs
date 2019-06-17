using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;


//This class is managing the player's movement ONLY. Rest of the game is in "Scrolling Object". If you want the player to also scroll again, drag the scrolling object to its components
public class Movement : MonoBehaviour
{
    public Camera myCamera;
    public Animator animator;

    //This is the only thing you should change, it scales the speed of the player
    public float speedScale = 1f;
    
    public static SerialPort spMovement;

    private long velocity;
    private float degree;
    private string data = "";
    private float startingPitch;
    private float startingRoll;
    private bool gotStartingPosition = false;
    private float modifiedDegree;
    private float speed;
    private float dirX;
    private float dirY;
    private Rigidbody2D rb;
    private Transform trans;
   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        
        //If you switch bluetooth, this part needs to change to the correct COM and frequency of the new bluetooth
        spMovement = new SerialPort("COM4", 38400);
        
        if (!spMovement.IsOpen)
        {
            spMovement.Open();
            spMovement.ReadTimeout = 10;
            spMovement.Handshake = Handshake.None;
            //Debug.Log("Opened port");
        }

        //Because the FPS of unity is much lower than the amount of messages the arduino sends (60FPS vs amount of ticks in a second), a new thread is needed to run the input recieving so we can take care of all inputs in time instead of having a delay of 20 seconds
        Thread btThread = new Thread(new ThreadStart(readFromStream));
        btThread.IsBackground = true;
        btThread.Start();   

    }

    void readFromStream()
    {
        //Reading from input every tick, not necesserily using it all, only the most recent input
        while (true)
        {

            if (spMovement.IsOpen)
            {
                try
                {
                    data = spMovement.ReadLine();
                    Debug.Log("Data: " + data);
                }
                catch (System.TimeoutException) { Debug.Log("Timeout"); }

                spMovement.BaseStream.Flush();

            }
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (animator.GetFloat("timeSinceBlink") > 4)
            animator.SetFloat("timeSinceBlink", 0);
        animator.SetFloat("timeSinceBlink", animator.GetFloat("timeSinceBlink") + Time.deltaTime);

        //Once per frame getting the most recent input from arduino and translating it into movement parameters
        float pitch, roll;

        string[] msg = data.ToString().Split(' ');
        if (spMovement.IsOpen)
        {
           
            try
            {
                roll = System.Convert.ToSingle(msg[2]);
                pitch = System.Convert.ToSingle(msg[1]);


                //The direction is calculated relative to the first position
                if (!gotStartingPosition)
                {
                    startingPitch = pitch;
                    startingRoll = roll;
                    gotStartingPosition = true;
                }

                speed = Mathf.Sqrt(Mathf.Pow(roll - startingRoll, 2) + Mathf.Pow(pitch - startingPitch, 2)) * speedScale; //Calculating distance of velocity vector
                dirX = startingPitch - pitch;
                dirY = roll - startingRoll;
                //Debug.Log("dirx: " + dirX + ", dirY: " + dirY + ", speed: " + speed);

            }
            catch (System.FormatException)
            {

            }
            catch (System.IndexOutOfRangeException) {  }
        }    
}


    private void FixedUpdate()
    {
        //This is where the movement actually happens. Creating a velocity vector with the data we recieved in Update
        rb.velocity = new Vector2(dirX * speed, dirY * speed);

        //Stopping the player from exiting the screen
        Vector3 posCameraSpace = myCamera.WorldToViewportPoint(trans.position);
        if (posCameraSpace.x <= 0 && rb.velocity.x < 0 || posCameraSpace.x >= 1 && rb.velocity.x > 0)
        {
            //If the player is at the leftest AND trying to go more left (same with right), we zero out his velocity only on the x-axis so it won't null out his movemnt completly
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (posCameraSpace.y <= 0 && rb.velocity.y < 0 || posCameraSpace.y >= 1 && rb.velocity.x > 0)
        {
            //Same thing for y-axis
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Stopping the player in case they hit an obstacle (the check is obsolete since there're no more borders
        if (collision.gameObject.GetComponent<Obstacle>())
        {
            rb.velocity = Vector2.zero;

        }
    }

    private void OnApplicationQuit()
    {
        //Closing the port
        if(spMovement.IsOpen)
            spMovement.Close();
    }
}
