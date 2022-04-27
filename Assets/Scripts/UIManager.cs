using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text startingEnergy;
    public Text currentEnergy;
    public Text lostEnergy;

    public Text potentialEnergy;
    public Text kineticEnergy;

    public Text speedVector;
    public Text directionVector;
    public Text accelerationVector;

    public Text work;
    public Text momentum;

    public Text distance;
    public Text displacement;

    public GameObject expand;
    public GameObject shrink;
    public RectTransform panel;
    private bool panelOpened = true;
    private Coroutine panelOpening;

    public Text envMass;
    public Text envGravity;
    public Text envFriction;
    public Text envBounciness;
    public Slider sliderMass;
    public Slider sliderGravity;
    public Slider sliderFriction;
    public Slider sliderBounciness;

    public GameObject ortamPaneli;
    public GameObject hakkındaPaneli;

    public Button dorkoduButon;

    private System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo()
    {
        NumberDecimalSeparator = "."
    };

    public void Init()
    {
        Instance = this;

        sliderMass.onValueChanged.AddListener(delegate
        {
            envMass.text = sliderMass.value.ToString("0.0", nf) + "kg";
            SceneManager.Instance.player.GetComponent<Rigidbody2D>().mass = sliderMass.value;
        });

        sliderGravity.onValueChanged.AddListener(delegate
        {
            envGravity.text = sliderGravity.value.ToString("0.0", nf) + "m/s²";
            Physics2D.gravity = new Vector2(Physics2D.gravity.x, sliderGravity.value);
        });

        sliderFriction.onValueChanged.AddListener(delegate
        {
            envFriction.text = sliderFriction.value.ToString("0.0", nf);
            GameObject[] objects = GameObject.FindGameObjectsWithTag("fizik");
            for (int i = 0; i < objects.Length; ++i)
            {
                PhysicsMaterial2D material = objects[i].GetComponent<Collider2D>().sharedMaterial;
                material.friction = sliderFriction.value;
                objects[i].GetComponent<Collider2D>().sharedMaterial = material;
            }
        });

        sliderBounciness.onValueChanged.AddListener(delegate
        {
            envBounciness.text = sliderBounciness.value.ToString("0.0", nf);
            GameObject[] objects = GameObject.FindGameObjectsWithTag("fizik");
            for (int i = 0; i < objects.Length; ++i)
            {
                PhysicsMaterial2D material = objects[i].GetComponent<Collider2D>().sharedMaterial;
                material.bounciness = sliderBounciness.value;
                objects[i].GetComponent<Collider2D>().sharedMaterial = material;
            }
        });

        //sliderMass.onValueChanged.Invoke(sliderMass.value);
        //sliderGravity.onValueChanged.Invoke(sliderGravity.value);
        sliderFriction.onValueChanged.Invoke(sliderFriction.value);
        sliderBounciness.onValueChanged.Invoke(sliderBounciness.value);
    }

    public void TogglePanel()
    {
        panelOpened = !panelOpened;
        if (!panelOpened)
        {
            ortamPaneli.SetActive(false);
            hakkındaPaneli.SetActive(false);
        }
        if (panelOpening != null)
        {
            StopCoroutine(panelOpening);
            panelOpening = null;
        }
        panelOpening = StartCoroutine(TogglingPanel());
        shrink.SetActive(panelOpened);
        expand.SetActive(!panelOpened);
    }

    private IEnumerator TogglingPanel()
    {
        Vector2 current = panel.anchoredPosition;
        Vector2 target = panelOpened ? new Vector2(0, 0) : new Vector2(-400, 0);

        float timeOfTravel = 0.5f; //time after object reach a target place 
        float currentTime = 0; // actual floting time 
        float normalizedValue = 0;

        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 

            panel.anchoredPosition = Vector3.Lerp(current, target, normalizedValue);
            yield return null;
        }
    }

    public void ToggleOrtam()
    {
        hakkındaPaneli.SetActive(false);
        ortamPaneli.SetActive(!ortamPaneli.activeSelf);
    }

    public void ToggleHakkında()
    {
        ortamPaneli.SetActive(false);
        hakkındaPaneli.SetActive(!hakkındaPaneli.activeSelf);
    }

    public void OpenDorkod()
    {
        Application.OpenURL("https://dorkodu.com");
    }
}