using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;

    public Camera cam;

    public UIManager uiManager;
    public SceneManager sceneManager;
    public CameraManager cameraManager;

    public GameObject[] blackbars;

    void Awake()
    {
        Instance = this;

        sceneManager.Init();
        uiManager.Init();
        cameraManager.Init();

        System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo()
        {
            NumberGroupSeparator = ","
        };
    }
}
