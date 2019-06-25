﻿using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;


//This class manages the color change of the player. Right now it's set to use the keyboard only, the arduino input is disabled. I marked areas that need changing when you need to switch to the ball
public class ColorControl : MonoBehaviour
{
    public int changeAngle=30; //The angle it needs to detect in order to count as a color change
    public GameObject body;
    public Sprite black, white, red, green, blue, yellow, magenta, cyan;
    public static SerialPort sp;

    private Color currentColor = Color.black;
    private Color nextColor = Color.black;
    private SpriteRenderer sr;
    private bool gotNewColor = false;
    private string approve = null; //~~~~~~~~ Uncommen this line for the ball ~~~~~~~~~~~


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentColor = Color.black;
        nextColor = Color.black;
        //Debug.Log("Start current color: " + currentColor);
        sr.color = Color.black;
        //~~~~~~~~ Start uncommenting from here for the ball ~~~~~~~~~~~

        sp = new SerialPort("COM7", 9600);
        if (!sp.IsOpen)
        {
            sp.Open();
            sp.ReadTimeout = 10;
            sp.Handshake = Handshake.None;
        }

        //Sends the angle to the arduino untill it gets back an OK response

        while (true)
        {
            SendAngle();

            if (approve != null)
            {
                if (approve.ToString() == "OK")
                    break;
            }
        }

        sp.ReadTimeout = 1;

        //~~~~~~~~ Stop uncommenting from here for the ball ~~~~~~~~~~~
    }

    // Update is called once per frame
    void Update()
    {
        //~~~~~~~~ Start uncommenting from here for the ball ~~~~~~~~~~~

        string data = null;
        if (sp.IsOpen)
        {
            try
            {
                data = sp.ReadLine();
            }
            catch (System.TimeoutException e)
            { }

            sp.BaseStream.Flush();

            string nextColorText;
            if (data != null)
            {
                //Converts arduino input to color

                nextColorText = data.ToString();
                Debug.Log(nextColorText);

                if (nextColorText == "1" && currentColor != Color.green)
                {
                    nextColor = Color.green;
                    gotNewColor = true;
                    Debug.Log(data);
                }
                else if (nextColorText == "2" && currentColor != Color.red)
                {
                    nextColor = Color.red;
                    gotNewColor = true;
                    Debug.Log(data);
                }
                else if (nextColorText == "3" && currentColor != Color.blue)
                {
                    nextColor = Color.blue;
                    gotNewColor = true;
                    Debug.Log(data);
                }
                else if (nextColorText == "4" && currentColor != Color.white)
                {
                    nextColor = Color.white;
                    gotNewColor = true;
                    Debug.Log(data);
                }
                else if (nextColorText == "9")
                {
                    //In case the ball went through a reset, sending angle again

                    SendAngle();
                    gotNewColor = false;

                }

            }
        }
        //~~~~~~~~ Stop uncommenting from here for the ball ~~~~~~~~~~~

        //~~~~~~~~ Start uncommenting from here to disable keyboard input ~~~~~~~~~~~

        if (Input.GetKey("1"))
        {
           nextColor = Color.blue;
            gotNewColor = true;
            //Debug.Log("Blue curren color: " + currentColor);
        }
        else if (Input.GetKey("2"))
        {
           nextColor = Color.green;
            gotNewColor = true;
        }
        else if (Input.GetKey("3"))
        {
           nextColor = Color.red;
            gotNewColor = true;
        }
        else if(Input.GetKey("4"))
        {
           nextColor = Color.black;
            gotNewColor = true;
        }
        //~~~~~~~~ Stop uncommenting from here to disable keyboard input ~~~~~~~~~~~


        //From here on calculates the new player color
        if (gotNewColor && !GameControl.instance.gameOver && !GameControl.instance.pause)
        {
            //Debug.Log("Current color: " + currentkColor);
            gotNewColor = false;
            if (sr.color == Color.black)
            {
                currentColor = nextColor;
            }
            else if (sr.color == Color.blue)
            {

                if (nextColor == Color.green)
                    currentColor = Color.cyan;

                if (nextColor == Color.red)
                    currentColor = Color.magenta;

                if (nextColor == Color.black)
                    currentColor = Color.black;

                if (nextColor == Color.blue)
                    currentColor = Color.blue;

            }

            else if (sr.color == Color.red)
            {

                if (nextColor == Color.green)
                    currentColor = Color.yellow;

                if (nextColor == Color.red)
                    currentColor = Color.red;

                if (nextColor == Color.black)
                    currentColor = Color.black;

                if (nextColor == Color.blue)
                    currentColor = Color.magenta;
            }

            else if (sr.color == Color.green)
            {

                if (nextColor == Color.green)
                    currentColor = Color.green;

                if (nextColor == Color.red)
                    currentColor = Color.yellow;

                if (nextColor == Color.black)
                    currentColor = Color.black;

                if (nextColor == Color.blue)
                    currentColor = Color.cyan;
            }

            else if (sr.color == Color.magenta)
            {

                if (nextColor == Color.green)
                    currentColor = Color.white;

                if (nextColor == Color.red)
                    currentColor = Color.magenta;

                if (nextColor == Color.black)
                    currentColor = Color.black;

                if (nextColor == Color.blue)
                    currentColor = Color.magenta;
            }

            else if (sr.color == Color.yellow)
            {

                if (nextColor == Color.green)
                    currentColor = Color.yellow;

                if (nextColor == Color.red)
                    currentColor = Color.yellow;

                if (nextColor == Color.black)
                    currentColor = Color.black;

                if (nextColor == Color.blue)
                    currentColor = Color.white;
            }

            else if (sr.color == Color.white)
            {

                if (nextColor == Color.green)
                    currentColor = Color.white;

                if (nextColor == Color.red)
                    currentColor = Color.white;

                if (nextColor == Color.black)
                    currentColor = Color.black;

                if (nextColor == Color.blue)
                    currentColor = Color.white;
            }

            else if (sr.color == Color.cyan)
            {

                if (nextColor == Color.green)
                    currentColor = Color.cyan;

                if (nextColor == Color.red)
                    currentColor = Color.white;

                if (nextColor == Color.black)
                    currentColor = Color.black;

                if (nextColor == Color.blue)
                    currentColor = Color.cyan;
            }


            sr.color = currentColor;

            SpriteRenderer bsr = body.GetComponent<SpriteRenderer>();

            if (currentColor == Color.black)
            {
                bsr.sprite = black;
            }

            if (currentColor == Color.red)
            {
                bsr.sprite = red;
            }

            if (currentColor == Color.blue)
                bsr.sprite = blue;

            if (currentColor == Color.yellow)
                bsr.sprite = yellow;

            if (currentColor == Color.magenta)
                bsr.sprite = magenta;

            if (currentColor == Color.green)
                bsr.sprite = green;

            if (currentColor == Color.white)
                bsr.sprite = white;

            if (currentColor == Color.cyan)
                bsr.sprite = cyan;


        }

        if (Input.GetKey(KeyCode.R))
        {
            //Reset the bluetooth
            SendCommand("R");
        }
    }

    //For after hitting obstacles
    public void ResetColor()
    {
       
        currentColor = Color.black;
        nextColor = Color.black;
        sr.color = Color.black;
        body.GetComponent<SpriteRenderer>().sprite = black;
        gotNewColor = false;
    }

    public void SendCommand(string cmd)
    {
        //~~~~~~~~ Start uncommenting from here for the ball ~~~~~~~~~~~

        //Send a command to the ball
        try
        {
            sp.WriteLine(cmd);
            Movement.spMovement.WriteLine(cmd);
        }
        catch (System.Exception e)
        {
            //Debug.LogWarning(e.ToString());
        }

        //~~~~~~~~ Stop uncommenting from here for the ball ~~~~~~~~~~~

    }

    //~~~~~~~~ Start uncommenting from here for the ball ~~~~~~~~~~~

    public void SendAngle()
    {
        sp.WriteLine("A " + changeAngle);

        try
        {
            approve = sp.ReadLine();
        }
        catch (System.TimeoutException e)
        { }

        sp.BaseStream.Flush();

    }

    //~~~~~~~~ Stop uncommenting from here for the ball ~~~~~~~~~~~


    private void OnApplicationQuit()
    {
        if(sp.IsOpen)
            sp.Close(); //~~~~~~~~ Uncomment this line for the ball ~~~~~~~~~~~

    }
}
