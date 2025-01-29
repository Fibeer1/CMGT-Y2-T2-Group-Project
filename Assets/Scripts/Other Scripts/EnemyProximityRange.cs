using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximityRange : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<Transform> enemies = new List<Transform>();
    
    private void Update()
    {
        FindClosestEnemy();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy collisionEnemy = other.GetComponent<Enemy>();
        if (collisionEnemy != null && !enemies.Contains(collisionEnemy.transform))
        {
            enemies.Add(collisionEnemy.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy collisionEnemy = other.GetComponent<Enemy>();
        if (collisionEnemy != null && enemies.Contains(collisionEnemy.transform))
        {
            enemies.Remove(collisionEnemy.transform);
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
        foreach (Transform enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }
            float distance = Vector3.Distance(player.transform.position, enemy.position);
            if (distance < closestEnemyDistance)
            {
                closestEnemyDistance = distance;
                player.closestEnemy = enemy.GetComponent<Enemy>();
            }
        }
    }
}
