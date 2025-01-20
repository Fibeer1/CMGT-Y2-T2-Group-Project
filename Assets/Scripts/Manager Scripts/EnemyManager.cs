using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform enemyUnitParent;
    [SerializeField] private Transform spawnCircle;
    private Player player;
    public static List<Enemy> enemies = new List<Enemy>();
    public static List<Entity> activeAllies = new List<Entity>();
    public float spawnDistance;
    [SerializeField] private float enemySpawnTime;
    private float enemySpawnTimer;
    [SerializeField] private bool shouldSpawnEnemies = false;

    private void Start()
    {
        enemySpawnTimer = enemySpawnTime;
        player = FindObjectOfType<Player>();
    }


    private void Update()
    {
        HandleSpawnTimer();
    }

    private void HandleSpawnTimer()
    {
        if (!shouldSpawnEnemies)
        {
            return;
        }
        enemySpawnTimer -= Time.deltaTime;
        if (enemySpawnTimer <= 0)
        {
            HandleEnemySpawning();
        }
    }

    private void HandleEnemySpawning()
    {
        enemySpawnTimer = enemySpawnTime;
    }

    private void SpawnEnemy(int enemyIndex)
    {
        //Spawn the enemy in a radius around the player
        float angle = Random.Range(-360, 360);
        Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        Vector3 position = player.transform.position + (spawnDistance * direction);

        GameObject enemyInstance = Instantiate(enemyPrefabs[enemyIndex], position, Quaternion.identity, enemyUnitParent);
        Enemy enemyScript = enemyInstance.GetComponent<Enemy>();
        enemies.Add(enemyScript);
    }
}
