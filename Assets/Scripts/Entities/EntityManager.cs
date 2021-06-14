using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject[] heroes = default;
    [SerializeField] GameObject heroSpawnPoint = default;
    
    GameObject heroInstance;

    [Header("Enemies")]
    [SerializeField] public int EnemiesMax = 100;
    [SerializeField] private Enemy[] enemies = default;
    private List<Enemy> initEnemies = new List<Enemy>();

    [HideInInspector] public int EnemiesSpawnCounter;
    [HideInInspector] public int EnemiesLiveCounter;

    [Header("Spawn Settings")]
    public bool DisableSpawning = default;
    [SerializeField] float firstSpawnDelay = 2f;
    [SerializeField, Min(0.1f)] float spawnTime = default;
    [SerializeField] BoxCollider2D spawnZone = default;

    float spawnTimer;
    float gameTime;
    
    CameraAnimation cameraAnim;
    bool init;

    private static EntityManager _instance;
    public static EntityManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<EntityManager>();
            return _instance;
        }
    }

    public void Init()
    {
        if (init)
        {
            Reload();
        }
        else
        {
            EnemiesLiveCounter = EnemiesSpawnCounter = EnemiesMax;

        }
        UIManager.Instance.Counter.text = EnemiesLiveCounter.ToString();

        cameraAnim = CameraFollow.Instance.gameObject.GetComponent<CameraAnimation>();
        init = true;
    }

    public void SpawnHero()
    {
        if (heroInstance != null)
            Destroy(heroInstance);
        heroInstance = Instantiate(heroes[GameManager.Instance.SelectedHero], heroSpawnPoint.transform.position, Quaternion.identity);
        CameraFollow.Instance.Target = heroInstance;
        CameraFollow.Instance.gameObject.GetComponent<AudioListener>().enabled = false;
    }
    private void Update()
    {
        if (!init || DisableSpawning)
            return;

        gameTime += Time.deltaTime;

        if (EnemiesSpawnCounter <= 0)
            return;

        if (gameTime < firstSpawnDelay)
            return;

        if (spawnTimer <= 0)
        {
            Spawn();
            spawnTimer = spawnTime;
        }
        else 
            spawnTimer -= Time.deltaTime;
    }

    private void Spawn()
    {
        int enemyId = Random.Range(0, enemies.Length);
        EnemiesSpawnCounter--;
        Vector2 spawnPoint = new Vector2(Random.Range(spawnZone.bounds.min.x + 1, spawnZone.bounds.max.x - 1), spawnZone.bounds.center.y);
        initEnemies.Add(Instantiate(enemies[enemyId], spawnPoint, Quaternion.identity));
        Debug.Log("Enemy " + enemyId + " was spawned at " + gameTime);
    }

    private void Reload()
    {
        foreach(Enemy enemy in initEnemies)
        {
            Destroy(enemy.gameObject);
        }
        initEnemies.Clear();
        EnemiesSpawnCounter = EnemiesLiveCounter;

        gameTime = 0f;
    }

    public void SetToDefault()
    {
        Reload();
        init = false;
    }

    public void OnEnemyDie()
    {
        EnemiesLiveCounter--;
        UIManager.Instance.Counter.text = EnemiesLiveCounter.ToString();
        if (EnemiesLiveCounter <= 0)
            GameManager.Instance.GameState = GameStates.WIN;
    }
}
