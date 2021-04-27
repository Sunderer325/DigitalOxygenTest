using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerSpawnPoint;
    [Header("Enemies")]
    [SerializeField, Range(1, 100)] int enemiesMax;
    [SerializeField] Enemy[] enemies;
    [SerializeField, Min(0.1f)] float spawnTime;
    [SerializeField] BoxCollider2D spawnZone;

    int enemiesSpawnCounter;
    int enemiesDeathCounter;
    float spawnTimer;
    GameObject playerInstance;

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

    private void Start()
    {
        enemiesDeathCounter = enemiesSpawnCounter = Random.Range(1, enemiesMax);
        UIManager.Instance.counter.text = enemiesDeathCounter.ToString();

        playerInstance = Instantiate(player, playerSpawnPoint.transform.position, Quaternion.identity);
        CameraFollow.Instance.target = playerInstance;
    }
    private void Update()
    {
        if (enemiesSpawnCounter <= 0)
            return;

        if (spawnTimer <= 0)
        {
            Spawn();
            spawnTimer = spawnTime;
        }
        else spawnTimer -= Time.deltaTime;
    }

    private void Spawn()
    {
        int enemyId = Random.Range(0, enemies.Length);
        enemiesSpawnCounter--;
        Vector2 spawnPoint = new Vector2(Random.Range(spawnZone.bounds.min.x + 1, spawnZone.bounds.max.x - 1), spawnZone.bounds.center.y);
        Instantiate(enemies[enemyId], spawnPoint, Quaternion.identity);
    }

    public void OnEnemyDie()
    {
        enemiesDeathCounter--;
        UIManager.Instance.counter.text = enemiesDeathCounter.ToString();
        if (enemiesDeathCounter <= 0)
            GameManager.Instance.OnWin();
    }

    public void OnPlayerDie()
    {
        GameManager.Instance.OnLose();
    }
}
