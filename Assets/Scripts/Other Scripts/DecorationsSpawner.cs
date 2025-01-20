using UnityEngine;
using System.Collections.Generic;

public class DecorationsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(10f, 10f);
    [SerializeField] private int objectCount = 20;
    [SerializeField] private float minSpacing = 1f;

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition;

         
            int attempts = 0;
            do
            {
                randomPosition = GenerateRandomPosition();
                attempts++;
            }
            while (!IsPositionValid(randomPosition) && attempts < 100);

            if (attempts < 100)
            {
                spawnedPositions.Add(randomPosition);

              
                GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
                Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid position after 100 attempts. Try increasing the spawn area or reducing the object count.");
            }
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        return new Vector3(randomX, 0f, randomZ);
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minSpacing)
            {
                return false;
            }
        }
        return true;
    }
}
