using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public Rigidbody2D rb;
    public Player player;

    private float startPosX;
    private float startPosY;
    private bool isBeingHeld;

    public bool accelerated;
    private Vector2 oldPos;
    private Vector2 newPos;

    void Update()
    {
        if (isBeingHeld)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Game.Instance.cam.ScreenToWorldPoint(mousePos);
            transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
            oldPos = newPos;
            if (rb)
            {
                newPos = rb.position;
                rb.velocity = Vector2.zero;
            }
            if (player)
            {
                player.RestartSimulation();
            }
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Game.Instance.cam.ScreenToWorldPoint(mousePos);
            startPosX = mousePos.x - transform.localPosition.x;
            startPosY = mousePos.y - transform.localPosition.y;
            isBeingHeld = true;
            if (rb && !accelerated)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnMouseUp()
    {
        isBeingHeld = false;
        if (rb && accelerated)
        {
            rb.velocity = (newPos - oldPos) * 10;
        }
        newPos = Vector2.zero;
        oldPos = Vector2.zero;
    }
}
