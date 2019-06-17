using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is managing the obstacle behivour including collison effect on the player
public class Obstacle : MonoBehaviour
{
    public GameObject thisPlayer;
    public Camera myCamera;
    public bool alreadyInjuredCat = false;


    private Transform trans;

    private void Start()
    {
        trans = GetComponent<Transform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //This check is obsolete because there are no borders
        if (collision.gameObject == thisPlayer)
        {
            SpriteRenderer playerSr = thisPlayer.GetComponent<SpriteRenderer>();
            SpriteRenderer thisSr = GetComponent<SpriteRenderer>();
            if (playerSr.color == thisSr.color)
            {
                if (playerSr.color == Color.red || playerSr.color == Color.blue || playerSr.color == Color.yellow)
                {
                    GameControl.instance.Score();
                }else if(playerSr.color == Color.black)
                {
                    GameControl.instance.Score(3);
                }
                else
                {
                    GameControl.instance.Score(2);
                }
            }
            //else
            //{
            //    //If the color was wrong, lower difficulty
            //    GameControl.health -= 1;
            //    ObstaclePool.LowerDifficulty();
            //}

            this.gameObject.SetActive(false);
            thisPlayer.GetComponent<ColorControl>().ResetColor();
            
        }

    }


    private void OnBecameInvisible()
    {
        GameControl.health -= 1;
        ObstaclePool.LowerDifficulty();
    }


}
