using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ColorControl : MonoBehaviour
{
    public enum color {white,red,blue,green,purple,cyan,yellow,black};
    public static Color orange = new Color(1, 0.549f, 0);
    public int changeAngle=30;

    private color currentColor = color.white;
    private color nextColor = color.white;
    private SpriteRenderer sr;
    private SerialPort sp;
    private bool gotNewColor = false;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sp = new SerialPort("COM7", 9600);
        if (!sp.IsOpen)
        {
            sp.Open();
            sp.ReadTimeout = 10;
            sp.Handshake = Handshake.None;
        }

        SendAngle();
        //while (true)
        //{
            
        //if (approve != null)
        //{
        //    if (approve.ToString() == "OK")
        //        break;
        //}
        //}

        sp.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
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

                nextColorText = data.ToString();
                Debug.Log(nextColorText);

                if (nextColorText == "1" && currentColor != color.green)
                {
                    nextColor = color.green;
                    gotNewColor = true;
                    Debug.Log(data);
                }
                else if (nextColorText == "2" && currentColor != color.red)
                {
                    nextColor = color.red;
                    gotNewColor = true;
                    Debug.Log(data);
                }
                else if (nextColorText == "3" && currentColor != color.blue)
                {
                    nextColor = color.blue;
                    gotNewColor = true;
                    Debug.Log(data);
                }
                else if (nextColorText == "4" && currentColor != color.white)
                {
                    nextColor = color.white;
                    gotNewColor = true;
                    Debug.Log(data);
                }else if (nextColorText == "9")
                {
                    SendAngle();
                    gotNewColor = false;
                    
                }
                //    switch (nextColorText)
                //  {
                //    case "1": nextColor = color.yellow;
                //      gotNewColor = true; break;
                // case "2": nextColor = color.red;
                //    gotNewColor = true; break;
                // case "3": nextColor = color.blue;
                //    gotNewColor = true; break;
                // case "4": nextColor = color.white;
                //    gotNewColor = true; break;

                //}

            }
        }

        if (Input.GetKey("1"))
        {
           nextColor = color.blue;
            gotNewColor = true;
        }
        else if (Input.GetKey("2"))
        {
           nextColor = color.green;
            gotNewColor = true;
        }
        else if (Input.GetKey("3"))
        {
           nextColor = color.red;
            gotNewColor = true;
        }
        else if(Input.GetKey("4"))
        {
           nextColor = color.white;
            gotNewColor = true;
        }

        if (gotNewColor && !GameControl.instance.gameOver && !GameControl.instance.pause)
        {
            gotNewColor = false;
            if (sr.color == Color.white)
            {
                currentColor = nextColor;
            }
            else if (sr.color == Color.blue)
            {
                switch (nextColor)
                {
                    case color.green: currentColor = color.cyan; break;
                    case color.red: currentColor = color.purple; break;
                    case color.white: currentColor = color.white; break;
                    case color.blue: currentColor = color.blue; break;
                }
            }
            else if (sr.color == Color.red)
            {
                switch (nextColor)
                {
                    case color.green: currentColor = color.yellow; break;
                    case color.red: currentColor = color.red; break;
                    case color.white: currentColor = color.white; break;
                    case color.blue: currentColor = color.purple; break;
                }
            }
            else if (sr.color == Color.green)
            {
                switch (nextColor)
                {
                    case color.green: currentColor = color.green; break;
                    case color.red: currentColor = color.yellow; break;
                    case color.white: currentColor = color.white; break;
                    case color.blue: currentColor = color.cyan; break;
                }
            }
            else if (sr.color == Color.magenta)
            {
                switch (nextColor)
                {
                    case color.green: currentColor = color.black; break;
                    case color.red: currentColor = color.purple; break;
                    case color.white: currentColor = color.white; break;
                    case color.blue: currentColor = color.purple; break;
                }
            }
            else if (sr.color == Color.yellow)
            {
                switch (nextColor)
                {
                    case color.green: currentColor = color.yellow; break;
                    case color.red: currentColor = color.yellow; break;
                    case color.white: currentColor = color.white; break;
                    case color.blue: currentColor = color.black; break;
                }
            }
            else if (sr.color == Color.black)
            {
                switch (nextColor)
                {
                    case color.green: currentColor = color.black; break;
                    case color.red: currentColor = color.black; break;
                    case color.white: currentColor = color.white; break;
                    case color.blue: currentColor = color.black; break;
                }
            }
            else if (sr.color == Color.cyan)
            {
                switch (nextColor)
                {
                    case color.green: currentColor = color.cyan; break;
                    case color.red: currentColor = color.black; break;
                    case color.white: currentColor = color.white; break;
                    case color.blue: currentColor = color.cyan; break;
                }
            }

            switch (currentColor)
            {
                case color.white: sr.color = Color.white; SendCommand("0"); break;
                case color.red: sr.color = Color.red; SendCommand("1");  break;
                case color.blue: sr.color = Color.blue; SendCommand("2");  break;
                case color.yellow: sr.color = Color.yellow; SendCommand("3");  break;
                case color.purple: sr.color = Color.magenta; SendCommand("4");  break;
                case color.green: sr.color = Color.green; SendCommand("5");  break;
                case color.black: sr.color = Color.black; SendCommand("7"); break;
                case color.cyan: sr.color = Color.cyan; SendCommand("6");  break;
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            SendCommand("R");
        }
    }

    public void ResetColor()
    {
        currentColor = color.white;
        nextColor = color.white;
        sr.color = Color.white;
        gotNewColor = false;
        Debug.Log("RESET!!");
    }

    public void SendCommand(string cmd)
    {
        try
        {
            sp.WriteLine(cmd);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.ToString());
        }
    }

    public void SendAngle()
    {
        sp.WriteLine("A " + changeAngle);
        string approve = null;
        try
        {
            approve = sp.ReadLine();
        }
        catch (System.TimeoutException e)
        { }

        sp.BaseStream.Flush();

    }

    private void OnApplicationQuit()
    {
        sp.Close();
    }
}
