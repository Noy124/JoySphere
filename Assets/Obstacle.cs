using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject thisPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
            else
            {
                GameControl.health -= 1;
                ObstaclePool.LowerDifficulty();
            }
            this.gameObject.SetActive(false);
            thisPlayer.GetComponent<ColorControl>().ResetColor();
            
        }

    }
}
