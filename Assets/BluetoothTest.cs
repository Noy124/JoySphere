using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO.Ports;

public class BluetoothTest : MonoBehaviour
{
    SerialPort sp;

    // Start is called before the first frame update
    void Start()
    {
        sp = new SerialPort("COM7", 9600);
        if (!sp.IsOpen)
        {
            sp.Open();
            sp.ReadTimeout = 100;
            sp.Handshake = Handshake.None;
        }
    }

    // Update is called once per frame
    void Update()
    {
  //      if (sp.IsOpen)
    //    {
            //print("SP is OPEN");
      //      data = sp.ReadLine();
        //    sp.BaseStream.Flush();

            //if (data != null) {
            //print("data recieved");
          //  text = data.ToString();
            //float step = speed * Time.deltaTime;
            //toPos.Set(data[6],data[7],data[8]);
            //rb.rotation.SetLookRotation(toPos,rb.position);
            //}

        //}
    }
}
