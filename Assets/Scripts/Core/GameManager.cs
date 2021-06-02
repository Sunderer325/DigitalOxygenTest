using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] int levelScene;
    [SerializeField] int uiScene;
    [SerializeField] int entityScene;

    [HideInInspector] public bool[] isHeroesLive = { true, true, true };
    [HideInInspector] public int selectedHero;
    bool firstTime = true;
    bool gameInit;
    float gameTime;
    bool inMenu;

    GameStates gameState = GameStates.MAIN_MENU;
    public GameStates GetState => gameState;
    public void SetState(GameStates state)
    {
        if(state == GameStates.STORY && !firstTime)
            state = GameStates.CHARACTERS;

        gameState = state;
    }

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
        InitializeApp();
    }

    private void OnEnable()
    {
        UnityEngine.Cursor.visible = false;
    }

    private void Update()
    {
        if (gameState == GameStates.GAME)
        {
            if (!gameInit)
            {
                LoadGame();
                gameInit = true;
            }

            if (Input.GetButtonDown("Cancel"))
                OnPause();
        }
        else
        {
            if (gameState == GameStates.PAUSE)
                return;

            if (gameInit)
            {
                UnloadGame();
                gameInit = false;
            }
        }
    }

    private void InitializeApp()
    {
        SceneManager.LoadScene(levelScene, LoadSceneMode.Additive);
        SceneManager.LoadScene(uiScene, LoadSceneMode.Additive);
        //async = SceneManager.LoadSceneAsync(entityScene, LoadSceneMode.Additive);
        //async.completed += SetActiveScene;
    }

    private void LoadGame()
    {
        AsyncOperation async;
        async = SceneManager.LoadSceneAsync(entityScene, LoadSceneMode.Additive);
        async.completed += SetActiveScene;

        Time.timeScale = 1;
        gameTime = Time.unscaledTime;
    }
    private void UnloadGame()
    {
        //SceneManager.UnloadScene(levelScene);
        //SceneManager.UnloadScene(uiScene);
        SceneManager.UnloadScene(entityScene);
    }

    private void SetActiveScene(AsyncOperation ao)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(entityScene));
    }

    public void OnWin()
    {
        TimeSpan time = TimeSpan.FromSeconds(Time.unscaledTime - gameTime);
        gameState = GameStates.PAUSE;
        inMenu = true;
    }

    public void OnLose()
    {
        gameState = GameStates.PAUSE;
        inMenu = true;
        isHeroesLive[selectedHero] = false;
    }

    public void OnPause()
    {
        if (inMenu)
        {
            gameState = GameStates.GAME;
            inMenu = false;
        }
        else
        {
            gameState = GameStates.PAUSE;
            inMenu = true;
        }
    }
}

public enum GameStates
{
    MAIN_MENU,
    STORY,
    CHARACTERS,
    GAME,
    PAUSE
}
