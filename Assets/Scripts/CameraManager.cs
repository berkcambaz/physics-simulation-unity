using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Camera cam;
    private float aspectRatio;
    private float fieldOfView;
    private bool isOrthographic;
    private float orthographicSize;

    public void Init()
    {
        Instance = this;

        CachePropertiesAndRecalculate();
    }

    private void LateUpdate()
    {
        if (CameraPropertyHasChanged())
            CachePropertiesAndRecalculate();
    }

    private void CacheCameraProperties()
    {
        aspectRatio = cam.aspect;
        fieldOfView = cam.fieldOfView;
        isOrthographic = cam.orthographic;
        orthographicSize = cam.orthographicSize;
    }

    private bool CameraPropertyHasChanged()
    {
        bool hasChanged = (aspectRatio != cam.aspect
            || fieldOfView != cam.fieldOfView
            || isOrthographic != cam.orthographic
            || orthographicSize != cam.orthographicSize);

        return hasChanged;
    }

    private void CachePropertiesAndRecalculate()
    {
        CacheCameraProperties();

        float size = 7f;

        Vector2 defRes = new Vector2(1920f, 1080f);
        Vector2 currRes = new Vector2(Screen.width, Screen.height);
        float dt = size * (defRes.x / defRes.y);

        cam.orthographicSize = dt / (currRes.x / currRes.y);

        float width = cam.ViewportToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x;

        Transform t0 = Game.Instance.blackbars[0].transform;
        t0.position = new Vector3(t0.position.x, cam.orthographicSize, t0.position.z);
        t0.localScale = new Vector3(width, cam.orthographicSize - 7f, t0.localScale.z);

        Transform t1 = Game.Instance.blackbars[1].transform;
        t1.position = new Vector3(t1.position.x, -cam.orthographicSize, t1.position.z);
        t1.localScale = new Vector3(width, cam.orthographicSize - 7f, t1.localScale.z);
    }
}
