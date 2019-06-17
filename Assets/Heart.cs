using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Heart : MonoBehaviour
{
    public GameObject thisPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == thisPlayer)
        {
            this.gameObject.SetActive(false);
            GameControl.health += 1;
        }
    }
}
