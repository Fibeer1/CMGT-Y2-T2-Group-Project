using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform enemyUnitParent;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private int maxEnemies = 10;
    public List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private float minSpawnDistance;
    [SerializeField] private float maxSpawnDistance;
    [SerializeField] private float enemySpawnTime;
    [SerializeField] private float enemySpawnTimer;
    [SerializeField] private bool shouldSpawnEnemies = true;
    [SerializeField] private bool shouldDrawSpawnCircle = false;

    private void Start()
    {
        enemySpawnTimer = enemySpawnTime;
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
        SpawnEnemy(Random.Range(0, enemyPrefabs.Length));
    }

    private void SpawnEnemy(int enemyIndex)
    {
        if (enemies.Count >= maxEnemies || GameManager.enemies.Count >= GameManager.maxEnemies)
        {
            return;
        }
        //Spawn the enemy in a radius around the spawn location
        float angle = Random.Range(-360, 360);
        Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        Vector3 position = spawnLocation.position + (Random.Range(minSpawnDistance, maxSpawnDistance) * direction);
        position.y = 0.25f;

        GameObject enemyInstance = Instantiate(enemyPrefabs[enemyIndex], position, Quaternion.identity, enemyUnitParent);
        Enemy enemyScript = enemyInstance.GetComponent<Enemy>();
        enemies.Add(enemyScript);
        GameManager.enemies.Add(enemyScript);
        enemyScript.originSpawner = this;
    }

    private void OnDrawGizmos()
    {
        if (spawnLocation == null || !shouldDrawSpawnCircle)
        {
            return;
        }

        Gizmos.color = Color.red;
        DrawSpawnCircle(minSpawnDistance);
        Gizmos.color = Color.yellow;
        DrawSpawnCircle(maxSpawnDistance);
    }

    private void DrawSpawnCircle(float radius)
    {
        int segments = 100;
        float angleStep = 360f / segments;
        Vector3 previousPoint = spawnLocation.position + new Vector3(radius, 0, 0);

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = spawnLocation.position + 
                new Vector3(Mathf.Cos(angle) * radius, 0, 
                Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }
    }
}
