using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform vector;
    public Transform drag;

    private Vector2 startPos;
    private Vector2 endPos;

    private Vector2 oldPos;
    private Vector2 newPos;
    private float startEnergy;
    private float currentEnergy;
    private float distance = 0f;

    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private bool dragging;

    private System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo()
    {
        NumberDecimalSeparator = "."
    };

    void FixedUpdate()
    {
        // Dragging
        if (dragging)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Game.Instance.cam.ScreenToWorldPoint(mousePos);
            dragEndPos = mousePos;

            drag.up = (dragStartPos - dragEndPos).normalized;
            drag.localScale = new Vector3(drag.localScale.x, (dragEndPos - dragStartPos).magnitude * 2f, drag.localScale.z);
        }

        vector.up = rb.velocity.normalized;
        vector.localScale = new Vector3(vector.localScale.x, rb.velocity.magnitude * 0.5f, vector.localScale.z);

        // Physics properties
        oldPos = newPos;
        newPos = rb.position;
        if (rb.velocity.magnitude == 0f)
        {
            endPos = rb.position;

            string work = (rb.mass * -Physics2D.gravity.y * (endPos - startPos).magnitude).ToString("0.0", nf);
            UIManager.Instance.work.text = $"{work}j";

            string displacement = ((endPos - startPos).magnitude).ToString("0.0", nf);
            UIManager.Instance.displacement.text = $"{displacement}m";
        }

        distance += (newPos - oldPos).magnitude;
        string distanceText = distance.ToString("0.0", nf);
        UIManager.Instance.distance.text = $"{distanceText}m";

        string velX = rb.velocity.x.ToString("0.0", nf);
        string velY = rb.velocity.y.ToString("0.0", nf);
        UIManager.Instance.speedVector.text = $"({velX},{velY})";

        string velNormX = rb.velocity.normalized.x.ToString("0.0", nf);
        string velNormY = rb.velocity.normalized.y.ToString("0.0", nf);
        UIManager.Instance.directionVector.text = $"({velNormX},{velNormY})";

        string accX = (newPos - oldPos).x.ToString("0.0", nf);
        string accY = (newPos - oldPos).y.ToString("0.0", nf);
        UIManager.Instance.accelerationVector.text = $"({accX},{accY})";

        currentEnergy = rb.mass * -Physics2D.gravity.y * (6.5f + rb.position.y) + 0.5f * rb.mass * rb.velocity.magnitude * rb.velocity.magnitude;
        string currentEnergyText = currentEnergy.ToString("0.0", nf);
        UIManager.Instance.currentEnergy.text = $"{currentEnergyText}j";

        string lostEnergy = (startEnergy - currentEnergy).ToString("0.0", nf);
        UIManager.Instance.lostEnergy.text = $"{lostEnergy}j";

        string potentialEnergy = (rb.mass * -Physics2D.gravity.y * (6.5f + rb.position.y)).ToString("0.0", nf);
        UIManager.Instance.potentialEnergy.text = $"{potentialEnergy}j";

        string kineticEnergy = (0.5f * rb.mass * rb.velocity.magnitude * rb.velocity.magnitude).ToString("0.0", nf);
        UIManager.Instance.kineticEnergy.text = $"{kineticEnergy}j";

        string momentum = (rb.mass * rb.velocity.magnitude).ToString("0.0", nf);
        UIManager.Instance.momentum.text = $"{momentum}Ns";
    }

    public void RestartSimulation()
    {
        distance = 0f;
        startPos = rb.position;
        // mgh + 1/2mv^2
        startEnergy = rb.mass * -Physics2D.gravity.y * (6.5f + rb.position.y) + 0.5f * rb.mass * rb.velocity.magnitude * rb.velocity.magnitude;
        string startEnergyText = startEnergy.ToString("0.0", nf);
        UIManager.Instance.startingEnergy.text = $"{startEnergyText}j";

        UIManager.Instance.sliderMass.value = 1;
        UIManager.Instance.sliderGravity.value = -9.81f;
        UIManager.Instance.sliderFriction.value = 0.1f;
        UIManager.Instance.sliderBounciness.value = 0.5f;
    }

    public void StopSimulation()
    {
        Time.timeScale = 0;
    }

    public void ResumeSimulation()
    {
        startPos = rb.position;
        Time.timeScale = 1;
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;

            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Game.Instance.cam.ScreenToWorldPoint(mousePos);
            dragStartPos = mousePos;
        }
    }

    void OnMouseUp()
    {
        dragging = false;
        rb.velocity = (dragStartPos - dragEndPos) * 5f;
        drag.localScale = new Vector3(drag.localScale.x, 0f, drag.localScale.z);

        distance = 0f;
        startPos = rb.position;
        // mgh + 1/2mv^2
        startEnergy = rb.mass * -Physics2D.gravity.y * (6.5f + rb.position.y) + 0.5f * rb.mass * rb.velocity.magnitude * rb.velocity.magnitude;
        string startEnergyText = startEnergy.ToString("0.0", nf);
        UIManager.Instance.startingEnergy.text = $"{startEnergyText}j";
    }
}
