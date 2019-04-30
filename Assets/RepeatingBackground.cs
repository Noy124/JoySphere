using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    private BoxCollider2D borderColliader;
    private float borderLength;
    // Start is called before the first frame update
    void Start()
    {
        borderColliader = GetComponent<BoxCollider2D>();
        borderLength = borderColliader.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -26)
        {
            RepositionBackground();
        }
    }

    private void RepositionBackground()
    {
        Vector2 borderOffset = new Vector2(52, 0);
        transform.position = (Vector2)transform.position + borderOffset;
    }
}
