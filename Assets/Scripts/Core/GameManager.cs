using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Texture2D crosshair;
    [Header("Scenes")]
    [SerializeField] int levelScene;
    [SerializeField] int uiScene;
    [SerializeField] int entityScene;

    float gameTime;
    bool inMenu;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        Cursor.SetCursor(crosshair, new Vector2(crosshair.width / 2, crosshair.height / 2), CursorMode.Auto);
        InitializeGame();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            OnPause();
    }

    private void InitializeGame()
    {
        AsyncOperation async;
        SceneManager.LoadSceneAsync(levelScene, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(uiScene, LoadSceneMode.Additive);
        async = SceneManager.LoadSceneAsync(entityScene, LoadSceneMode.Additive);
        async.completed += SetActiveScene;

        Time.timeScale = 1;
        gameTime = Time.unscaledTime;
    }
    private void ReloadGame()
    {
        SceneManager.UnloadSceneAsync(entityScene);
        SceneManager.UnloadSceneAsync(levelScene);
        SceneManager.UnloadSceneAsync(uiScene);
        InitializeGame();
    }

    private void SetActiveScene(AsyncOperation ao)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(entityScene));
    }

    public void OnWin()
    {
        TimeSpan time = TimeSpan.FromSeconds(Time.unscaledTime - gameTime);
        UIManager.Instance.ShowMenu("You Win!", "Your time:\n" + time.ToString(@"h\:mm\:ss\.ffff"));
        Time.timeScale = 0;
        inMenu = true;
    }

    public void OnLose()
    {
        UIManager.Instance.ShowMenu("You Lose!", "");
        Time.timeScale = 0;
        inMenu = true;
    }

    public void OnPause()
    {
        if (inMenu)
        {
            UIManager.Instance.CloseMenu();
            Time.timeScale = 1;
            inMenu = false;
        }
        else
        {
            UIManager.Instance.ShowMenu("Paused", "");
            Time.timeScale = 0;
            inMenu = true;
        }
    }

    public void OnRestart()
    {
        ReloadGame();
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
