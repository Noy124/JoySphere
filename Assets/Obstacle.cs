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
                GameControl.instance.Score();
            }
            else
            {
                GameControl.health -= 1;
            }
            this.gameObject.SetActive(false);
            thisPlayer.GetComponent<ColorControl>().ResetColor();
            
        }

    }
}
