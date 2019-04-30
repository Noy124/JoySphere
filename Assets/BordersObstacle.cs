using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersObstacle : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
            GameControl.health -= 1;

    }
}
