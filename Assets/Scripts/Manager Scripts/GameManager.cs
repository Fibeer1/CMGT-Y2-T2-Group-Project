using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int maxEnemies = 10;
    public static List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private int maxEnemiesValue = 30;

    private void Start()
    {
        maxEnemies = maxEnemiesValue;
    }

    private void Update()
    {
        
    }
}
