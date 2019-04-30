using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControl : MonoBehaviour
{
    public enum color {white,red,blue,yellow,purple,orange,green,black};
    public static Color orange = new Color(1, 0.549f, 0);

    private color currentColor = color.white;
    private color nextColor = color.white;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
        {
            nextColor = color.blue;
        }else if (Input.GetKey("2"))
        {
            nextColor = color.yellow;
        }else if (Input.GetKey("3"))
        {
            nextColor = color.red;
        }else if(Input.GetKey("4"))
        {
            nextColor = color.white;
        }

        if(sr.color == Color.white)
        {
            currentColor = nextColor;
        }else if(sr.color == Color.blue){
            switch (nextColor)
            {
                case color.yellow: currentColor = color.green;break;
                case color.red: currentColor = color.purple;break;
                case color.white: currentColor = color.white;break;
                case color.blue: currentColor = color.blue;break;
            }
        }else if(sr.color == Color.red)
        {
            switch (nextColor)
            {
                case color.yellow: currentColor = color.orange; break;
                case color.red: currentColor = color.red; break;
                case color.white: currentColor = color.white; break;
                case color.blue: currentColor = color.purple; break;
            }
        }else if(sr.color == Color.yellow)
        {
            switch (nextColor)
            {
                case color.yellow: currentColor = color.yellow; break;
                case color.red: currentColor = color.orange; break;
                case color.white: currentColor = color.white; break;
                case color.blue: currentColor = color.green; break;
            }
        }else if(sr.color == Color.magenta)
        {
            switch (nextColor)
            {
                case color.yellow: currentColor = color.black; break;
                case color.red: currentColor = color.purple; break;
                case color.white: currentColor = color.white; break;
                case color.blue: currentColor = color.purple; break;
            }
        }else if(sr.color == Color.green)
        {
            switch (nextColor)
            {
                case color.yellow: currentColor = color.green; break;
                case color.red: currentColor = color.black; break;
                case color.white: currentColor = color.white; break;
                case color.blue: currentColor = color.green; break;
            }
        }else if(sr.color == Color.black)
        {
            switch (nextColor)
            {
                case color.yellow: currentColor = color.black; break;
                case color.red: currentColor = color.black; break;
                case color.white: currentColor = color.white; break;
                case color.blue: currentColor = color.black; break;
            }
        }else if(sr.color == orange)
        {
            switch (nextColor)
            {
                case color.yellow: currentColor = color.orange; break;
                case color.red: currentColor = color.orange; break;
                case color.white: currentColor = color.white; break;
                case color.blue: currentColor = color.black; break;
            }
        }

        switch (currentColor)
        {
            case color.white: sr.color = Color.white;break;
            case color.red: sr.color = Color.red; break;
            case color.blue: sr.color = Color.blue; break;
            case color.yellow: sr.color = Color.yellow; break;
            case color.purple: sr.color = Color.magenta; break;
            case color.green: sr.color = Color.green; break;
            case color.black: sr.color = Color.black; break;
            case color.orange: sr.color = orange; break;
        }
    }

    public void ResetColor()
    {
        currentColor = color.white;
        nextColor = color.white;
    }
}
