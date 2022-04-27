using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Scene
{
    None = -1,
    TriangleFall,
    Fall
}

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public GameObject player;

    private Scene scene = Scene.TriangleFall;
    private bool loading = false;

    public void Init()
    {
        Instance = this;

        LoadScene(Scene.TriangleFall);
    }

    public void LoadScene(Scene _scene)
    {
        if (loading) return;
        loading = !loading;

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_scene.ToString(), UnityEngine.SceneManagement.LoadSceneMode.Additive).completed += (AsyncOperation o) =>
        {
            loading = false;
            scene = _scene;
            player = GameObject.Find("Player");
            player.GetComponent<Player>().RestartSimulation();
        };
    }

    public void UnloadScene(Scene _scene)
    {
        if (loading) return;
        loading = !loading;

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_scene.ToString()).completed += (AsyncOperation o) =>
        {
            loading = false;
            scene = Scene.None;
            player = null;
        };
    }

    public void RestartSimulation()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadScene(scene.ToString());
        LoadScene(scene);
    }

    public void StopSimulation()
    {
        player.GetComponent<Player>().StopSimulation();
    }

    public void ResumeSimulation()
    {
        player.GetComponent<Player>().ResumeSimulation();
    }
}
