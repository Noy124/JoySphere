using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class manages the scrolling effect. Drag this script to any object you want scrolling
//Note that in order to make the player scroll you need to change the speed values in the "moving outside of camera" check inside Movement class
public class ScrollingObject : MonoBehaviour
{
    public bool hasSpecialScrollSpeed=false;
    public float specialScrollSpeed = -0.5f;
    private Rigidbody2D rb2d;
    private float scrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (hasSpecialScrollSpeed)
        {
            scrollSpeed = specialScrollSpeed;
        }
        else
        {
            scrollSpeed = GameControl.instance.scrollSpeed;
        }

        rb2d.velocity = new Vector2(scrollSpeed,0);
    }

    //Unlike start which is called once, this function is called everytime someone spawns the object
    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Vector2 temp = new Vector2(scrollSpeed, 0);
        rb2d.velocity = temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.instance.gameOver || GameControl.instance.pause)
        {
            rb2d.velocity = new Vector2(0, 0);

        }
        else
        {
            rb2d.velocity = new Vector2(scrollSpeed, 0);
        }

    }
}
