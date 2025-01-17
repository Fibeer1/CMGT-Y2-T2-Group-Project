using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximityRange : MonoBehaviour
{
    [SerializeField] private Player player;
    private List<Enemy> enemies = new List<Enemy>();
    
    private void Update()
    {
        FindClosestEnemy();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy collisionEnemy = other.GetComponent<Enemy>();
        if (collisionEnemy != null && !enemies.Contains(collisionEnemy))
        {
            enemies.Add(collisionEnemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy collisionEnemy = other.GetComponent<Enemy>();
        if (collisionEnemy != null && enemies.Contains(collisionEnemy))
        {
            enemies.Remove(collisionEnemy);
        }
    }

    private void FindClosestEnemy()
    {
        if (enemies.Count == 0)
        {
            player.closestEnemy = null;
            return;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }

        float closestEnemyDistance = Mathf.Infinity;
        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
            if (distance < closestEnemyDistance)
            {
                closestEnemyDistance = distance;
                player.closestEnemy = enemy;
            }
        }
    }
}
