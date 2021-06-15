using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] int levelScene = default;
    [SerializeField] int uiScene = default;
    [SerializeField] int entityScene = default;

    [Header("Culling masks")]
    [SerializeField] LayerMask onlyUIMask = default;
    [SerializeField] LayerMask heroDeathMask = default;
    [SerializeField] LayerMask drawAllMask = default;

    [HideInInspector] public bool ReadyToChangeUI;
    [HideInInspector] public bool PlayerCheck;

    private GameStates gameState;
    public GameStates GameState
    {
        get { return gameState; }
        set
        {
            gameState = value;
            GameStateChanged();
        }
    }

    [HideInInspector] public bool[] IsHeroLive = { true, true, true };
    [HideInInspector] private int selectedHero;

    bool pause;

    new AudioPrefab audio;

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

    public int SelectedHero { get => selectedHero; 
        set 
        { 
            selectedHero = value;
            audio.Stop();
        } 
    }

    private void Awake()
    {
        InitializeGame();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (GameState == GameStates.GAME)
            {
                GameState = GameStates.PAUSE;
                pause = true;
            }
            else if (GameState == GameStates.PAUSE)
            {
                GameState = GameStates.GAME;
            }
        }
    }

    private void InitializeGame()
    {
        ReadyToChangeUI = true;
        audio = GetComponent<AudioPrefab>();

        AsyncOperation async;
        SceneManager.LoadSceneAsync(uiScene, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(levelScene, LoadSceneMode.Additive);
        async = SceneManager.LoadSceneAsync(entityScene, LoadSceneMode.Additive);
        async.completed += GameIsLoaded;
    }

    private void GameIsLoaded(AsyncOperation ao)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(entityScene));
        //UnityEngine.Cursor.visible = false;
        //UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        GameState = GameStates.TUTORIAL;
        EntityManager.Instance.Init();
    }

    private void NextHero()
    {
        GameState = GameStates.CHARACTERS;
        EntityManager.Instance.Init();
        audio.PlayLoop("menu");
    }

    private void SetGameToDefault()
    {
        for (int i = 0; i < IsHeroLive.Length; i++)
        {
            IsHeroLive[i] = true;
        }
        audio.Stop();
        GameState = GameStates.MAIN_MENU;
        EntityManager.Instance.SetToDefault();
        UIManager.Instance.Reset();
        EntityManager.Instance.Init();
    }

    private void GameStateChanged()
    {
        switch (GameState)
        {
            case GameStates.TUTORIAL:
                StartCoroutine(UIManager.Instance.BlinkAnim(UIManager.Instance.clickToContinue, GameStates.TUTORIAL));
                EntityManager.Instance.DisableSpawning = true;
                break;
            case GameStates.MAIN_MENU:
                audio.PlayLoop("menu");
                EntityManager.Instance.DisableSpawning = true;
                break;
            case GameStates.CHARACTERS:
                CameraFollow.Instance.Camera.cullingMask = onlyUIMask;
                EntityManager.Instance.DisableSpawning = true;
                break;
            case GameStates.GAME:
                if (pause)
                {
                    pause = false;
                    EntityManager.Instance.DisableSpawning = false;
                    return;
                }
                CameraFollow.Instance.Camera.cullingMask = drawAllMask;
                EntityManager.Instance.SpawnHero();

                StartCoroutine(LevelManager.Instance.Opening());
                StartCoroutine(Opening());
                break;
            case GameStates.PAUSE:
                StartCoroutine(UIManager.Instance.BlinkAnim(UIManager.Instance.statusText, GameStates.PAUSE));
                EntityManager.Instance.DisableSpawning = true;
                break;
            case GameStates.WIN:
                StartCoroutine(WinEnding());
                EntityManager.Instance.DisableSpawning = true;
                break;
            case GameStates.LOSE:
                StartCoroutine(LoseEnding());
                EntityManager.Instance.DisableSpawning = true;
                break;
        }
    }

    IEnumerator Opening()
    {
        StartCoroutine(LevelManager.Instance.Opening());
        while (LevelManager.Instance.IsOpening)
            yield return null;

        yield return new WaitForSeconds(0.5f);
        PlayerCheck = true;
        while (PlayerCheck)
            yield return null;

        audio.Stop();
        audio.PlayLoop("fight");
        UIManager.Instance.EnableTopBar();
        UIManager.Instance.SetHP(5);
        EntityManager.Instance.DisableSpawning = false;
    }

    IEnumerator WinEnding()
    {
        audio.Stop();
        StartCoroutine(LevelManager.Instance.Ending());
        while (LevelManager.Instance.IsEnding == true)
            yield return null;

        UIManager.Instance.ShowWinScreen();
        audio.Play("victory");
        while (!Input.GetButtonDown("Fire1"))
            yield return null;

        SetGameToDefault();
    }
    IEnumerator LoseEnding()
    {
        audio.Stop();
        CameraFollow.Instance.Camera.cullingMask = heroDeathMask;

        yield return new WaitForSeconds(0.3f);
        audio.Play("gameover");
        yield return new WaitForSeconds(3);

        IsHeroLive[SelectedHero] = false;

        foreach(bool hero in IsHeroLive)
        {
            if (hero == true)
            {
                NextHero();
                yield break;
            }
        }
        SetGameToDefault();
    }
}

public enum GameStates
{
    TUTORIAL,
    MAIN_MENU,
    STORY,
    CHARACTERS,
    GAME,
    PAUSE,
    WIN,
    LOSE
}
